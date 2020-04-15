namespace AutoDataScraper
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
            this.linksLabel = new System.Windows.Forms.Label();
            this.linksTextBox = new System.Windows.Forms.RichTextBox();
            this.dataFolderLabel = new System.Windows.Forms.Label();
            this.dataFolderBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.statusListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusUpdateButton = new System.Windows.Forms.Button();
            this.parsingStatusBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linksLabel
            // 
            this.linksLabel.AutoSize = true;
            this.linksLabel.Location = new System.Drawing.Point(13, 9);
            this.linksLabel.Name = "linksLabel";
            this.linksLabel.Size = new System.Drawing.Size(32, 13);
            this.linksLabel.TabIndex = 0;
            this.linksLabel.Text = "Links";
            // 
            // linksTextBox
            // 
            this.linksTextBox.Location = new System.Drawing.Point(16, 25);
            this.linksTextBox.Name = "linksTextBox";
            this.linksTextBox.Size = new System.Drawing.Size(456, 162);
            this.linksTextBox.TabIndex = 1;
            this.linksTextBox.Text = "";
            // 
            // dataFolderLabel
            // 
            this.dataFolderLabel.AutoSize = true;
            this.dataFolderLabel.Location = new System.Drawing.Point(16, 200);
            this.dataFolderLabel.Name = "dataFolderLabel";
            this.dataFolderLabel.Size = new System.Drawing.Size(59, 13);
            this.dataFolderLabel.TabIndex = 2;
            this.dataFolderLabel.Text = "Data folder";
            // 
            // dataFolderBox
            // 
            this.dataFolderBox.Location = new System.Drawing.Point(16, 216);
            this.dataFolderBox.Name = "dataFolderBox";
            this.dataFolderBox.Size = new System.Drawing.Size(456, 20);
            this.dataFolderBox.TabIndex = 3;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(16, 251);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(97, 251);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 5;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // statusListBox
            // 
            this.statusListBox.FormattingEnabled = true;
            this.statusListBox.Location = new System.Drawing.Point(16, 302);
            this.statusListBox.Name = "statusListBox";
            this.statusListBox.Size = new System.Drawing.Size(140, 147);
            this.statusListBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Parsing status";
            // 
            // statusUpdateButton
            // 
            this.statusUpdateButton.Location = new System.Drawing.Point(178, 251);
            this.statusUpdateButton.Name = "statusUpdateButton";
            this.statusUpdateButton.Size = new System.Drawing.Size(94, 23);
            this.statusUpdateButton.TabIndex = 10;
            this.statusUpdateButton.Text = "Update Status";
            this.statusUpdateButton.UseVisualStyleBackColor = true;
            this.statusUpdateButton.Click += new System.EventHandler(this.StatusUpdateButton_Click);
            // 
            // parsingStatusBox
            // 
            this.parsingStatusBox.FormattingEnabled = true;
            this.parsingStatusBox.Location = new System.Drawing.Point(162, 302);
            this.parsingStatusBox.Name = "parsingStatusBox";
            this.parsingStatusBox.Size = new System.Drawing.Size(310, 147);
            this.parsingStatusBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Data status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.parsingStatusBox);
            this.Controls.Add(this.statusUpdateButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusListBox);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataFolderBox);
            this.Controls.Add(this.dataFolderLabel);
            this.Controls.Add(this.linksTextBox);
            this.Controls.Add(this.linksLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Data Scraper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label linksLabel;
        private System.Windows.Forms.RichTextBox linksTextBox;
        private System.Windows.Forms.Label dataFolderLabel;
        private System.Windows.Forms.TextBox dataFolderBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.ListBox statusListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button statusUpdateButton;
        private System.Windows.Forms.ListBox parsingStatusBox;
        private System.Windows.Forms.Label label1;
    }
}

