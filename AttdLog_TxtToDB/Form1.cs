using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace AttdLog_TxtToDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.Helper.constr = "Server=172.16.12.47;Database=Attendance;User Id=sa;Password=testomonials@123;";
            
            listView1.Items.Clear();
            listView1.Columns.Add("FileName", 400);
            listView1.Columns.Add("Punches", 400);

            txtSourceFolder.Text = "e:\\punchLog";
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSourceFolder.Text.Trim()))
                return;

            string path = txtSourceFolder.Text.Trim();

            if (Directory.Exists(path))
            {
                path = txtSourceFolder.Text.Trim();
                // This path is a directory
                this.Cursor = Cursors.WaitCursor;
                ProcessDirectory(path);
                this.Cursor = Cursors.Default;
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }

            this.Cursor = Cursors.Default;
        }

        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);

                string newFileName = Path.Combine(Path.GetDirectoryName(fileName), "Processed", Path.GetFileName(fileName));
                try
                {
                    File.Move(fileName, newFileName);
                }catch(Exception ex)
                {

                }
            }               

        }

        public void ProcessFile(string tpath)
        {
            if (File.Exists(tpath))
            {
                var lines = File.ReadLines(tpath);
                List<AttdLog> listAttd = new List<AttdLog>();

                

                foreach (var line in lines)
                {
                    //O;2018-12-08 05:54:06;20007766;192.168.6.100;0
                    string[] trow = line.Split(';');
                
                    if(trow.Length >= 4 )
                    {
                        AttdLog t = new AttdLog();
                        t.AddID = "upload";
                        t.AddDt = new DateTime(2018, 12, 12);
                        t.EmpUnqID = trow[2].ToString();
                        t.IOFLG = trow[0].ToString();
                        t.LunchFlg = (trow[4].ToString() == "0" ? false : true);
                        t.MachineIP = trow[3].ToString();
                        t.PunchDate = DateTime.ParseExact(trow[1].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        t.t1Date = t.PunchDate.Date;
                        t.TableName = Utils.Helper.GetAttdTableName(t.MachineIP, Utils.Helper.constr);
                        t.tYear = t.PunchDate.Year;
                        t.tYearMt = Convert.ToInt32(t.PunchDate.ToString("yyyyMM"));
                        
                        listAttd.Add(t);
                    }                    
                }

                foreach (AttdLog t in listAttd)
                {
                    string dberr = Utils.Helper.AttdLogStoreToDb(t);
                    string err = string.Empty;
                    if (!string.IsNullOrEmpty(dberr))
                    {
                        t.Error = dberr;
                        err = "Error while store to db : " + t.EmpUnqID + " : " + dberr + Environment.NewLine;
                    }

                    if(!string.IsNullOrEmpty(err))
                    {
                        string newFileName = Path.Combine(Path.GetDirectoryName(tpath), "Processed", "Errors.txt");

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(newFileName, true))
                        {
                            file.WriteLine(tpath + "->" + err);
                        }
                    }
                   
                }


			    ListViewItem row = new ListViewItem(tpath);
			    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, listAttd.Count.ToString()));
			    listView1.Items.Add(row);
                listView1.Refresh();
                Application.DoEvents();
            }
        }


    }
}
