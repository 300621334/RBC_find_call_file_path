using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace FormFindCalls
{
    public partial class Form1 : Form
    {
        #region GUI form initializer

        public Form1()
        {
            InitializeComponent();
            #region DateTime Box's format
            dateFrom.Format = DateTimePickerFormat.Custom;
            dateFrom.CustomFormat = "MMM/dd/yyyy --- HH:mm:ss";
            dateTo.Format = DateTimePickerFormat.Custom;
            dateTo.CustomFormat = "MMM/dd/yyyy --- HH:mm:ss";
            #endregion

            #region Add items to dropDownKvp(comboBox)
            Dictionary<string, string> kvp = new Dictionary<string, string>();
            kvp.Add("p1", "Account_Aspect");
            kvp.Add("p2", "SRF");
            kvp.Add("p3", "IXN_Purpose");
            kvp.Add("p4", "Language");
            kvp.Add("p5", "Auth");
            kvp.Add("p6", "CONNID");
            kvp.Add("p7", "RG");
            kvp.Add("p8", "CallPurpose");
            kvp.Add("p9", "Original_SkillSet");
            kvp.Add("p10", "CallPurpose_ID");
            kvp.Add("p11", "DM_Campaign_Name");
            kvp.Add("p12", "Province");
            kvp.Add("p13", "Original_Language");
            kvp.Add("p14", "Queue_Aspect");
            kvp.Add("p15", "Termination_Aspect");
            kvp.Add("p16", "CallId");
            kvp.Add("p17", "ChannelName");
            kvp.Add("p18", "DTMFdigits");
            kvp.Add("p19", "EndDate");
            kvp.Add("p20", "Custom_ThirdParty");
            kvp.Add("p21", "CallDirection");
            kvp.Add("p22", "NumberOfHolds");
            kvp.Add("p23", "TimeOnHold");
            kvp.Add("p43", "GSW_Phone");
            kvp.Add("p48", "IPG");
            dropDownKvp.Text = "select KVP";
            foreach (KeyValuePair<string, string> x in kvp)//instead of Dictionary<> use KeyValuePair<>
            {
                dropDownKvp.Items.Add(x.Value);//in prop pane set "Sorted=true" for alphabetical sorting
            }
            #endregion

            #region populate month's comboBox
            string[] tblToSearch = { "Sessions_month_1", "Sessions_month_2", "Sessions_month_3", "Sessions_month_4", "Sessions_month_5", "Sessions_month_6", "Sessions_month_7", "Sessions_month_8", "Sessions_month_9", "Sessions_month_10", "Sessions_month_11", "Sessions_month_12", };

            foreach (string x in tblToSearch)
            {
                tblToSearchDD.Items.Add(x);
            }
            tblToSearchDD.SelectedIndex = 0;
            #endregion
        }
        #endregion

        #region declare global vars
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string query;
        List<string> audioFilesList = new List<string>();
        int howManyFilesFound = 0;
        Form popUp;
        TextBox txtPass, txtUser;
        string popUsrName, popPass;
        string tblToSearchStr;
        #endregion

        #region btn to connect DB and generate PATHs
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            audioFilesList.Clear();
            howManyFilesFound = 0;
            lbl_paths.Text = "Paths will show here:";
            txtBxPaths.Clear();//txtBxPaths.Text = "";

            #region Query String
            tblToSearchStr = tblToSearchDD.SelectedItem.ToString();//if NO item selected from DD then err.

            query = "select * from " + tblToSearchStr + " where 1=1 "; //old: query = "select * from centralcontact.dbo.sessions where 1=1 and audio_module_no = 871001 ";
            query += " and local_start_time >= @startDate and local_end_time <= @endDate";
            query += (!string.IsNullOrWhiteSpace(contactID.Text)) ? " and contact_key = " + contactID.Text : ""; //old: query += (!string.IsNullOrWhiteSpace(contactID.Text))? " and contact_id = " +contactID.Text : "";//without single quotes err=invalid column name
            query += (!string.IsNullOrWhiteSpace(sessionID.Text)) ? " and sid_key = " + sessionID.Text : "";
            query += (!string.IsNullOrWhiteSpace(kvp1.Text)) ? " and p1_value = '" + kvp1.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp2.Text)) ? " and p2_value = '" + kvp2.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp3.Text)) ? " and p3_value = '" + kvp3.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp4.Text)) ? " and p4_value = '" + kvp4.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp5.Text)) ? " and p5_value = '" + kvp5.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp6.Text)) ? " and p6_value = '" + kvp6.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp7.Text)) ? " and p7_value = '" + kvp7.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp8.Text)) ? " and p8_value = '" + kvp8.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp9.Text)) ? " and p9_value = '" + kvp9.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp10.Text)) ? " and p10_value = '" + kvp10.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp11.Text)) ? " and p11_value = '" + kvp11.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp12.Text)) ? " and p12_value = '" + kvp12.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp13.Text)) ? " and p13_value = '" + kvp13.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp14.Text)) ? " and p14_value = '" + kvp14.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp15.Text)) ? " and p15_value = '" + kvp15.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp16.Text)) ? " and p16_value = '" + kvp16.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp17.Text)) ? " and p17_value = '" + kvp17.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp18.Text)) ? " and p18_value = '" + kvp18.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp19.Text)) ? " and p19_value = '" + kvp19.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp20.Text)) ? " and p20_value = '" + kvp20.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp21.Text)) ? " and p21_value = '" + kvp21.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp22.Text)) ? " and p22_value = '" + kvp22.Text + "'" : "";
            query += (!string.IsNullOrWhiteSpace(kvp23.Text)) ? " and p23_value = '" + kvp23.Text + "'" : "";
            //removing 43 & 48 bcoz DB Sessions_month_1/12 do not have p43_value etc & would cause error  
            //query += (!string.IsNullOrWhiteSpace(kvp43.Text)) ? " and p43_value = '" + kvp43.Text + "'" : "";
            //query += (!string.IsNullOrWhiteSpace(kvp48.Text)) ? " and p48_value = '" + kvp48.Text + "'" : "";
            #endregion


            #region connection String
            //SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder();
            ////conStrBuilder.DataSource = @"SE104499.saimaple.saifg.rbc.com\MSSQLSERVER"; //Lab2 server -- add fully qualified server name backslash instance name
            //conStrBuilder.DataSource = @"(local)\MSSQLSERVER";

            //conStrBuilder.InitialCatalog = "CentralContact";
            //conStrBuilder.IntegratedSecurity = true;//if false then put username & password
            ////conStrBuilder.UserID = "svyx0srvoat"; //OAT is Lab2
            ////conStrBuilder.Password = "Password1";
            //string conStr = conStrBuilder.ConnectionString;
            
            string conStr = "Persist Security Info=False;Integrated Security=true;Initial Catalog=CentralDWH;server=(local)";
            con = new SqlConnection(conStr);

            //SqlCommand cmd = new SqlCommand("select * from rbc_contacts", con);
            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 100;
            cmd.Parameters.AddWithValue("startDate", dateFrom.Value);
            cmd.Parameters.AddWithValue("endDate", dateTo.Value);
            #endregion

            #region open connection to db
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    #region Catenate audio_module_no and audio_ch_no and add zeros to make 15 digit, w is name of audio file and basis for nested folders too


                    howManyFilesFound++;
                    string x = reader.GetValue(12).ToString();//old: GetValue(2) //reader["audio_module_no"].ToString(); //reader.GetString(1);//audio_module_no//zero based index//getValue(1) same as Java's getObject().
                    string y = reader.GetValue(13).ToString();//old: GetValue(3)//reader["audio_ch_no"].ToString(); //reader.GetString(2);//audio_channel_no
                    int L = (x + y).Length;//combined length of above two
                    int howManyZeros = (L != 15) ? 15 - L : 0;//if x+y is 15 then do NOT insert any zeros. 
                    string zeros = "";
                    while (howManyZeros != 0)//add string of zeros to be inserted between x & y
                    {
                        zeros += "0";
                        howManyZeros--;
                    }
                    y = zeros + y;//insert zeros before channel#
                    #endregion

                    #region Server-to-SerialNumber When try to REMOTELY access files. Asks for password/usrname!!!
                    string fullDomain;

                    //Switch for Production servers:
                    switch (x)
                    {
                        //OCC
                        case "471002":
                            fullDomain = @"\\se441600\h$\Calls\"; //older: fullDomain = @"\\SE104421.saimaple.fg.rbc.com\h$\Calls\";
                            break;
                        case "471001":
                            fullDomain = @"\\se441601\h$\Calls\";
                            break;
                        case "471003":
                            fullDomain = @"\\se441602\h$\Calls\";
                            break;
                        case "471004":
                            fullDomain = @"\\se441603\h$\Calls\";
                            break;
                        case "471005":
                            fullDomain = @"\\se441604\h$\Calls\";
                            break;
                        case "471006":
                            fullDomain = @"\\se441605\h$\Calls\";
                            break;
                        case "471007":
                            fullDomain = @"\\se441606\h$\Calls\";
                            break;
                        case "471008":
                            fullDomain = @"\\se441607\h$\Calls\";
                            break;
                        case "471009":
                            fullDomain = @"\\se441608\h$\Calls\";
                            break;
                        //GCC
                        case "471010":
                            fullDomain = @"\\se441902\h$\Calls\";
                            break;
                        case "471011":
                            fullDomain = @"\\se441903\h$\Calls\";
                            break;
                        case "471012":
                            fullDomain = @"\\se441904\h$\Calls\";
                            break;
                        case "471013":
                            fullDomain = @"\\se441905\h$\Calls\"; //older: fullDomain = @"\\SE104421.saimaple.fg.rbc.com\h$\Calls\";
                            break;
                        case "471014":
                            fullDomain = @"\\se441906\h$\Calls\";
                            break;
                        case "471015":
                            fullDomain = @"\\se441907\h$\Calls\";
                            break;
                        case "471016":
                            fullDomain = @"\\se441908\h$\Calls\";
                            break;
                        case "471017":
                            fullDomain = @"\\se441909\h$\Calls\"; //older: fullDomain = @"\\SE104421.saimaple.fg.rbc.com\h$\Calls\";
                            break;
                        case "471018":
                            fullDomain = @"\\se441910\h$\Calls\";
                            break;


                        default:
                            continue;//passes control to next iterration of loop.
                    }


                    //Switch for Lab2
                    //switch (x)
                    //{
                    //    case "871001":
                    //        fullDomain = @"\\SE104421\h$\Calls\"; //older: fullDomain = @"\\SE104421.saimaple.fg.rbc.com\h$\Calls\";
                    //        break;
                    //    case "871002":
                    //        fullDomain = @"\\SE104422\h$\Calls\";
                    //        break;
                    //    case "871003":
                    //        fullDomain = @"\\SE104426\h$\Calls\";
                    //        break;
                    //    case "871004":
                    //        fullDomain = @"\\SE104427\h$\Calls\";
                    //        break;
                    //    default:
                    //        continue;//passes control to next iterration of loop.
                    //}
                    #endregion

                    #region path created here

                    string path = fullDomain + //e.g. \\SE104427.saimaple.fg.rbc.com\h$\Calls\871001\000\04\92\871001000049202.wav
                        x +
                        "\\" + y.Substring(0, 3) + //871001-000-049202
                        "\\" + y.Substring(3, 2) + //871001-000-04-9202
                        "\\" + y.Substring(5, 2) + //871001-000-04-92-02
                        "\\" + x + y + ".wav"; //871001000049202.wav
                    //+"\n\n";//this causes err sometimes when try to open file or copy: "Illegal characters in path". But if manually delete last few chars like .wav and reType then no-err bcoz that removes \n\n. Also have to hit back arrow few times before delete takes effect on last few chars again bcoz \n\n!!!
                    #endregion

                    #region add paths to collection
                    audioFilesList.Add(path);
                    //writing ea path after ea query was taking too long. So i removed it from here.
                    //File.AppendAllText(@"C:\temp\paths.txt", path + " " + DateTime.Now.ToString() + Environment.NewLine);

                    #endregion


                    //lbl_paths.Text += "\n\n"+ path;//display paths on GUI label
                    //txtBxPaths.Text += "\r\n" + path;//display paths on GUI text box//http://stackoverflow.com/questions/8536958/how-to-add-a-line-to-a-multiline-textbox
                    //lbl_paths.Text += reader.GetDateTime(7).ToString();

                }//while(reader.Read()) ends

                #region Write whole collection to log file
                string txtForLogFile = "";
                foreach (string x in audioFilesList)
                {
                    txtForLogFile += (x + Environment.NewLine);
                }
                File.AppendAllText(@"C:\temp\paths.txt", DateTime.Now.ToString() /*+ Environment.NewLine*/ + "\n" + txtForLogFile);
                txtBxPaths.Text = txtForLogFile;
                //File.AppendAllText(@"C:\temp\paths.txt", "=======================" + Environment.NewLine);
                //File.AppendAllText(@"C:\Users\SVYX0SRVOAT\Desktop\Test\paths.txt", "=======================" + Environment.NewLine);
                #endregion
            }
                catch(SqlException sqlexp)
            {
                MessageBox.Show("SQL server took longer than 100 sec. Please try again.");
                }
            finally
            {
                con.Close();
            }
            #endregion

            lbl_paths.Text = howManyFilesFound + " files found. \n\n" + lbl_paths.Text;//total files found appended at beginning of all results

            button1.Enabled = true;
            button2.Enabled = true;
        }
        #endregion

        #region btn to copy the files
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            string destinationPath = @"C:\temp\"; //this might as well be an empty str as it's being changed later based on dialogue box selection

            FolderBrowserDialog d = new FolderBrowserDialog();
            
            if (d.ShowDialog() == DialogResult.OK)//this pops up dialogue as well as checks if OK was clicked aft that.
            {
                destinationPath = d.SelectedPath + "\\";//need a backslash aft folder path
            }//if cancel or close is pressed then "destinationPath" is NOT overriden


            #region CircularProgressBar's progress defined
            double step = 100 / audioFilesList.Count;
            double counter = 0;
            int missingFiles = 0;
            #endregion


            foreach (string file in audioFilesList) //switch 'paths' e 'audioFilesList' for actual files
            {
                counter += step;

                string moveTo = destinationPath + file.Substring(file.LastIndexOf('\\') + 1); //\\SE104427.saimaple.fg.rbc.com\h$\Calls\871001\000\04\92\871001000049202.wav
                try
                {
                    File.Copy(file, moveTo, true);//true to overwrite existing files, else err
                }
                catch (System.IO.FileNotFoundException excep)
                {
                    missingFiles++;
                    continue;
                }

            }
            lbl_paths.Text = " Files NOT found: " + missingFiles;

            button1.Enabled = true;
            button2.Enabled = true;
        }
        #endregion

        #region btn for Testing

        private void button3_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region button click on pop-up window
        private void btnLogin_Click(object sender, EventArgs e)
        {
            popUsrName = txtUser.Text;
            popPass = txtPass.Text;
            popUp.Close();
            test.Text = popUsrName + popPass;

        }
        #endregion

    }
}
