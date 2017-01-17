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
using System.Threading;

namespace FormFindCalls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*single Query Approach.
         */

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string query;

        private void button1_Click(object sender, EventArgs e)
        {
            lbl_paths.Text = "Paths ro call-files are:";

            query = "select * from rbc_contacts where 1=1 ";
            
            query += (!string.IsNullOrWhiteSpace(contactID.Text))? " and contact_ID = '" + contactID.Text+"'" : "";//without single quotes err=invalid column name
            query += (!string.IsNullOrWhiteSpace(kvp1.Text)) ? " and pcd1_value = '" + kvp1.Text+"'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp2.Text)) ? " and pcd2_value = '" + kvp2.Text+"'" : "";
            query += " and convert(date, start_time) >= @startDate and convert(date, end_time) <= @endDate";
            

            if (timeRange.Checked)
            {
                query += " and cast(substring(convert(varchar, start_time, 113),13,8) as datetime) >= @startTime and cast(substring(convert(varchar, end_time, 113),13,8) as datetime) <= @endTime";//'time' instead of 'datetime' gave err= data type time not compatible with datetime
            }



            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RBC_call_path;Integrated Security=True";
            con = new SqlConnection(conStr);
            //SqlCommand cmd = new SqlCommand("select * from rbc_contacts", con);
            cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("startDate", dateFrom.Value.Date);
            cmd.Parameters.AddWithValue("endDate", dateTo.Value.Date);

            if (timeRange.Checked)
            {
            DateTime modifiedTimeFrom = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0,8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//hh: instead of HH: caused incorrect query results. bcoz hh: has AM/PM and substr dropped that AM/PM part//last arg to avoid yyyy/mm/dd format that might cause confusion //
            DateTime modifiedTimeTo = DateTime.ParseExact("1900-01-01 " + timeTo.Value.TimeOfDay.ToString().Substring(0, 8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            cmd.Parameters.AddWithValue("startTime", modifiedTimeFrom);
            cmd.Parameters.AddWithValue("endTime", modifiedTimeTo);
            }


            //open the connection to DB
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

        private void button3_Click(object sender, EventArgs e)
        {

            DateTime myDate = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0, 8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//last arg to avoid yyyy/mm/dd format that might cause confusion //
            test.Text = myDate.ToString();
        }
    }
}
