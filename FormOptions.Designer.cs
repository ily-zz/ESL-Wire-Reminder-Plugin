namespace CSReminder
{
    partial class FormOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.movingComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.reminderComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxActivate = new System.Windows.Forms.CheckBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.SuspendLayout();
            // 
            // movingComboBox
            // 
            this.movingComboBox.FormattingEnabled = true;
            this.movingComboBox.Items.AddRange(new object[] {
            "Move demo file to target directory",
            "Copy demo file to target directory"});
            this.movingComboBox.Location = new System.Drawing.Point(141, 60);
            this.movingComboBox.Name = "movingComboBox";
            this.movingComboBox.Size = new System.Drawing.Size(256, 21);
            this.movingComboBox.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Completed demo file:";
            // 
            // reminderComboBox
            // 
            this.reminderComboBox.FormattingEnabled = true;
            this.reminderComboBox.Items.AddRange(new object[] {
            "10 seconds",
            "20 seconds",
            "30 seconds",
            "1 minute"});
            this.reminderComboBox.Location = new System.Drawing.Point(141, 33);
            this.reminderComboBox.Name = "reminderComboBox";
            this.reminderComboBox.Size = new System.Drawing.Size(256, 21);
            this.reminderComboBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Reminder Interval:";
            // 
            // checkBoxActivate
            // 
            this.checkBoxActivate.AutoSize = true;
            this.checkBoxActivate.Location = new System.Drawing.Point(16, 13);
            this.checkBoxActivate.Name = "checkBoxActivate";
            this.checkBoxActivate.Size = new System.Drawing.Size(113, 17);
            this.checkBoxActivate.TabIndex = 15;
            this.checkBoxActivate.Text = "Activate Reminder";
            this.checkBoxActivate.UseVisualStyleBackColor = true;
            this.checkBoxActivate.CheckedChanged += new System.EventHandler(this.checkBoxActivate_CheckedChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(241, 91);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 14;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(322, 91);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 126);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // FormOptions
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(406, 126);
            this.Controls.Add(this.movingComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reminderComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxActivate);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.splitter1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(422, 164);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(422, 164);
            this.Name = "FormOptions";
            this.ShowIcon = false;
            this.Text = "CS Reminder Plugin";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox movingComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox reminderComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxActivate;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Splitter splitter1;
    }
}