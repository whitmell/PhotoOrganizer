namespace PhotoOrganizer
{
    partial class frmMain
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDest = new System.Windows.Forms.TextBox();
            this.btnBrowseDest = new System.Windows.Forms.Button();
            this.Message = new System.Windows.Forms.Label();
            this.copyCheck = new System.Windows.Forms.CheckBox();
            this.CompareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(117, 25);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(295, 20);
            this.txtSource.TabIndex = 0;
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(427, 25);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSource.TabIndex = 1;
            this.btnBrowseSource.Text = "Browse...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a source";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(221, 158);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "GO!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select a destination";
            // 
            // txtDest
            // 
            this.txtDest.Location = new System.Drawing.Point(118, 80);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(295, 20);
            this.txtDest.TabIndex = 5;
            // 
            // btnBrowseDest
            // 
            this.btnBrowseDest.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnBrowseDest.Location = new System.Drawing.Point(427, 80);
            this.btnBrowseDest.Name = "btnBrowseDest";
            this.btnBrowseDest.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDest.TabIndex = 6;
            this.btnBrowseDest.Text = "Browse...";
            this.btnBrowseDest.UseVisualStyleBackColor = true;
            this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(16, 129);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(19, 13);
            this.Message.TabIndex = 8;
            this.Message.Text = "lml";
            // 
            // copyCheck
            // 
            this.copyCheck.AutoSize = true;
            this.copyCheck.Checked = true;
            this.copyCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyCheck.Location = new System.Drawing.Point(19, 158);
            this.copyCheck.Name = "copyCheck";
            this.copyCheck.Size = new System.Drawing.Size(86, 17);
            this.copyCheck.TabIndex = 9;
            this.copyCheck.Text = "Make Copy?";
            this.copyCheck.UseVisualStyleBackColor = true;
            // 
            // CompareButton
            // 
            this.CompareButton.Location = new System.Drawing.Point(427, 157);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(75, 23);
            this.CompareButton.TabIndex = 10;
            this.CompareButton.Text = "Compare";
            this.CompareButton.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 193);
            this.Controls.Add(this.CompareButton);
            this.Controls.Add(this.copyCheck);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.btnBrowseDest);
            this.Controls.Add(this.txtDest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseSource);
            this.Controls.Add(this.txtSource);
            this.Name = "frmMain";
            this.Text = "Photo Organizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDest;
        private System.Windows.Forms.Button btnBrowseDest;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.CheckBox copyCheck;
        private System.Windows.Forms.Button CompareButton;
    }
}

