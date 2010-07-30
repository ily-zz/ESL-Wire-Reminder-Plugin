using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Wire;

namespace CSReminder
{
    public class Reminder : Wire.Plugin
    {
        private GameInterface m_gameInterface;
        private FormOptions m_optionsDialog;
        private Watcher m_watch;
        private System.Threading.Timer m_timer;
        private bool m_demoIsRunning;
        private bool m_runningTimer = false;
        private int m_matchId;

        private const int dueReminderTime = 3000; // 3 seconds

        public override string Author
        {
            get { return "Turtle Entertainment GmbH"; }
        }

        public override string Title
        {
            get { return "CS Reminder Plugin"; }
        }

        public override string Version
        {
            get { return "0.1.1000"; }
        }

        // Init the plugin
        public override void init()
        {
            // We need the match start and the match stop signal
            m_gameInterface = InterfaceFactory.gameInterface();
            m_gameInterface.MatchStarted += new GameInterface.MatchStartedHandler(gi_MatchStarted);
            m_gameInterface.MatchEnded += new GameInterface.MatchEndedHandler(gi_MatchEnded);

            // Set the option dialog sytle
            System.Windows.Forms.Application.EnableVisualStyles();

            // Instanciate the form dialog member, read persistent settings and hide the options dialog
            m_optionsDialog = new FormOptions();
            m_optionsDialog.initAllItems();
            m_optionsDialog.Hide();

            // Listen to the form dialog event "OnReminderActivated", that means activation sttaus of the plugin
            // has been changed
            FormOptions.OnReminderEvent += new FormOptions.OnReminderActivated(Reminder_OnActivatedEvent);

            // show icon
            showIcon(Properties.Settings.Default.Active);
            setTooltip("CS Reminder Plugin");

            // Watcher class, which observes the debug output of the CS process
            m_watch = new Watcher();
            Watcher.OnPlayerEvent += new Watcher.OnPlayerEventHandler(Player_OnPlayerEvent);


            // Timer for notification
            TimerCallback tcb = CheckStatus;
            m_timer = new System.Threading.Timer(tcb);
            StopTimer();
        }

        // Plugin icon in Wire has been clicked, show the options dialog
        public override void iconClicked(int x, int y, Plugin.MouseButton button)
        {
            switch (button)
            {
                // Show the dialog if left or right button is clicked
                case MouseButton.LeftButton:
                case MouseButton.RightButton:
                    try
                    {
                        m_optionsDialog.initAllItems();
                        m_optionsDialog.Show();
                        m_optionsDialog.Activate();
                    }
                    catch (System.ObjectDisposedException)
                    {
                        // Our form is destroyed, create new one
                        m_optionsDialog = new FormOptions();
                        m_optionsDialog.initAllItems();
                        m_optionsDialog.Show();
                        m_optionsDialog.Activate();
                    }
                    break;
                // Toggle activation with midbutton
                case MouseButton.MidButton:
                    bool currentState = Properties.Settings.Default.Active;
                    Properties.Settings.Default.Active = !currentState;

                    // persistent save
                    Properties.Settings.Default.Save();
                    showIcon(Properties.Settings.Default.Active);

                    break;
                default:
                    break;
            }
        }

        // Wire is closing
        public override void onExit()
        {
            // Stop watcher thread if necessary
            if ((m_watch != null) && m_watch.Running)
            {
                m_watch.StopWatcher();
            }
        }

        // Match has been stopped. Check if it was a CS match and stop the watcher
        void gi_MatchEnded(int matchId)
        {
            StopTimer();
            if ((m_watch != null) && m_watch.Running)
            {
                m_watch.StopWatcher();
            }
        }

        // Match has been started. Check if it is a CS match and start the watcher
        void gi_MatchStarted(int matchId, string matchMediaPath)
        {
            var matchInfo = m_gameInterface.matchInfo(matchId);
            int gameId = 0;

            try
            {
                gameId = (int) matchInfo["gameId"];
            }
            catch
            {
                return;
            }

            // check for CS 1.6
            if (gameId != 61)
            {
                return;
            }

            m_matchId = matchId;

            if (Properties.Settings.Default.Active && (m_watch != null) && (!m_watch.Running))
            {
                m_watch.StartWatcher();
            }
        }

