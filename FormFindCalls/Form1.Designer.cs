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
            this.timeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.timeTo = new System.Windows.Forms.DateTimePicker();
            this.timeRange = new System.Windows.Forms.CheckBox();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Label();
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
            this.lbl_kvp1.Location = new System.Drawing.Point(27, 83);
            this.lbl_kvp1.Name = "lbl_kvp1";
            this.lbl_kvp1.Size = new System.Drawing.Size(148, 13);
            this.lbl_kvp1.TabIndex = 3;
            this.lbl_kvp1.Text = "pcd1_ID (KVP) e.g. insurance";
            // 
            // lbl_kvp2
            // 
            this.lbl_kvp2.AutoSize = true;
            this.lbl_kvp2.Location = new System.Drawing.Point(44, 128);
            this.lbl_kvp2.Name = "lbl_kvp2";
            this.lbl_kvp2.Size = new System.Drawing.Size(128, 13);
            this.lbl_kvp2.TabIndex = 4;
            this.lbl_kvp2.Text = "pcd2_ID (KVP) e.g. credit";
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
            this.button1.Location = new System.Drawing.Point(9, 192);
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
            // timeFrom
            // 
            this.timeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeFrom.Location = new System.Drawing.Point(439, 162);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.ShowUpDown = true;
            this.timeFrom.Size = new System.Drawing.Size(101, 20);
            this.timeFrom.TabIndex = 8;
            this.timeFrom.Visible = false;
            // 
            // dateFrom
            // 
            this.dateFrom.Location = new System.Drawing.Point(371, 37);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(200, 20);
            this.dateFrom.TabIndex = 9;
            // 
            // timeTo
            // 
            this.timeTo.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeTo.Location = new System.Drawing.Point(439, 188);
            this.timeTo.Name = "timeTo";
            this.timeTo.ShowUpDown = true;
            this.timeTo.Size = new System.Drawing.Size(101, 20);
            this.timeTo.TabIndex = 14;
            this.timeTo.Visible = false;
            // 
            // timeRange
            // 
            this.timeRange.AutoSize = true;
            this.timeRange.Location = new System.Drawing.Point(373, 139);
            this.timeRange.Name = "timeRange";
            this.timeRange.Size = new System.Drawing.Size(87, 17);
            this.timeRange.TabIndex = 15;
            this.timeRange.Text = "Time Range:";
            this.timeRange.UseVisualStyleBackColor = true;
            this.timeRange.CheckedChanged += new System.EventHandler(this.timeRange_CheckedChanged);
            // 
            // dateTo
            // 
            this.dateTo.Location = new System.Drawing.Point(371, 81);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(200, 20);
            this.dateTo.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(404, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "To:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(270, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "Copy Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "From This Date/On This Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(374, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "To This Date:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(532, 464);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // test
            // 
            this.test.AutoSize = true;
            this.test.Location = new System.Drawing.Point(30, 464);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(57, 13);
            this.test.TabIndex = 26;
            this.test.Text = "Test Label";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 499);
            this.Controls.Add(this.test);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTo);
            this.Controls.Add(this.timeRange);
            this.Controls.Add(this.timeTo);
            this.Controls.Add(this.dateFrom);
            this.Controls.Add(this.timeFrom);
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
        private System.Windows.Forms.DateTimePicker timeFrom;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.DateTimePicker timeTo;
        private System.Windows.Forms.CheckBox timeRange;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label test;
    }
}

