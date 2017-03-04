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
using System.Text.RegularExpressions;//needed for RegEx


/*Important:
 * user=test; pass=Password.12345;
 * I tried logging in e "saimaple\svyx0srvoat" & "Password1" e SSMS, but failed, so probably this pwd is wrong!
 If try to conn to server on SaiMaple from a computer on Maple: err=cannot connect bcoz trying to connect froman untrusted domain. 
 * Possible Solution=add to trusted domains -or- change user's auth type to SQL auth (only sb e privilages to that that can do!).
 * =====
 * Last resort, create another user: rClick security node>>New>>Login>>any userName>>select SQL auth>>pwd at least 12char!>>
 * >>User Mapping Tab>>select DBs that this user can access>>
 * >>User Mapping Tab>>Highlight each DB one-by-one and then in bottom box select "db_reader" to allow SELECT or "db_owner" etc. In Server-RolesTab(server wide roles) chk serverAdmin also gives read/write permissions.. on all tbls in that DB>>OK
 * (To set permissions on individul tbls, rClk a tbl>>"search" user>>chk the boxes as needed)
 */
namespace FormFindCalls
{
    public partial class Form1 : Form
    {
        #region Global variables
        //int connTimeOut = 30;
        BackgroundWorker bw, bwCopy;
        int linesRead = 0;
        //double counter = 0;
        int missingFiles = 0;
        int found = 0;
        string txtForLogFile = "";
        string destinationPath = "";
        #endregion

