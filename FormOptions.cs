using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSReminder
{
    public partial class FormOptions : Form
    {
        public delegate void OnReminderActivated(bool enabled);
        // The event
        public static event OnReminderActivated OnReminderEvent;

        public FormOptions()
        {
            InitializeComponent();
        }

        // Read Settings and set all options
        public void initAllItems()
        {
            checkBoxActivate.Checked = Properties.Settings.Default.Active;
            reminderComboBox.SelectedIndex = Properties.Settings.Default.ReminderInterval;
            movingComboBox.SelectedIndex = Properties.Settings.Default.MovingDemoFile;
        }

        private void checkBoxActivate_CheckedChanged(object sender, EventArgs e)
        {
            itemEnabled(checkBoxActivate.Checked);
        }

        private void itemEnabled(bool enable)
        {
            reminderComboBox.Enabled = enable;
            movingComboBox.Enabled = enable;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            saveSettings();

            Hide();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void saveSettings()
        {
            Properties.Settings.Default.Active = checkBoxActivate.Checked;
            Properties.Settings.Default.ReminderInterval = reminderComboBox.SelectedIndex;
            Properties.Settings.Default.MovingDemoFile = movingComboBox.SelectedIndex;

            // persistent save
            Properties.Settings.Default.Save();

            OnReminderEvent(Properties.Settings.Default.Active);
        }
    }
}
