using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.IO;

namespace FormFindCalls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /*
         */

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string query;

        private void button1_Click(object sender, EventArgs e)
        {
            lbl_paths.Text = "Paths ro call-files are:";

            query = "select * from rbc_contacts";
            //query += string.IsNullOrWhiteSpace(contactID.Text) ? "" : " where contact_ID = \"" + contactID.Text + "\"";
            //query += string.IsNullOrWhiteSpace(kvp1.Text) ? "" : " where pcd1_value = \"" + kvp1.Text + "\"";
            //query += string.IsNullOrWhiteSpace(kvp2.Text) ? "" : " where  pcd2_value = \"" + kvp2.Text + "\"";

            //string query = "select * from rbc_contacts where pcd1_value = " + kvp1.Text;//error because no quotes around value of kvp1.Text
            //string query = "select * from rbc_contacts where pcd1_value = @searchWord";
            string searchWord = "";
            if(!string.IsNullOrWhiteSpace(contactID.Text))
            {
                query += " where contact_ID = @searchWord";
                searchWord = contactID.Text;
            }
            else if(!string.IsNullOrWhiteSpace(kvp1.Text))
            {
                query += " where pcd1_value = @searchWord";
                searchWord = kvp1.Text;
            }
            else if(!string.IsNullOrWhiteSpace(kvp2.Text))
            {
                query += " where  pcd2_value = @searchWord";
                searchWord = kvp2.Text;
            }

            

            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RBC_call_path;Integrated Security=True";
            con = new SqlConnection(conStr);
            //SqlCommand cmd = new SqlCommand("select * from rbc_contacts", con);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("searchWord", searchWord);

            con.Open();
             reader = cmd.ExecuteReader();

            lbl_paths.Text += "\n";

            File.WriteAllText(@"C:\Users\Zoya\Google Drive\RBC\Form_find_contacts\paths.txt", string.Empty);//clear content of file if any

            while (reader.Read())
            {
                string x = reader.GetString(1);//audio_module_no
                string y = reader.GetString(2);//audio_channel_no
                int L = (x + y).Length;//combined length of above two
                int howManyZeros = (L != 15) ? 15-L : 0;//if x+y is 15 then do NOT insert any zeros. 
                string zeros = "";
                while(howManyZeros != 0)//add string of zeros to be inserted between x & y
                {
                    zeros += "0";
                    howManyZeros--;
                }
                y = zeros + y;
                    

                lbl_paths.Text += x + y + "\n";


                //Now write a text file
                //http://stackoverflow.com/questions/2695444/clearing-content-of-text-file-using-c-sharp
                //http://stackoverflow.com/questions/7569904/easiest-way-to-read-from-and-write-to-files
                //http://stackoverflow.com/questions/2837020/open-existing-file-append-a-single-line

               

                string path = "Path is: \\\\SE441903.maple.fg.rbc.com\\h$\\Calls\\" +
                    x +
                    "\\" + y.Substring(0, 3) +
                    "\\" + y.Substring(3, 2) +
                    "\\" + y.Substring(5, 2) +
                    "\\" + y.Substring(7, 2) +
                    "\n\n";

                File.AppendAllText(@"C:\Users\Zoya\Google Drive\RBC\Form_find_contacts\paths.txt", path+Environment.NewLine);
                lbl_paths.Text += path;
            }
            con.Close();

            //just testing what timePicker outputs
            lbl_paths.Text += dateFrom.Value.ToString();//it's 1/21/2017 1:55:56 PM


        }

        private void contactID_MouseClick(object sender, MouseEventArgs e)
        {
            kvp1.Text = "";
            kvp2.Text = "";
        }

        private void kvp1_MouseClick(object sender, MouseEventArgs e)
        {
            kvp2.Text = "";
            contactID.Text = "";
        }

        private void kvp2_MouseClick(object sender, MouseEventArgs e)
        {
            kvp1.Text = "";
            contactID.Text = "";
        }

        private void dateFromChkBx_CheckedChanged(object sender, EventArgs e)
        {
            dateFrom.Visible = dateFromChkBx.Checked ?true:false;
        }
        private void dateToChkBx_CheckedChanged(object sender, EventArgs e)
        {
            dateTo.Visible = dateToChkBx.Checked ?true:false;
        }

        private void timeRange_CheckedChanged(object sender, EventArgs e)
        {
            timeFrom.Visible = timeRange.Checked ? true : false;
            timeTo.Visible = timeRange.Checked ? true : false;
        }


        //copy files btn
        //http://stackoverflow.com/questions/21733756/best-way-to-split-string-by-last-occurrence-of-character

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> paths = new List<string>();
            paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file1.txt");
            paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file2.txt");
            paths.Add(@"C:\Users\Zoya\Google Drive\RBC\b\file3.txt");

            string destinationPath = @"C:\Users\Zoya\Google Drive\RBC\c\";
            //string filesToDelete = @"*_DONE.wav";   // Only delete WAV files ending by "_DONE" in their filenames
            //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);




            foreach (string file in paths)
            {
                string moveTo = destinationPath + file.Substring(file.LastIndexOf('\\') + 1);

                File.Copy(file, moveTo);
            }
        }
    }
}