                public Form1()
        {
            InitializeComponent();
            #region BackGround Worker
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += openConnection; //implicitly converts to += new DoWorkEventHandler(openConnection);//same as casting +=(DoWorkEventHandler)openConnection;
            bw.RunWorkerCompleted += bwCompleted;
            bw.ProgressChanged +=bw_ProgressChanged;
            
            //a 2nd bw for copying process
            bwCopy = new BackgroundWorker();
            bwCopy.WorkerReportsProgress = true;
            bwCopy.WorkerSupportsCancellation = true;
            bwCopy.DoWork+= new DoWorkEventHandler(startCopying);
            bwCopy.RunWorkerCompleted += copyCompleted;
            bwCopy.ProgressChanged +=bwCopy_ProgressChanged;

            #endregion
           
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
            //dropDownKvp.Text = "select KVP";
            foreach(KeyValuePair<string, string> x in kvp)//instead of Dictionary<> use KeyValuePair<>
            {
                //dropDownKvp.Items.Add(x.Value);//in prop pane set "Sorted=true" for alphabetical sorting
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

            //readConfigFile();
          
         
        }

                private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
                {
                    lblProgress.Text = e.ProgressPercentage.ToString() + " %";
                }

                private void bwCopy_ProgressChanged(object sender, ProgressChangedEventArgs e)
                {
                    lblProgress.Text = e.ProgressPercentage.ToString() + " %";
                }

                private void startCopying(object sender, DoWorkEventArgs e)
                {
                    //circularProgressBar1.Value = 0;
                    //circularProgressBar1.Update();
                    //circularProgressBar1.Visible = true;


                    //List<string> paths = new List<string>();
                    //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file1.txt");
                    //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\a\file2.txt");
                    //paths.Add(@"C:\Users\Zoya\Google Drive\RBC\b\file3.txt");

                    //string destinationPath = @"C:\Users\SVYX0SRVOAT\Desktop\Test\"; //this might as well be an empty str as it's being changed later based on dialogue box selection

                    //string filesToDelete = @"*_DONE.wav";   // Only delete WAV files ending by "_DONE" in their filenames
                    //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);

                    //open dialogue to overwrite "destinationPath"
                    //OpenFileDialog d = new OpenFileDialog();//it's to OPEN files
                    //SaveFileDialog d = new SaveFileDialog();
                    //d.Title = "Where to save files?";
                   
                    //destinationPath = "";
                    //linesRead = 0;
                    ////douFle counter = 0;
                    //missingFiles = 0;
                    //found = 0;
                    //FolderBrowserDialog d = new FolderBrowserDialog();
                    ////d.ShowDialog();
                    //if (d.ShowDialog() == DialogResult.OK)//this pops up dialogue as well as checks if OK was clicked aft that.
                    //{
                    //    destinationPath = d.SelectedPath + "\\";//need a backslash aft folder path
                    //}//if cancel or close is pressed then "destinationPath" is NOT overriden




                    destinationPath = e.Argument.ToString();

                    if(hasWriteAccessToFolder(destinationPath))//if need login and password to access files, give err msg & restart app
                    {
                        MessageBox.Show("You don't have permission to copy!", "Permission Denied");
                        Application.Restart();
                    }

                    foreach (string file in audioFilesList) //switch 'paths' e 'audioFilesList' for actual files
                    {
                        linesRead++;
                        if (File.Exists(file))//if user doesn't have permissions then returns "FALSE"//excludes lines like "\", "\\", "\\se", "SE123456\h$" etc that gave error "part of path not found"
                        {
                            //circularProgressBar1.Value += step;//accepts ONLY int. but then if >100 files we get fraction that rounds to 0--problem!!
                            //counter += step;
                            //circularProgressBar1.Value = (int)counter;
                            //circularProgressBar1.Update();
                            //circularProgressBar1.Text = (int)counter + "%";


                            string moveTo = destinationPath + file.Substring(file.LastIndexOf('\\') + 1); //\\SE104427.saimaple.fg.rbc.com\h$\Calls\871001\000\04\92\871001000049202.wav
                            try
                            {
                                File.Copy(file, moveTo, true);//true to overwrite existing files, else err
                                found++;
                            }
                            catch (Exception ex)
                            {
                                if (ex is System.IO.FileNotFoundException || ex is UnauthorizedAccessException)
                                {
                                missingFiles++;
                                continue;
                                }
                               
                            }
                        }
                        bwCopy.ReportProgress(linesRead * 100 / audioFilesList.Count);
                    }
                    e.Result = "Files copied: " + found + "  Files NOT copied: " + missingFiles;
                    
                }

                private bool hasWriteAccessToFolder(string folderPath)
                {//http://stackoverflow.com/questions/1410127/c-sharp-test-if-user-has-write-access-to-a-folder
                    try
                    {
                        // Attempt to get a list of security permissions from the folder. 
                        // This will raise an exception if the path is read only or do not have access to view the permissions. 
                        System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                        return true;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return false;
                    }
                }
               
                private void copyCompleted(object sender, RunWorkerCompletedEventArgs e)
                {
                    lbl_paths.Text = e.Result.ToString();
                    //circularProgressBar1.Value = 100;
                    //circularProgressBar1.Update();
                    linesRead = 0;
                    missingFiles = 0;
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
             
      
        #region helpful web sites

        /*single Query Approach.
         * //Extract time only: http://stackoverflow.com/questions/1026841/how-to-get-only-time-from-date-time-c-sharp
         * //compare TimeOfDay in if-else: http://stackoverflow.com/questions/10290187/how-to-compare-time-part-of-datetime
         * --search entire DB in MS-Sql Server: http://stackoverflow.com/questions/15757263/find-a-string-by-searching-all-tables-in-sql-server-management-studio-2008
         */
        #endregion

        #region declare global vars
        bool winAuth = true;
        string serverName, dbName;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string query;
        List<string> audioFilesList = new List<string>();
        int howManyFilesFound = 0;
        Form popUp;
        TextBox txtPass, txtUser, txtServer, txtDb, txtAuth;
        CheckBox chkAuth;
        string popUsrName, popPass;
        string tblToSearchStr;
        #endregion

        #region btn to connect DB and generate PATHs
        private void button1_Click(object sender, EventArgs e)
        {
            readConfigFile();

            winAuth = true;
            if (checkBox1.Checked)
            {
                winAuth = false;
                askPassword();
                //makeQueryString();
                //makeConnection();
            }
            else 
            {
                button1.Enabled = false;
                button2.Enabled = false;
                makeQueryString();
                pepareConnection();
            }

           
        }

        private void readConfigFile()
        {
            //AudioFilesCopyConfig.txtstring[] iniTxt = File.ReadAllLines(Path.GetFullPath(@"..\..\ini.txt"), Encoding.UTF8);//http://www.csharp-examples.net/read-text-file/
            if (!File.Exists("AudioFilesCopyConfig.txt")) createConfigFile();
            string[] iniTxt = File.ReadAllLines(Path.GetFullPath(@"AudioFilesCopyConfig.txt"), Encoding.UTF8);
            foreach (string line in iniTxt)
            {
                if (Regex.Match(line, "Server Name:").Success)
                {
                    var match = Regex.Match(line, ":");
                    serverName = line.Substring(match.Index + 1).Trim();//assign this to sqlCommand
                }
                if (Regex.Match(line, "Database Name:").Success)
                {
                    var match = Regex.Match(line, ":");
                    dbName = line.Substring(match.Index + 1).Trim();
                }
            }
        }

        private void pepareConnection()
        {
            //pictureBox1.Visible = true;
            //Thread.Sleep(3000);
            //pictureBox1.Visible = false;
            audioFilesList.Clear();
            howManyFilesFound = 0;
            lbl_paths.Text = "Paths will show here:";
            txtBxPaths.Clear();//txtBxPaths.Text = "";
            //circularProgressBar1.Visible = false;

            string conStr = makeConnectionString();
            bw.RunWorkerAsync(conStr); //openConnection(conStr); converted this method to an EventHandler so that can run in b thread//To pass multiple args, create a List: https://social.msdn.microsoft.com/Forums/vstudio/en-US/d7c0ba24-29b7-4fc9-86ef-92fb8cd5e17a/sending-multiple-arguments-to-background-worker?forum=csharpgeneral

            

        }

        private void openConnection(object s, DoWorkEventArgs e)
        {
            con = new SqlConnection(e.Argument.ToString());//= new SqlConnection(conStr); conStr passed an bw.Argument; & rcvd here as e.Argument;

            //SqlCommand cmd = new SqlCommand("select * from rbc_contacts", con);
            cmd = new SqlCommand(query, con);
            //cmd.CommandTimeout = connTimeOut;
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


                    #region Path's 1st segment. Server-to-Serial No.
                    string fullDomain;
                    switch (x)
                    {
                        case "871001":
                            fullDomain = @"\\SE104421\h$\Calls\"; //older: fullDomain = @"\\SE104421.saimaple.fg.rbc.com\h$\Calls\";
                            break;
                        case "871002":
                            fullDomain = @"\\SE104422\h$\Calls\";
                            break;
                        case "871003":
                            fullDomain = @"\\SE104426\h$\Calls\";
                            break;
                        case "871004":
                            fullDomain = @"\\SE104427\h$\Calls\";
                            break;
                        default:
                            continue;//passes control to next iterration of loop.
                    }
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
                    //File.AppendAllText(@"C:\Users\SVYX0SRVOAT\Desktop\Test\paths.txt", path+" "+DateTime.Now.ToString() + Environment.NewLine);
                    #endregion


                    //lbl_paths.Text += "\n\n"+ path;//display paths on GUI label
                    //txtBxPaths.Text += "\r\n" + path;//display paths on GUI text box//http://stackoverflow.com/questions/8536958/how-to-add-a-line-to-a-multiline-textbox
                    //lbl_paths.Text += reader.GetDateTime(7).ToString();

                }//while(reader.Read()) ends

                #region Write whole collection to log file
                txtForLogFile = "";
                foreach (string x in audioFilesList)
                {
                    txtForLogFile += (x + Environment.NewLine);
                }
                File.AppendAllText(@"C:\temp\paths.txt", DateTime.Now.ToString() + Environment.NewLine + txtForLogFile);
                //txtBxPaths.Text = txtForLogFile;//e bg worker this cause err, since bg thread tries to reach out to fg thread directly
                //File.AppendAllText(@"C:\temp\paths.txt", "=======================" + Environment.NewLine);
                //File.AppendAllText(@"C:\Users\SVYX0SRVOAT\Desktop\Test\paths.txt", "=======================" + Environment.NewLine);

                #endregion
            }
            catch (SqlException sqlEx)
            {
                //MessageBox.Show("Connection Timed Out. Please try again.");
                int errNum = sqlEx.Number;
                MessageBox.Show(sqlEx.Message, //msg & caption
                    (errNum==53?"Server Not Found!":"")+
                    (errNum == 18456? "Wrong UserName/Password!" : "") +
                    (errNum == 18452? "Instead of Windows Auth, use a userName & Password" : "")
                    );
            }
            finally
            {
                con.Close();
            }
            //cannot access foreground thread(GUI) from bg thread directly:- //lbl_paths.Text = howManyFilesFound + " files found. \n\n" + lbl_paths.Text;//total files found appended at beginning of all results        
            //Also cannot "return aString" and change VOID to STRING. Instead use e.Result prop.
            e.Result = new List<string>() { txtForLogFile, howManyFilesFound + " files found. \n\n" + lbl_paths.Text }; //howManyFilesFound + " files found. \n\n" + lbl_paths.Text;
        }

