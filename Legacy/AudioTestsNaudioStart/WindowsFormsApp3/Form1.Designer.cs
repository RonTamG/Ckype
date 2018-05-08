namespace WindowsFormsApp3
{
    partial class Form1
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
            this.PlayButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.RecordButton = new System.Windows.Forms.Button();
            this.StopRecordButton = new System.Windows.Forms.Button();
            this.OutputText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(30, 22);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(75, 23);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(168, 21);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // RecordButton
            // 
            this.RecordButton.Location = new System.Drawing.Point(30, 79);
            this.RecordButton.Name = "RecordButton";
            this.RecordButton.Size = new System.Drawing.Size(75, 23);
            this.RecordButton.TabIndex = 2;
            this.RecordButton.Text = "Record";
            this.RecordButton.UseVisualStyleBackColor = true;
            this.RecordButton.Click += new System.EventHandler(this.RecordButton_Click);
            // 
            // StopRecordButton
            // 
            this.StopRecordButton.Enabled = false;
            this.StopRecordButton.Location = new System.Drawing.Point(158, 79);
            this.StopRecordButton.Name = "StopRecordButton";
            this.StopRecordButton.Size = new System.Drawing.Size(101, 23);
            this.StopRecordButton.TabIndex = 3;
            this.StopRecordButton.Text = "Stop Recording";
            this.StopRecordButton.UseVisualStyleBackColor = true;
            this.StopRecordButton.Click += new System.EventHandler(this.StopRecordButton_Click);
            // 
            // OutputText
            // 
            this.OutputText.Location = new System.Drawing.Point(30, 126);
            this.OutputText.Multiline = true;
            this.OutputText.Name = "OutputText";
            this.OutputText.Size = new System.Drawing.Size(229, 97);
            this.OutputText.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.OutputText);
            this.Controls.Add(this.StopRecordButton);
            this.Controls.Add(this.RecordButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.PlayButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClose);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button RecordButton;
        private System.Windows.Forms.Button StopRecordButton;
        private System.Windows.Forms.TextBox OutputText;
    }
}

