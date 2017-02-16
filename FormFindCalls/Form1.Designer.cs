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
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Label();
            this.txtBxPaths = new System.Windows.Forms.TextBox();
            this.dropDownKvp = new System.Windows.Forms.ComboBox();
            this.txtBxKvp1 = new System.Windows.Forms.TextBox();
            this.tblToSearchDD = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // contactID
            // 
            this.contactID.Location = new System.Drawing.Point(236, 14);
            this.contactID.Margin = new System.Windows.Forms.Padding(4);
            this.contactID.Name = "contactID";
            this.contactID.Size = new System.Drawing.Size(132, 22);
            this.contactID.TabIndex = 0;
            // 
            // kvp1
            // 
            this.kvp1.Location = new System.Drawing.Point(236, 49);
            this.kvp1.Margin = new System.Windows.Forms.Padding(4);
            this.kvp1.Name = "kvp1";
            this.kvp1.Size = new System.Drawing.Size(132, 22);
            this.kvp1.TabIndex = 1;
            // 
            // lbl_contactID
            // 
            this.lbl_contactID.AutoSize = true;
            this.lbl_contactID.Location = new System.Drawing.Point(147, 17);
            this.lbl_contactID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_contactID.Name = "lbl_contactID";
            this.lbl_contactID.Size = new System.Drawing.Size(77, 17);
            this.lbl_contactID.TabIndex = 2;
            this.lbl_contactID.Text = "Contact_ID";
            // 
            // lbl_kvp1
            // 
            this.lbl_kvp1.AutoSize = true;
            this.lbl_kvp1.Location = new System.Drawing.Point(121, 53);
            this.lbl_kvp1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_kvp1.Name = "lbl_kvp1";
            this.lbl_kvp1.Size = new System.Drawing.Size(100, 17);
            this.lbl_kvp1.TabIndex = 3;
            this.lbl_kvp1.Text = "RG - p7_value";
            // 
            // lbl_kvp2
            // 
            this.lbl_kvp2.AutoSize = true;
            this.lbl_kvp2.Location = new System.Drawing.Point(59, 85);
            this.lbl_kvp2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_kvp2.Name = "lbl_kvp2";
            this.lbl_kvp2.Size = new System.Drawing.Size(168, 17);
            this.lbl_kvp2.TabIndex = 4;
            this.lbl_kvp2.Text = "pcd2_ID (KVP) e.g. credit";
            // 
            // kvp2
            // 
            this.kvp2.Location = new System.Drawing.Point(236, 81);
            this.kvp2.Margin = new System.Windows.Forms.Padding(4);
            this.kvp2.Name = "kvp2";
            this.kvp2.Size = new System.Drawing.Size(132, 22);
            this.kvp2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 236);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(303, 28);
            this.button1.TabIndex = 6;
            this.button1.Text = "Create File with path to call files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_paths
            // 
            this.lbl_paths.AutoSize = true;
            this.lbl_paths.Location = new System.Drawing.Point(20, 282);
            this.lbl_paths.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_paths.Name = "lbl_paths";
            this.lbl_paths.Size = new System.Drawing.Size(139, 17);
            this.lbl_paths.TabIndex = 7;
            this.lbl_paths.Text = "Paths will show here:";
            // 
            // dateFrom
            // 
            this.dateFrom.Location = new System.Drawing.Point(495, 46);
            this.dateFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(265, 22);
            this.dateFrom.TabIndex = 9;
            // 
            // dateTo
            // 
            this.dateTo.Location = new System.Drawing.Point(495, 100);
            this.dateTo.Margin = new System.Windows.Forms.Padding(4);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(265, 22);
            this.dateTo.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(360, 236);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 22;
            this.button2.Text = "Copy Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(495, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 17);
            this.label3.TabIndex = 23;
            this.label3.Text = "From This Date/On This Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(499, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 24;
            this.label4.Text = "To This Date:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(709, 571);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 25;
            this.button3.Text = "Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // test
            // 
            this.test.AutoSize = true;
            this.test.Location = new System.Drawing.Point(40, 571);
            this.test.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 17);
            this.test.TabIndex = 26;
            this.test.Text = "Test Label";
            // 
            // txtBxPaths
            // 
            this.txtBxPaths.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBxPaths.Location = new System.Drawing.Point(415, 282);
            this.txtBxPaths.Margin = new System.Windows.Forms.Padding(4);
            this.txtBxPaths.Multiline = true;
            this.txtBxPaths.Name = "txtBxPaths";
            this.txtBxPaths.Size = new System.Drawing.Size(448, 269);
            this.txtBxPaths.TabIndex = 27;
            // 
            // dropDownKvp
            // 
            this.dropDownKvp.FormattingEnabled = true;
            this.dropDownKvp.Location = new System.Drawing.Point(12, 186);
            this.dropDownKvp.Margin = new System.Windows.Forms.Padding(4);
            this.dropDownKvp.Name = "dropDownKvp";
            this.dropDownKvp.Size = new System.Drawing.Size(160, 24);
            this.dropDownKvp.Sorted = true;
            this.dropDownKvp.TabIndex = 28;
            // 
            // txtBxKvp1
            // 
            this.txtBxKvp1.Location = new System.Drawing.Point(191, 187);
            this.txtBxKvp1.Margin = new System.Windows.Forms.Padding(4);
            this.txtBxKvp1.Name = "txtBxKvp1";
            this.txtBxKvp1.Size = new System.Drawing.Size(132, 22);
            this.txtBxKvp1.TabIndex = 29;
            // 
            // tblToSearchDD
            // 
            this.tblToSearchDD.FormattingEnabled = true;
            this.tblToSearchDD.Location = new System.Drawing.Point(128, 130);
            this.tblToSearchDD.Margin = new System.Windows.Forms.Padding(4);
            this.tblToSearchDD.Name = "tblToSearchDD";
            this.tblToSearchDD.Size = new System.Drawing.Size(160, 24);
            this.tblToSearchDD.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 134);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 31;
            this.label1.Text = "Select a Table";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 764);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tblToSearchDD);
            this.Controls.Add(this.txtBxKvp1);
            this.Controls.Add(this.dropDownKvp);
            this.Controls.Add(this.txtBxPaths);
            this.Controls.Add(this.test);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dateTo);
            this.Controls.Add(this.dateFrom);
            this.Controls.Add(this.lbl_paths);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.kvp2);
            this.Controls.Add(this.lbl_kvp2);
            this.Controls.Add(this.lbl_kvp1);
            this.Controls.Add(this.lbl_contactID);
            this.Controls.Add(this.kvp1);
            this.Controls.Add(this.contactID);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label test;
        private System.Windows.Forms.TextBox txtBxPaths;
        private System.Windows.Forms.ComboBox dropDownKvp;
        private System.Windows.Forms.TextBox txtBxKvp1;
        private System.Windows.Forms.ComboBox tblToSearchDD;
        private System.Windows.Forms.Label label1;
    }
}

