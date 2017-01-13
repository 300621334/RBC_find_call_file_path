namespace FormFindCalls
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
            this.contactID = new System.Windows.Forms.TextBox();
            this.kvp1 = new System.Windows.Forms.TextBox();
            this.lbl_contactID = new System.Windows.Forms.Label();
            this.lbl_kvp1 = new System.Windows.Forms.Label();
            this.lbl_kvp2 = new System.Windows.Forms.Label();
            this.kvp2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_paths = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // contactID
            // 
            this.contactID.Location = new System.Drawing.Point(177, 38);
            this.contactID.Name = "contactID";
            this.contactID.Size = new System.Drawing.Size(100, 20);
            this.contactID.TabIndex = 0;
            // 
            // kvp1
            // 
            this.kvp1.Location = new System.Drawing.Point(177, 80);
            this.kvp1.Name = "kvp1";
            this.kvp1.Size = new System.Drawing.Size(100, 20);
            this.kvp1.TabIndex = 1;
            // 
            // lbl_contactID
            // 
            this.lbl_contactID.AutoSize = true;
            this.lbl_contactID.Location = new System.Drawing.Point(110, 41);
            this.lbl_contactID.Name = "lbl_contactID";
            this.lbl_contactID.Size = new System.Drawing.Size(61, 13);
            this.lbl_contactID.TabIndex = 2;
            this.lbl_contactID.Text = "Contact_ID";
            // 
            // lbl_kvp1
            // 
            this.lbl_kvp1.AutoSize = true;
            this.lbl_kvp1.Location = new System.Drawing.Point(53, 83);
            this.lbl_kvp1.Name = "lbl_kvp1";
            this.lbl_kvp1.Size = new System.Drawing.Size(118, 13);
            this.lbl_kvp1.TabIndex = 3;
            this.lbl_kvp1.Text = "pcd1_ID (KVP) e.g. RG";
            // 
            // lbl_kvp2
            // 
            this.lbl_kvp2.AutoSize = true;
            this.lbl_kvp2.Location = new System.Drawing.Point(12, 128);
            this.lbl_kvp2.Name = "lbl_kvp2";
            this.lbl_kvp2.Size = new System.Drawing.Size(159, 13);
            this.lbl_kvp2.TabIndex = 4;
            this.lbl_kvp2.Text = "pcd2_ID (KVP) e.g. call purpose";
            // 
            // kvp2
            // 
            this.kvp2.Location = new System.Drawing.Point(177, 125);
            this.kvp2.Name = "kvp2";
            this.kvp2.Size = new System.Drawing.Size(100, 20);
            this.kvp2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(50, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(227, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Create File with path to call files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_paths
            // 
            this.lbl_paths.AutoSize = true;
            this.lbl_paths.Location = new System.Drawing.Point(15, 229);
            this.lbl_paths.Name = "lbl_paths";
            this.lbl_paths.Size = new System.Drawing.Size(107, 13);
            this.lbl_paths.TabIndex = 7;
            this.lbl_paths.Text = "Paths ro call-files are:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 499);
            this.Controls.Add(this.lbl_paths);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.kvp2);
            this.Controls.Add(this.lbl_kvp2);
            this.Controls.Add(this.lbl_kvp1);
            this.Controls.Add(this.lbl_contactID);
            this.Controls.Add(this.kvp1);
            this.Controls.Add(this.contactID);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox contactID;
        private System.Windows.Forms.TextBox kvp1;
        private System.Windows.Forms.Label lbl_contactID;
        private System.Windows.Forms.Label lbl_kvp1;
        private System.Windows.Forms.Label lbl_kvp2;
        private System.Windows.Forms.TextBox kvp2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_paths;
    }
}