        private void bwCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             List<string> resultsList = (List<string>)e.Result;
             txtBxPaths.Text = resultsList.ElementAt(0);
             lbl_paths.Text =resultsList.ElementAt(1);

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private string makeConnectionString()
        {

            //SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder();
            ////conStrBuilder.DataSource = @"SE104499.saimaple.saifg.rbc.com\MSSQLSERVER"; //Lab2 server -- add fully qualified server name backslash instance name
            //conStrBuilder.DataSource = @"(local)\MSSQLSERVER";

            //conStrBuilder.InitialCatalog = "CentralContact";
            //conStrBuilder.IntegratedSecurity = true;//if false then put username & password
            ////conStrBuilder.UserID = "svyx0srvoat"; //OAT is Lab2
            ////conStrBuilder.Password = "Password1";
            //string conStr = conStrBuilder.ConnectionString;
            //==================================================
            //SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder();
            //conStrBuilder.DataSource = @"SE104499.saimaple.saifg.rbc.com\MSSQLSERVER"; //Lab2 server -- add fully qualified server name backslash instance name
            //conStrBuilder.DataSource = @"SE104499.saimaple.saifg.rbc.com\MSSQLSERVER";/IP: 10.241.205.88
            //conStrBuilder.InitialCatalog = "CentralDWH";
            //conStrBuilder.IntegratedSecurity = true;//if false then put username & password
            //conStrBuilder.UserID = "svyx0srvoat"; //OAT is Lab2
            //conStrBuilder.Password = "Password1";
            //string conStr = conStrBuilder.ConnectionString;
            //==================================================
            //string conStr = "Persist Security Info=False;Integrated Security=true;Initial Catalog=CentralDWH;server=(local)";

            //string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RBC_call_path;Integrated Security=True";
            //string conStr = @"Data Source=SE104499;Initial Catalog=CentralDWH;Integrated Security=False;User ID=test;Password=Password.12345;Connect Timeout=30;";//@"Data Source=SE104499;Initial Catalog=CentralDWH;Integrated Security=False;User ID=SAIMAPLR\SVYX0SRVOAT;Password=password1;Connect Timeout=30";
            //string conStr = @"Data Source=SE104499;Initial Catalog=CentralDWH;Integrated Security=True;Connect Timeout=30";
            //==================================================
            SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder();
            conStrBuilder.DataSource = serverName;//IP: 10.241.205.88
            conStrBuilder.InitialCatalog = dbName;
            conStrBuilder.IntegratedSecurity = winAuth;//if false then put username & password
            conStrBuilder.UserID = string.IsNullOrWhiteSpace(popUsrName) ? "test" : popUsrName; //cannot use .IsNullOrWhiteSpace(txtUser.Text) bcoz once popUp form is closed it becomes empty str ""
            conStrBuilder.Password = string.IsNullOrWhiteSpace(popPass)? "Password.12345":popPass;
            return conStrBuilder.ConnectionString;
            //==================================================        
        }

