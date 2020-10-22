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
using Attendance;
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
            //Utils.Helper.constr = "Server=172.16.12.47;Database=Attendance;User Id=sa;Password=testomonials@123;";

            listView1.Columns.Clear();

            listView1.Columns.Add("SrNo", "SrNo", 100);
            listView1.Columns.Add("EmpUnqID","EmpUnqID",100);
            listView1.Columns.Add("MachineIP","MachineIP", 80);
            listView1.Columns.Add("PunchDate","PunchDate", 100);
            listView1.Columns.Add("InOutFlg","InOutFlg", 50);
            listView1.Columns.Add("LunchFlg","LunchFlg", 50);
            listView1.Columns.Add("Remarks","Remarks", 100);


            txtBrowse.Text = "";


            Utils.DbCon dbcon = Utils.Helper.ReadConDb("DBCON");
            //Utils.DbCon empdbcon = Utils.Helper.ReadConDb("EMPDBCON");

            if (string.IsNullOrEmpty(dbcon.DataSource))
            {
                var b = new FrmConnection();
                b.typeofcon = "DBCON";
                b.ShowDialog();
                return;
            }
            else
            {
                Utils.Helper.constr = dbcon.ToString();
            }

            this.Text = "Upload Text File Attendance DataBase Upload (" + dbcon.DataSource + "->" + dbcon.DbName + ")";

            //cmbListMachine1.Properties.Items.Add("192.168.1.1");

            //load all machine ip in combo
            DataSet ds = new DataSet();
            string err = string.Empty;
            ds = Utils.Helper.GetData("Select MachineIP,MachineDesc,IOFLG From ReaderConfig where master = 0 and active = 1 Order By MachineNo", Utils.Helper.constr,out err);
            if(!string.IsNullOrEmpty(err))
            {
                MessageBox.Show("Could not connect Attendance Server" + Environment.NewLine + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string machineip = dr["MachineIP"].ToString() + ";" + dr["MachineDesc"].ToString() + ";" + dr["IOFLG"].ToString();
                    cmbListMachine1.Properties.Items.Add(machineip);
                   
                }
            }

            cmbListMachine1.SelectedIndex = 0;

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBrowse.Text.Trim()) )
            {
                MessageBox.Show("Please Select the File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }                

            if (cmbListMachine1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select the machine ip", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path = txtBrowse.Text.Trim();

            if (File.Exists(path))
            {
                path = txtBrowse.Text.Trim();
                // This path is a directory
                this.Cursor = Cursors.WaitCursor;
                cmbListMachine1.Enabled = false;
                btnBrowse.Enabled = false;
                ProcessFile(path);
                cmbListMachine1.Enabled = true;
                btnBrowse.Enabled = true;                
                this.Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Invalid File...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }

            this.Cursor = Cursors.Default;
        }

        //public void ProcessDirectory(string targetDirectory)
        //{
        //    // Process the list of files found in the directory.
        //    string[] fileEntries = Directory.GetFiles(targetDirectory);
        //    foreach (string fileName in fileEntries)
        //    {
        //        ProcessFile(fileName);

        //        string newFileName = Path.Combine(Path.GetDirectoryName(fileName), "Processed", Path.GetFileName(fileName));
        //        try
        //        {
        //            File.Move(fileName, newFileName);
        //        }catch(Exception ex)
        //        {

        //        }
        //    }               

        //}

        public void ProcessFile(string tpath)
        {
            //listView1.Columns.Add("SrNo", "SrNo", 100);
            //listView1.Columns.Add("EmpUnqID", "EmpUnqID", 100);
            //listView1.Columns.Add("MachineIP", "MachineIP", 80);
            //listView1.Columns.Add("PunchDate", "PunchDate", 100);
            //listView1.Columns.Add("InOutFlg", "InOutFlg", 50);
            //listView1.Columns.Add("LunchFlg", "LunchFlg", 50);
            //listView1.Columns.Add("Remarks", "Remarks", 100);


            foreach(ListViewItem tm in listView1.Items)
            {


                if (tm.SubItems.Count == 7)
                {



                    AttdLog t = new AttdLog();
                    t.AddID = "upload";
                    t.AddDt = DateTime.Now;

                    //ListViewItem.ListViewSubItem x = tm.SubItems;


                    t.EmpUnqID = tm.SubItems[1].Text.Trim();
                    t.IOFLG = tm.SubItems[4].Text.Trim();
                    t.LunchFlg = Convert.ToBoolean(tm.SubItems[5].Text.Trim());
                    t.MachineIP = tm.SubItems[2].Text.Trim();
                    t.PunchDate = DateTime.ParseExact(tm.SubItems[3].Text.Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    t.t1Date = t.PunchDate.Date;
                    t.TableName = Utils.Helper.GetAttdTableName(t.MachineIP, Utils.Helper.constr);
                    t.tYear = t.PunchDate.Year;
                    t.tYearMt = Convert.ToInt32(t.PunchDate.ToString("yyyyMM"));
                    //listAttd.Add(t);
                    string dberr = Utils.Helper.AttdLogStoreToDb(t);
                    string err = string.Empty;
                    if (!string.IsNullOrEmpty(dberr))
                    {
                        t.Error = dberr;
                        tm.SubItems[6].Text = dberr;
                    }
                    else
                    {
                        tm.SubItems[6].Text = "Uploaded";
                    }
                }

            }

            listView1.Refresh();
            Application.DoEvents();
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKeywordsFileDialog = new OpenFileDialog();
            openKeywordsFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openKeywordsFileDialog.Multiselect = false;
            openKeywordsFileDialog.ValidateNames = true;
            //openKeywordsFileDialog.CheckFileExists = true;
            openKeywordsFileDialog.DereferenceLinks = false;        //Will return .lnk in shortcuts.
            openKeywordsFileDialog.Filter = "Files|*.txt";
            openKeywordsFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(OpenKeywordsFileDialog_FileOk);
            var dialogResult = openKeywordsFileDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //first check if already exits if found return..
                string filenm = openKeywordsFileDialog.FileName.ToString();
                if (string.IsNullOrEmpty(filenm))
                    return;
                try
                {
                    txtBrowse.Text = openKeywordsFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    txtBrowse.Text = "";
                }
            }
            else
            {
                txtBrowse.Text = "";
            }
        }

        void OpenKeywordsFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog fileDialog = sender as OpenFileDialog;
            string selectedFile = fileDialog.FileName;
            if (string.IsNullOrEmpty(selectedFile) || selectedFile.Contains(".lnk"))
            {
                MessageBox.Show("Please select a valid File");
                e.Cancel = true;
            }
            return;
        }

        

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBrowse.Text.Trim()) )
            {
                MessageBox.Show("Please Select the File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }                

            if (cmbListMachine1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select the machine ip", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            //222222222	2019-11-23 11:47:07	1	0	15	0
            //222222222	2019-11-23 11:47:09	1	0	4	0
            //222222222	2019-11-23 11:47:11	1	0	4	0
            //222222222	2019-11-23 11:50:22	1	0	15	0
            //222222222	2019-11-23 11:50:25	1	0	15	0
            
            string ioflg = string.Empty;
            string lunchflg = string.Empty;
            string tpath = txtBrowse.Text.Trim();
            string err = string.Empty;

            string machineip = cmbListMachine1.Text.Trim().Split(';')[0].ToString().Trim();
            ioflg = cmbListMachine1.Text.Trim().Split(';')[2].ToString().Trim();
             //Utils.Helper.GetDescription("Select IOFLG From ReaderConfig where MachineIP = '" + machineip + "'", Utils.Helper.constr,out err);
            //ioflg = "I";

            if(!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            err = string.Empty;
            lunchflg = Utils.Helper.GetDescription("Select CanteenFLG From ReaderConfig where MachineIP = '" + machineip + "'", Utils.Helper.constr);
            //lunchflg = "FALSE";

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return;
            }


            if (File.Exists(tpath))
            {
                var lines = File.ReadLines(tpath);
                List<AttdLog> listAttd = new List<AttdLog>();
                int i = 1;
                listView1.Items.Clear();
                foreach (var line in lines)
                {
                    //O;2018-12-08 05:54:06;20007766;192.168.6.100;0

                    string[] trow = line.Split('\t');

                    if (trow.Length >= 4)
                    {
                        string EmpUnqID = trow[0].ToString();
                        string PunchDate = DateTime.ParseExact(trow[1].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        
                        ListViewItem row = new ListViewItem(i.ToString());
                        row.SubItems.Add(EmpUnqID);
                        row.SubItems.Add(machineip);
                        row.SubItems.Add(PunchDate);
                        row.SubItems.Add(ioflg);
                        row.SubItems.Add(lunchflg);
                        row.SubItems.Add("Pending to Upload");
                        listView1.Items.Add(row);
                        i += 1;
                       
                    }//if
                }//for
                listView1.Refresh();
                Application.DoEvents();
            }//if
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["FrmConnection"];

            if (t == null)
            {
                FrmConnection m = new FrmConnection();
               
                m.typeofcon = "DBCON";
                m.Show();
            }
        }


    }
}
