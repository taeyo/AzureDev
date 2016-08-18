namespace AzureStorageUpload
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
            this.uploadButton = new System.Windows.Forms.Button();
            this.Log = new System.Windows.Forms.Label();
            this.timespent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AccountKey = new System.Windows.Forms.TextBox();
            this.AccountName = new System.Windows.Forms.TextBox();
            this.ConnectState = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Connect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.FilePath = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ThreadNum = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadButton
            // 
            this.uploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadButton.Enabled = false;
            this.uploadButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadButton.Location = new System.Drawing.Point(486, 311);
            this.uploadButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(107, 64);
            this.uploadButton.TabIndex = 0;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // Log
            // 
            this.Log.AutoSize = true;
            this.Log.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Log.Location = new System.Drawing.Point(16, 312);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(127, 23);
            this.Log.TabIndex = 1;
            this.Log.Text = "Bytes uploaded";
            // 
            // timespent
            // 
            this.timespent.AutoSize = true;
            this.timespent.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timespent.Location = new System.Drawing.Point(14, 354);
            this.timespent.Name = "timespent";
            this.timespent.Size = new System.Drawing.Size(161, 21);
            this.timespent.TabIndex = 1;
            this.timespent.Text = "Total Elapsed Time :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(490, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Azure Storage Data Movement Library(v0.2.0)을 사용하여 제작함";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(18, 33);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(336, 15);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/Azure/azure-storage-net-data-movement";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.AccountKey);
            this.groupBox1.Controls.Add(this.AccountName);
            this.groupBox1.Controls.Add(this.ConnectState);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Connect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(18, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 167);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Storage Connection";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(141, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(408, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "mycontainer";
            // 
            // AccountKey
            // 
            this.AccountKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountKey.Location = new System.Drawing.Point(141, 62);
            this.AccountKey.Name = "AccountKey";
            this.AccountKey.Size = new System.Drawing.Size(408, 23);
            this.AccountKey.TabIndex = 1;
            this.AccountKey.Text = "****";
            // 
            // AccountName
            // 
            this.AccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountName.Location = new System.Drawing.Point(141, 30);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(408, 23);
            this.AccountName.TabIndex = 1;
            this.AccountName.Text = "delmeplz";
            // 
            // ConnectState
            // 
            this.ConnectState.AutoSize = true;
            this.ConnectState.ForeColor = System.Drawing.Color.Red;
            this.ConnectState.Location = new System.Drawing.Point(24, 129);
            this.ConnectState.Name = "ConnectState";
            this.ConnectState.Size = new System.Drawing.Size(131, 15);
            this.ConnectState.TabIndex = 0;
            this.ConnectState.Text = "* 연결되지 않았습니다.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Container Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Account Key";
            // 
            // Connect
            // 
            this.Connect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Connect.Location = new System.Drawing.Point(466, 123);
            this.Connect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(82, 26);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Account Name";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(153, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "File to upload";
            // 
            // FilePath
            // 
            this.FilePath.BackColor = System.Drawing.Color.AliceBlue;
            this.FilePath.Location = new System.Drawing.Point(250, 241);
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Size = new System.Drawing.Size(254, 23);
            this.FilePath.TabIndex = 1;
            // 
            // Browse
            // 
            this.Browse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Browse.Location = new System.Drawing.Point(508, 238);
            this.Browse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(83, 26);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse..";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(26, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Thread";
            // 
            // ThreadNum
            // 
            this.ThreadNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ThreadNum.Location = new System.Drawing.Point(82, 241);
            this.ThreadNum.Name = "ThreadNum";
            this.ThreadNum.Size = new System.Drawing.Size(46, 23);
            this.ThreadNum.TabIndex = 1;
            this.ThreadNum.Text = "4";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(21, 278);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(570, 18);
            this.progressBar1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 387);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ThreadNum);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timespent);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.uploadButton);
            this.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Fast Blob Uploader(Azure Storage)";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Label Log;
        private System.Windows.Forms.Label timespent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AccountKey;
        private System.Windows.Forms.TextBox AccountName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Label ConnectState;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ThreadNum;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