        private void makeQueryString()
        {
           

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
           
        }

        #endregion
        
        #region btn to copy the files
        //http://stackoverflow.com/questions/21733756/best-way-to-split-string-by-last-occurrence-of-character
        //https://msdn.microsoft.com/en-us/library/cc148994.aspx

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            
            destinationPath = "";
            linesRead = 0;
            //double counter = 0;
            missingFiles = 0;
            found = 0;
            
            FolderBrowserDialog d = new FolderBrowserDialog();
            //d.ShowDialog();
            if (d.ShowDialog() == DialogResult.OK)//this pops up dialogue as well as checks if OK was clicked aft that.
            {
                destinationPath = d.SelectedPath + "\\";//need a backslash aft folder path
            }//if cancel or close is pressed then "destinationPath" is NOT overriden

            if(!bwCopy.IsBusy)
            {
                bwCopy.RunWorkerAsync(destinationPath);
            }

           
        }
        #endregion

        #region btn for Testing 
        private void button3_Click(object sender, EventArgs e)
        {
            

            //if I don't catenate & hard code ini.txt then it ends up creating a new file!!! or not not sure
            //test.Text = Path.GetFullPath(@"..\..")+"\\ini.txt";//find full path of a file. Using (@"ini.txt") gives wrong path to a non-existing ini.txt in bin\Debug\ini.txt



            //=================================================================================================
            

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


        #region Create a config txt file
        private void button4_Click(object sender, EventArgs e)
        {

            createConfigFile();
            

            
        }
        void createConfigFile()
        {
                string configFileTxt =
                "Server Name: SE104499" + System.Environment.NewLine +
                "Database Name:	CentralDWH" + System.Environment.NewLine;

            File.WriteAllText("AudioFilesCopyConfig.txt", configFileTxt, Encoding.UTF8);
        }
        #endregion

        #region button click on pop-up window
        private void btnLogin_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            popUsrName = txtUser.Text;
            popPass = txtPass.Text;
            popUp.Close();//as soon as close() above 2 texts become "". Causing wrong reading in IsNullOrWhiteSpaces
            test.Text = popUsrName + popPass;
            makeQueryString();
            pepareConnection();
        //=================================================

        //#region Pop-up Login Window
        //popUp = new Form();
        //popUp.Size = new Size(500, 700);

        //Label userName, pass, server, db, auth;

        //userName = new Label();
        //userName.Text = "User Name: ";
        //userName.Location = new Point(10, 560);
        //txtUser = new TextBox();
        //txtUser.Location = new Point(120, 560);

        //pass = new Label();
        //pass.Text = "Password: ";
        //pass.Location = new Point(10, 600);
        //txtPass = new TextBox();
        //txtPass.Location = new Point(120, 600);
        //txtPass.UseSystemPasswordChar = true;//shows default black circles 
        ////txtPass.PasswordChar = '*';//Notice it's a char so single quotes.//Shows * instead of circles

        //server = new Label() { Text = "Server Name", Location = new Point(10, 20) };
        //txtServer = new TextBox() { Text = "SE104499", Location = new Point(120, 20), Size = new Size(200, 20) };

        //db = new Label() { Text = "Database Name: ", Location = new Point(10, 60)};
        //txtDb = new TextBox() { Text = "CentralDWH", Location = new Point(120, 60), Size = new Size(200, 20) };

        //auth = new Label() {Text="Use Windows Authorization? ", Location=new Point(10, 100), Size=new Size(200, 20) };
        //chkAuth = new CheckBox() { Location = new Point(215, 100) };


        //Button btnLogin = new Button();
        //btnLogin.Text = " OK ";
        //btnLogin.Location = new Point(300, 600);
        //btnLogin.Click += new System.EventHandler(btnLogin_Click);//notice can also do += btnLogin_Click;

        ////add all ctrls to popup window
        //popUp.Controls.Add(userName);
        //popUp.Controls.Add(pass);
        //popUp.Controls.Add(txtUser);
        //popUp.Controls.Add(txtPass);
        //popUp.Controls.Add(server);
        //popUp.Controls.Add(db);
        //popUp.Controls.Add(auth);
        //popUp.Controls.Add(txtServer);
        //popUp.Controls.Add(txtDb);
        //popUp.Controls.Add(chkAuth);
        ////popUp.Controls.Add();






        //popUp.Controls.Add(btnLogin);
        //popUp.Show();//display pop up window asking for userName and Password
        //#endregion
        }
        #endregion

        void askPassword()
        {
            popUp = new Form();
            popUp.Size = new Size(400, 400);
            Label userName, pass;
            userName = new Label();
            userName.Text = "User Name: ";
            userName.Location = new Point(10, 20);
            txtUser = new TextBox();
            txtUser.Location = new Point(120, 20);

            pass = new Label();
            pass.Text = "Password: ";
            pass.Location = new Point(10, 60);
            txtPass = new TextBox();
            txtPass.Location = new Point(120, 60);
            txtPass.UseSystemPasswordChar = true;//shows default black circles 
            txtPass.PasswordChar = '*';//Notice it's a char so single quotes.//Shows * instead of circles
            Button btnLogin = new Button();
            btnLogin.Text = " OK ";
            btnLogin.Location = new Point(10, 100);
            btnLogin.Click += new System.EventHandler(btnLogin_Click);
            popUp.Controls.Add(userName);
            popUp.Controls.Add(pass);
            popUp.Controls.Add(txtUser);
            popUp.Controls.Add(txtPass);
            popUp.Controls.Add(btnLogin);
            popUp.Show();
        }
    }
}
