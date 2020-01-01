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
            this.CompareVidsButton = new System.Windows.Forms.Button();
            this.SortByType = new System.Windows.Forms.Button();
            this.renameVidsBtn = new System.Windows.Forms.Button();
            this.mergeHEIC = new System.Windows.Forms.Button();
            this.removeShortVids = new System.Windows.Forms.Button();
            this.handleHEVC = new System.Windows.Forms.Button();
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
            this.btnGo.Size = new System.Drawing.Size(125, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "Sort By Date";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select a destination";
            // 
            // txtDest
            // 
            this.txtDest.Location = new System.Drawing.Point(118, 65);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(295, 20);
            this.txtDest.TabIndex = 5;
            // 
            // btnBrowseDest
            // 
            this.btnBrowseDest.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnBrowseDest.Location = new System.Drawing.Point(427, 65);
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
            this.CompareButton.Location = new System.Drawing.Point(363, 124);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(125, 23);
            this.CompareButton.TabIndex = 10;
            this.CompareButton.Text = "Compare Pics";
            this.CompareButton.UseVisualStyleBackColor = true;
            // 
            // CompareVidsButton
            // 
            this.CompareVidsButton.Location = new System.Drawing.Point(363, 158);
            this.CompareVidsButton.Name = "CompareVidsButton";
            this.CompareVidsButton.Size = new System.Drawing.Size(125, 23);
            this.CompareVidsButton.TabIndex = 11;
            this.CompareVidsButton.Text = "Compare Vids";
            this.CompareVidsButton.UseVisualStyleBackColor = true;
            this.CompareVidsButton.Click += new System.EventHandler(this.CompareVidsButton_Click);
            // 
            // SortByType
            // 
            this.SortByType.Location = new System.Drawing.Point(221, 124);
            this.SortByType.Name = "SortByType";
            this.SortByType.Size = new System.Drawing.Size(125, 23);
            this.SortByType.TabIndex = 12;
            this.SortByType.Text = "Sort By Type";
            this.SortByType.UseVisualStyleBackColor = true;
            this.SortByType.Click += new System.EventHandler(this.SortByType_Click);
            // 
            // renameVidsBtn
            // 
            this.renameVidsBtn.Location = new System.Drawing.Point(221, 192);
            this.renameVidsBtn.Name = "renameVidsBtn";
            this.renameVidsBtn.Size = new System.Drawing.Size(125, 23);
            this.renameVidsBtn.TabIndex = 13;
            this.renameVidsBtn.Text = "Rename Vids";
            this.renameVidsBtn.UseVisualStyleBackColor = true;
            this.renameVidsBtn.Click += new System.EventHandler(this.renameVidsBtn_Click);
            // 
            // mergeHEIC
            // 
            this.mergeHEIC.Location = new System.Drawing.Point(363, 192);
            this.mergeHEIC.Name = "mergeHEIC";
            this.mergeHEIC.Size = new System.Drawing.Size(125, 23);
            this.mergeHEIC.TabIndex = 14;
            this.mergeHEIC.Text = "Merge HEIC";
            this.mergeHEIC.UseVisualStyleBackColor = true;
            this.mergeHEIC.Click += new System.EventHandler(this.mergeHEIC_Click);
            // 
            // removeShortVids
            // 
            this.removeShortVids.Location = new System.Drawing.Point(221, 227);
            this.removeShortVids.Name = "removeShortVids";
            this.removeShortVids.Size = new System.Drawing.Size(125, 23);
            this.removeShortVids.TabIndex = 15;
            this.removeShortVids.Text = "Remove Short Vids";
            this.removeShortVids.UseVisualStyleBackColor = true;
            this.removeShortVids.Click += new System.EventHandler(this.removeShortVids_Click);
            // 
            // handleHEVC
            // 
            this.handleHEVC.Location = new System.Drawing.Point(363, 228);
            this.handleHEVC.Name = "handleHEVC";
            this.handleHEVC.Size = new System.Drawing.Size(125, 23);
            this.handleHEVC.TabIndex = 16;
            this.handleHEVC.Text = "Handle HEVC";
            this.handleHEVC.UseVisualStyleBackColor = true;
            this.handleHEVC.Click += new System.EventHandler(this.handleHEVC_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 263);
            this.Controls.Add(this.handleHEVC);
            this.Controls.Add(this.removeShortVids);
            this.Controls.Add(this.mergeHEIC);
            this.Controls.Add(this.renameVidsBtn);
            this.Controls.Add(this.SortByType);
            this.Controls.Add(this.CompareVidsButton);
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
        private System.Windows.Forms.Button CompareVidsButton;
        private System.Windows.Forms.Button SortByType;
        private System.Windows.Forms.Button renameVidsBtn;
        private System.Windows.Forms.Button mergeHEIC;
        private System.Windows.Forms.Button removeShortVids;
        private System.Windows.Forms.Button handleHEVC;
    }
}

