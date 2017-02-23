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
        #region GUI form initializer

        public Form1()
        {
            InitializeComponent();
            #region DateTime Box's format

           
            dateFrom.Format = DateTimePickerFormat.Custom;
            dateFrom.CustomFormat = "MMM/dd/yyyy --- HH:mm:ss";
            dateTo.Format = DateTimePickerFormat.Custom;
            dateTo.CustomFormat = "MMM/dd/yyyy --- HH:mm:ss";

            //timeFrom.MaxDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //timeTo.MaxDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //custom format //http://stackoverflow.com/questions/13711358/datetime-picker-c-sharp-format
            #endregion

            #region Add items to dropDownKvp(comboBox)
            //in C#, a Dictionary contains key-value
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
            //kvp.Add("", "");
            dropDownKvp.Text = "select KVP";
            foreach(KeyValuePair<string, string> x in kvp)//instead of Dictionary<> use KeyValuePair<>
            {
                dropDownKvp.Items.Add(x.Value);//in prop pane set "Sorted=true" for alphabetical sorting
            }
            #endregion

            #region populate month's comboBox
            string[] tblToSearch = { "Sessions_month_1", "Sessions_month_2", "Sessions_month_3", "Sessions_month_4", "Sessions_month_5", "Sessions_month_6", "Sessions_month_7", "Sessions_month_8", "Sessions_month_9", "Sessions_month_10", "Sessions_month_11", "Sessions_month_12", };
            //tblToSearchDD.Text = "select Table";//sets this str when no item is selected, BUT selectedIndex remains negative
            foreach(string x in tblToSearch)
            {
                tblToSearchDD.Items.Add(x);
            }
            tblToSearchDD.SelectedIndex = 0;
            #endregion
        }
        #endregion

        #region helpful web sites

        /*single Query Approach.
         * //Extract time only: http://stackoverflow.com/questions/1026841/how-to-get-only-time-from-date-time-c-sharp
         * //compare TimeOfDay in if-else: http://stackoverflow.com/questions/10290187/how-to-compare-time-part-of-datetime
         * --search entire DB in MS-Sql Server: http://stackoverflow.com/questions/15757263/find-a-string-by-searching-all-tables-in-sql-server-management-studio-2008
         */
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
            
            //pictureBox1.Visible = true;
            //Thread.Sleep(3000);
            //pictureBox1.Visible = false;
            audioFilesList.Clear();
            howManyFilesFound = 0;
            lbl_paths.Text = "Paths will show here:";
            txtBxPaths.Clear();//txtBxPaths.Text = "";
            //circularProgressBar1.Visible = false;

            #region Query

            tblToSearchStr = tblToSearchDD.SelectedItem.ToString();//if NO item selected from DD then err.
            

            query = "select * from " + tblToSearchStr + " where 1=1 "; //old: query = "select * from centralcontact.dbo.sessions where 1=1 and audio_module_no = 871001 ";

            query += (!string.IsNullOrWhiteSpace(contactID.Text)) ? " and contact_key = " + contactID.Text : ""; //old: query += (!string.IsNullOrWhiteSpace(contactID.Text))? " and contact_id = " +contactID.Text : "";//without single quotes err=invalid column name
            query += (!string.IsNullOrWhiteSpace(sessionID.Text)) ? " and sid_key = " + sessionID.Text : ""; 
            //query += (!string.IsNullOrWhiteSpace(kvp1.Text)) ? " and pcd1_value = '" + kvp1.Text+"'" : "";
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
            query += " and local_start_time >= @startDate and local_end_time <= @endDate";

            //query += " and DatePart(yyyy, start_time) >= @yearStart and DatePart(yyyy, end_time) <= @yearEnd";//http://www.w3schools.com/sql/func_datepart.asp
            //query += " and DatePart(mm, start_time) >= @monthStart and DatePart(mm, end_time) <= @monthEnd";
            //query += " and DatePart(dd, start_time) >= @dayStart and DatePart(dd, end_time) <= @dayEnd";
            //query += " and DatePart(hh, start_time) >= @hourStart and DatePart(hh, end_time) <= @hourEnd";
            //query += " and DatePart(mi, start_time) >= @minStart and DatePart(mi, end_time) <= @minEnd";
            //query += " and DatePart(ss, start_time) >= @secStart and DatePart(ss, end_time) <= @secEnd";


            //Format(cast('2016-03-03 23:59:59' as datetime),'dd-MMM-yyyy HH:mm:ss','en-us')//http://stackoverflow.com/questions/19563261/convert-a-12-hour-format-to-24-hour-format-in-sql-2000
            //https://msdn.microsoft.com/en-CA/library/hh213505.aspx
            //Password1

            //if (timeRange.Checked)
            //{
            //    query += " and cast(substring(convert(varchar, start_time, 113),13,8) as datetime) >= @startTime and cast(substring(convert(varchar, end_time, 113),13,8) as datetime) <= @endTime";//'time' instead of 'datetime' gave err= data type time not compatible with datetime
            //    //    query += " and convert(time, start_time) >= @startTime and convert(time, end_time) <= @endTime";
            //}
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

            //string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RBC_call_path;Integrated Security=True";



            con = new SqlConnection(conStr);

            //SqlCommand cmd = new SqlCommand("select * from rbc_contacts", con);
            cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("startDate", dateFrom.Value);
            cmd.Parameters.AddWithValue("endDate", dateTo.Value);


            //if (timeRange.Checked)
            //{
            //    //timePicker has millisec when app launches. But as soon as manually change it, ms r gone. So format .ffffff is needed BEFORE manually changing time, but AFT changing it it gives err "str not recognized as valid format"!!!
            //    //            DateTime modifiedTimeFrom = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0,8), "yyyy-MM-dd hh:mm:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture);//last arg to avoid yyyy/mm/dd format that might cause confusion //


            //DateTime modifiedTimeFrom = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0,8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//hh: instead of HH: caused incorrect query results. bcoz hh: has AM/PM and substr dropped that AM/PM part//last arg to avoid yyyy/mm/dd format that might cause confusion //
            //    //Console.WriteLine(modifiedTimeFrom.ToString());
            //DateTime modifiedTimeTo = DateTime.ParseExact("1900-01-01 " + timeTo.Value.TimeOfDay.ToString().Substring(0, 8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            //cmd.Parameters.AddWithValue("startTime", modifiedTimeFrom);
            //cmd.Parameters.AddWithValue("endTime", modifiedTimeTo);
            //}
            #endregion

          
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();

                //lbl_paths.Text += "\n";

                //File.WriteAllText(@"C:\Users\Zoya\Google Drive\RBC\Form_find_contacts\paths.txt", string.Empty);//clear content of file if any

                while (reader.Read())
                {
                    #region Catenate audio_module_no and audio_ch_no and add zeros to make 15 digit, w is name of audio file and basis for nested folders too

                  
                    howManyFilesFound++;
                    //To getValue(byColName) //http://stackoverflow.com/questions/8655965/how-to-get-data-by-sqldatareader-getvalue-by-column-name
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

                    //lbl_paths.Text += x + y + "\n";
                    #endregion

                    #region REMOTELY access files. Asks for password/usrname!!!
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

                    #region access files LOCALLY... instead of remotely to avoid entering username/Password!!!
                    //switch (x)
                    //{
                    //    case "871001":
                    //        fullDomain = "H:\\Calls\\";
                    //        break;
                    //    case "871002":
                    //        fullDomain = "H:\\Calls\\";
                    //        break;
                    //    case "871003":
                    //        fullDomain = "H:\\Calls\\";
                    //        break;
                    //    case "871004":
                    //        fullDomain = "H:\\Calls\\";
                    //        break;
                    //    default:
                    //        continue;
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

                    #region log file & display list
                    //Now write a text file
                    //http://stackoverflow.com/questions/2695444/clearing-content-of-text-file-using-c-sharp
                    //http://stackoverflow.com/questions/7569904/easiest-way-to-read-from-and-write-to-files
                    //http://stackoverflow.com/questions/2837020/open-existing-file-append-a-single-line

                    audioFilesList.Add(path);
                    File.AppendAllText(@"C:\Users\SVYX0SRVOAT\Desktop\Test\paths.txt", path+" "+DateTime.Now.ToString() + Environment.NewLine);
                    #endregion


                    //lbl_paths.Text += "\n\n"+ path;//display paths on GUI label
                    txtBxPaths.Text += "\r\n" + path;//display paths on GUI text box//http://stackoverflow.com/questions/8536958/how-to-add-a-line-to-a-multiline-textbox
                    //lbl_paths.Text += reader.GetDateTime(7).ToString();

                }//while(reader.Read()) ends
                File.AppendAllText(@"C:\Users\SVYX0SRVOAT\Desktop\Test\paths.txt", "=======================" + Environment.NewLine);
            }
            finally
            {
                con.Close();
            }
            lbl_paths.Text = howManyFilesFound + " files found. \n\n" + lbl_paths.Text;//total files found appended at beginning of all results

            button1.Enabled = true;
            button2.Enabled = true;
        }

        #endregion
        
        #region btn to copy the files
        //http://stackoverflow.com/questions/21733756/best-way-to-split-string-by-last-occurrence-of-character
        //https://msdn.microsoft.com/en-us/library/cc148994.aspx

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            //circularProgressBar1.Value = 0;
            //circularProgressBar1.Update();
            //circularProgressBar1.Visible = true;


            //List<string> paths = new List<string>();
            //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file1.txt");
            //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file2.txt");
            //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\b\file3.txt");

            string destinationPath = @"C:\Users\SVYX0SRVOAT\Desktop\Test\"; //this might as well be an empty str as it's being changed later based on dialogue box selection

            //string filesToDelete = @"*_DONE.wav";   // Only delete WAV files ending by "_DONE" in their filenames
            //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);

            //open dialogue to overwrite "destinationPath"
            //OpenFileDialog d = new OpenFileDialog();//it's to OPEN files
            //SaveFileDialog d = new SaveFileDialog();
            //d.Title = "Where to save files?";

            FolderBrowserDialog d = new FolderBrowserDialog();
            //d.ShowDialog();
            if (d.ShowDialog() == DialogResult.OK)//this pops up dialogue as well as checks if OK was clicked aft that.
            {
                destinationPath = d.SelectedPath + "\\";//need a backslash aft folder path
            }//if cancel or close is pressed then "destinationPath" is NOT overriden


            #region CircularProgressBar's progress defined
            double step = 100/audioFilesList.Count;
            double counter = 0;
            int missingFiles = 0;
            #endregion


            foreach (string file in audioFilesList) //switch 'paths' e 'audioFilesList' for actual files
            {
                //circularProgressBar1.Value += step;//accepts ONLY int. but then if >100 files we get fraction that rounds to 0--problem!!
                counter += step;
                //circularProgressBar1.Value = (int)counter;
                //circularProgressBar1.Update();
                //circularProgressBar1.Text = (int)counter + "%";


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
            //circularProgressBar1.Value = 100;
            //circularProgressBar1.Update();

            button1.Enabled = true;
            button2.Enabled = true;
        }
        #endregion

        #region btn for Testing 

        private void button3_Click(object sender, EventArgs e)
        {




            //=================================================================================================
            #region Pop-up Login Window


            //popUp = new Form();
            //Label userName = new Label();
            //userName.Text = "User Name: ";
            //userName.Location = new Point(10, 20);
            //txtUser = new TextBox();
            //txtUser.Location = new Point(120, 20);


            //Label pass = new Label();
            //pass.Text = "Password: ";
            //pass.Location = new Point(10, 60);
            //txtPass = new TextBox();
            //txtPass.Location = new Point(120, 60);
            //txtPass.UseSystemPasswordChar = true;//shows default black circles 
            ////txtPass.PasswordChar = '*';//Notice it's a char so single quotes.//Shows * instead of circles


            //Button btnLogin = new Button();
            //btnLogin.Text = "Login";
            //btnLogin.Location = new Point(30, 90);
            //btnLogin.Click += new System.EventHandler(btnLogin_Click);//notice +=

            //popUp.Controls.Add(userName);
            //popUp.Controls.Add(pass);
            //popUp.Controls.Add(txtUser);
            //popUp.Controls.Add(txtPass);



            //popUp.Controls.Add(btnLogin);
            //popUp.Show();//display pop up window asking for userName and Password
            #endregion

            #region  Experimenting DateTime


            //test.Text = DateTime.Now.ToString();

            //=======================================================
            //DateTime myDate = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0, 8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//last arg to avoid yyyy/mm/dd format that might cause confusion //
            //=======================================================
            //test.Text = timeFrom.Value.TimeOfDay.ToString();

            //            DateTime myDate = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString(), "yyyy-MM-dd HH:mm:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture);//last arg to avoid yyyy/mm/dd format that might cause confusion //
            //=======================================================
            //DateTime myDate = DateTime.ParseExact("1900-01-01 " + timeFrom.Value.TimeOfDay.ToString().Substring(0,8), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//last arg to avoid yyyy/mm/dd format that might cause confusion //
            //test.Text = myDate.ToString();//test.Text = myDate.ToString("HH:mm:ss");//24he format display
            //=======================================================
            //DateTime myDate = DateTime.ParseExact("2009-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture)//http://stackoverflow.com/questions/919244/converting-a-string-to-datetime
            //string text = dateTime.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);//http://stackoverflow.com/questions/18874102/net-datetime-tostringmm-dd-yyyy-hhmmss-fff-resulted-in-something-like
            //=======================================================
            #endregion

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
