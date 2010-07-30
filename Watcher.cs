using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using DbMon.NET;

namespace CSReminder
{
    enum PlayerState
    {
        Invalid = 0,
        DemoStarted,
        DemoStopped,
        TeamChanged
    }

    class Watcher
    {
        enum TeamSide
        {
            None = 0,
            Terrorist,
            Counterterrorist
        }

        private List<string> JoiningTerroristTeam = new List<string>();
        private List<string> JoiningCounterTeam = new List<string>();

        static string DemoStarted = "recording to";
        static string DemoCompleted = "Completed demo";
        static string CSHalfLifeExe = "hl.exe";
        static string CSHalfLife = "hl";

        private string m_CSPath = "";
        private string m_DemoFileName = "";

        private bool m_running = false;

        public delegate void OnPlayerEventHandler(PlayerState currentState);

        // The event
        public static event OnPlayerEventHandler OnPlayerEvent;

        public Watcher()
        {
            // Open the language config file, the file is located in the same folder of the plugin dll
            string languageConfigPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!languageConfigPath.EndsWith("\\"))
            {
                languageConfigPath += "\\";
            }

            languageConfigPath += "LanguageConfig.txt";
            StreamReader sr;

            try
            {
                sr = new StreamReader(languageConfigPath);
            }
            catch
            {
                // Error no file found we interrupt here
                return;
            }
            string line = "";

            TeamSide newSide = TeamSide.None;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "[team terrorist]")
                {
                    newSide = TeamSide.Terrorist;
                    continue;
                }
                else if (line == "[team counterterrorist]")
                {
                    newSide = TeamSide.Counterterrorist;
                    continue;
                }
                else if (line.Length < 1)
                {
                    continue;
                }

                switch (newSide)
                {
                    case TeamSide.Terrorist:
                        JoiningTerroristTeam.Add(line);
                        break;
                    case TeamSide.Counterterrorist:
                        JoiningCounterTeam.Add(line);
                        break;
                    default:
                        break;
                }
            }

            DebugMonitor.OnOutputDebugString += new OnOutputDebugStringHandler(DebugMonitor_OnOutputDebugString);
        }

        public string GamePath
        {
            get { return m_CSPath; }
        }

        public string DemoFileName
        {
            get { return m_DemoFileName; }
        }

        public bool Running
        {
            get { return m_running; }
        }

        public void StartWatcher()
        {
            DebugMonitor.Start();
            m_running = true;
        }

        public void StopWatcher()
        {
            DebugMonitor.Stop();
            m_running = false;
        }

        private void DebugMonitor_OnOutputDebugString(int pid, string text)
        {
            if (Regex.IsMatch(text, DemoStarted))
            {
                string pattern = @"\b(\w+).dem\b";
                Regex fileReg = new Regex(pattern, RegexOptions.IgnoreCase);
                string wholeFileName = fileReg.Match(text).ToString();
                try
                {
                    m_DemoFileName = wholeFileName.Remove(wholeFileName.Length - 4, 4);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    m_DemoFileName = wholeFileName;
                }

                // Demo started, retrieve the directory of the executable object
                if (m_CSPath.Length < 1)
                {
                    m_CSPath = GetCSPath(pid);
                }

                FireOnPlayerEvent(PlayerState.DemoStarted);
            }
            else if (Regex.IsMatch(text, DemoCompleted))
            {
                FireOnPlayerEvent(PlayerState.DemoStopped);
            }
            else
            {
                foreach (string entry in JoiningTerroristTeam)
                {
                    if (Regex.IsMatch(text, entry))
                    {
                        FireOnPlayerEvent(PlayerState.TeamChanged);
                        return;
                    }
                }

                foreach (string entry in JoiningCounterTeam)
                {
                    if (Regex.IsMatch(text, entry))
                    {
                        FireOnPlayerEvent(PlayerState.TeamChanged);
                        return;
                    }
                }
            }
        }

        // Function to get the CS executable path
        private string GetCSPath(int pid)
        {
            string processName;
            Process currentProcess;

            if (pid < 1)
            {
                currentProcess = Process.GetCurrentProcess();
                processName = currentProcess.ProcessName;
            }
            else
            {
                try
                {
                    currentProcess = Process.GetProcessById(pid);
                    processName = currentProcess.ProcessName;
                }
                catch
                {
                    return "";
                }
            }

            if (Regex.IsMatch(processName, CSHalfLife) || Regex.IsMatch(processName, CSHalfLifeExe))
            {
                return Path.GetDirectoryName(currentProcess.MainModule.FileName);
            }
            else
            {
                return "";
            }
        }

        // Each time a debugoutput is recognized, a event is fired.
        private static void FireOnPlayerEvent(PlayerState state)
        {
            // Raise event if we have any listeners
            if (OnPlayerEvent == null)
            {
                return;
            }

            try
            {
                OnPlayerEvent(state);
            }
            catch
            {
                return;
            }
        }
    }
}