        // Change the Wire icon, based on the activation status
        private void Reminder_OnActivatedEvent(bool enabled)
        {
            showIcon(enabled);
        }

        // Parse the debug output for specific CS outputs
        private void Player_OnPlayerEvent(PlayerState state)
        {
            if ((state == PlayerState.DemoStopped) && (m_watch != null))
            {
                string gamePath = GetValidPath(m_watch.DemoFileName);

                if (gamePath.Length == 0)
                {
                    // File not found
                    return;
                }

                try
                {
                    // The demo file is moved or copied through the Wire plugin interface
                    if (Properties.Settings.Default.MovingDemoFile == 0)
                    {
                        // Moving the file
                        m_gameInterface.moveToMatchMedia(gamePath, m_matchId);
                        m_gameInterface.showInGameNotification("CS Reminder",
                            "Demo file has been moved to match media folder.", dueReminderTime);
                    }
                    else
                    {
                        // Copy the file
                        m_gameInterface.copyToMatchMedia(gamePath, m_matchId);
                        m_gameInterface.showInGameNotification("CS Reminder",
                            "Demo file has been copied to match media folder.", dueReminderTime);
                    }
                }
                catch
                {
                    return;
                }

                m_demoIsRunning = false;
            }
            else if (state == PlayerState.DemoStarted)
            {
                m_demoIsRunning = true;
                StopTimer();

                // Show In-Game notification
                string message = "Demo file " + m_watch.DemoFileName + " is recording";
                m_gameInterface.showInGameNotification("CS Reminder", message, dueReminderTime);
            }
            else if (state == PlayerState.TeamChanged)
            {
                if (m_demoIsRunning)
                {
                    // User must stop and start a new demo
                    StartTimer(dueReminderTime);

                    // Show In-Game notification
                    m_gameInterface.showInGameNotification("CS Reminder",
                        "Detected a Team Change while recording a demo.", dueReminderTime);
                }
                else
                {
                    // User must start a new demo, the timer will be triggered after 3 secs and then every
                    // minute
                    StartTimer(dueReminderTime);

                    // Show In-Game notification
                    m_gameInterface.showInGameNotification("CS Reminder", "Team Changed", dueReminderTime);
                }
            }
        }

        // Find the CS demo file in the CS Folder
        private string GetValidPath(string filename)
        {
            if (m_watch == null)
            {
                return "";
            }

            string parentDir = m_watch.GamePath + "\\";

            string[] directories = Directory.GetDirectories(parentDir, "cstrike*");
            foreach (string dir in directories)
            {
                string validFilePath = dir + "\\" + filename;
                if (!validFilePath.EndsWith(".dem"))
                {
                    validFilePath += ".dem";
                }

                if (File.Exists(validFilePath))
                {
                    return validFilePath;
                }
            }

            return "";
        }

        // Stop the notification timer
        public void StopTimer()
        {
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            m_runningTimer = false;
        }

        // Start the notification timer
        public void StartTimer(int dueTime)
        {
            int periodTime = 20000;

            switch (Properties.Settings.Default.ReminderInterval)
            {
                case 0:
                    periodTime = 10000;
                    break;
                case 1:
                    periodTime = 20000;
                    break;
                case 2:
                    periodTime = 30000;
                    break;
                case 3:
                    periodTime = 60000;
                    break;
                default:
                    periodTime = 20000;
                    break;
            }

            m_timer.Change(dueTime, periodTime);
            m_runningTimer = true;
        }

        // Reminds the player
        private void CheckStatus(Object stateInfo)
        {
            if (m_runningTimer)
            {
                // Show In-Game notification
                System.Media.SystemSounds.Beep.Play();
                m_gameInterface.showInGameNotification("CS Reminder", "Don't forget to record a demo.",
                    dueReminderTime);
            }
        }

        private void showIcon(bool enabled)
        {
            if (enabled)
            {
                setIcon("icon.png");
            }
            else
            {
                setIcon("noIcon.png");
            }
        }
    }
}
