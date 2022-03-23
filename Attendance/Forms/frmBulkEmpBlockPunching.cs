using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using Attendance.Classes;
using System.Net.Http;
using Newtonsoft.Json;

namespace Attendance.Forms
{
    public partial class frmBulkEmpBlockPunching : DevExpress.XtraEditors.XtraForm
    {
        public string GRights = "XXXV";
        private int GFormID = 0;


        DataTable dt = new DataTable();

        public frmBulkEmpBlockPunching()
        {
            InitializeComponent();
            
        }

        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKeywordsFileDialog = new OpenFileDialog();
            openKeywordsFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openKeywordsFileDialog.Multiselect = false;
            openKeywordsFileDialog.ValidateNames = true;
            //openKeywordsFileDialog.CheckFileExists = true;
            openKeywordsFileDialog.DereferenceLinks = false;        //Will return .lnk in shortcuts.
            openKeywordsFileDialog.Filter = "Files|*.xls;*.xlsx;*.xlsb";
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

        private string DataValidate(DataRow tdr)
        {
            string err = string.Empty;
            string sql = "Select Compcode,WrkGrp,EmpUnqID,Active from MastEmp where EmpUnqID='" + tdr["EmpUnqID"].ToString() + "'";
            DataSet ds = Utils.Helper.GetData(sql,Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
           

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    //check emplyee active status
                    if (!Convert.ToBoolean(dr["Active"]))
                    {
                        err += "InActive Employee.";
                        return err;
                    }
                }
            }else{
                err += "Employee does not exist.";
            }


            if (string.IsNullOrEmpty(tdr["Reason"].ToString()))
            {
                err += "Reason is required.";
            }

            //get if already blocked or not
            bool isBlocked = Convert.ToBoolean(Utils.Helper.GetDescription("Select PunchingBlocked from MastEmp where EmpUnqID ='" + tdr["EmpUnqID"].ToString() + "'", Utils.Helper.constr));

            if (isBlocked)
            {
                err += "Employee Already Blocked.";
            }

            if (Globals.GetWrkGrpRights(1080, "", tdr["EmpUnqID"].ToString()) == false)
            {
                err += "Un-authorized";
            }
            
            return err;
        }


        private void btnImport_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            DataTable mailtb = new DataTable();
            DataColumn dc = new DataColumn();
            mailtb.Columns.Add("EmpUnqID", typeof(string));
            mailtb.Columns.Add("EmpName", typeof(string));
            mailtb.Columns.Add("WrkGrp", typeof(string));
            mailtb.Columns.Add("Department", typeof(string));
            mailtb.Columns.Add("Station", typeof(string));
            mailtb.Columns.Add("Action", typeof(string));
            mailtb.Columns.Add("Reason", typeof(string));
            mailtb.Columns.Add("ActionTime", typeof(string));
            mailtb.Columns.Add("ActionBy", typeof(string));
            mailtb.Columns.Add("Remarks", typeof(string));
            mailtb.AcceptChanges();

            DataTable dtMaterial = new DataTable();
            DataTable sortedDT = new DataTable();
            try
            {

                foreach (GridColumn column in grd_view1.VisibleColumns)
                {
                    if (column.FieldName != string.Empty)
                        dtMaterial.Columns.Add(column.FieldName, column.ColumnType);
                }


                for (int i = 0; i < grd_view1.DataRowCount; i++)
                {
                    DataRow row = dtMaterial.NewRow();

                    foreach (GridColumn column in grd_view1.VisibleColumns)
                    {
                        row[column.FieldName] = grd_view1.GetRowCellValue(i, column);
                    }
                    dtMaterial.Rows.Add(row);
                }

                DataView dv = dtMaterial.DefaultView;
                dv.Sort = "EmpUnqID asc";
                sortedDT = dv.ToTable();

                int srno = 0;

                using (SqlConnection con = new SqlConnection(Utils.Helper.constr))
                {
                    con.Open();
                    foreach (DataRow dr in sortedDT.Rows)
                    {
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        srno += 1;        
                        

                        string err = DataValidate(dr);

                        if (!string.IsNullOrEmpty(err))
                        {
                            
                            DataRow mr = mailtb.NewRow();
                            string WrkGrp = string.Empty;
                            string Dept = string.Empty;
                            string Stat = string.Empty;
                            string EmpName = string.Empty;
                                
                            bool t = GetEmpData(tEmpUnqID,out EmpName,out WrkGrp,out Dept,out Stat);
                            if (t)
                            {
                                mr["EmpUnqID"] = tEmpUnqID;
                                mr["EmpName"] = EmpName;
                                mr["WrkGrp"] = WrkGrp;
                                mr["Department"] = Dept;
                                mr["Station"] = Stat;
                                mr["Action"] = "Block";
                                mr["ActionBy"] = Utils.User.GUserID;
                                mr["Reason"] = dr["Reason"].ToString();
                                mr["ActionTime"] = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                                mr["Remarks"] = err;
                                mailtb.Rows.Add(mr);
                                mailtb.AcceptChanges();
                            }
                            
                            dr["Remarks"] = err;
                            continue; 
                        }

                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                try
                                {
                                    cn.Open();
                                    cmd.Connection = cn;


                                    int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));

                                    string sql = "select MachineIP,IOFLG from readerconfig where canteenflg = 0  and [master] = 0 and compcode = '01' and Active = 1 and MachineIP in ( " +
                                        " Select Distinct MachineIP from AttdLog where EmpUnqID = '" + tEmpUnqID + "' and Punchdate between DATEADD(day,-60,getdate()) and DateAdd(day,1,GETDATE()) ) " +
                                        " Union Select MachineIP,IOFLG From TripodReaderConfig  ";


                                    DataSet tds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                                    foreach (DataRow tdr in tds.Tables[0].Rows)
                                    {

                                        sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,Remarks) Values ('" + tmaxid + "','" +
                                            tEmpUnqID + "','" + tdr["MachineIP"].ToString() + "','" + tdr["IOFLG"].ToString() + "','BLOCK',GetDate(),'" + Utils.User.GUserID + "',0,GetDate(),'" + dr["Reason"].ToString() + "')";


                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();

                                    }

                                    cmd.CommandText = "Update MastEmp Set PunchingBlocked = 1 where CompCode = '01' and EmpUnqID = '" + tEmpUnqID + "'";
                                    cmd.ExecuteNonQuery();

                                    cmd.CommandText = "Update EmpBioData Set Blocked = 1 where EmpUnqID = '" + tEmpUnqID + "'";
                                    cmd.ExecuteNonQuery();

                                    
                                    dr["Remarks"] = "Employee has been blocked";

                                    DataRow mr = mailtb.NewRow();
                                    string WrkGrp = string.Empty;
                                    string Dept = string.Empty;
                                    string Stat = string.Empty;
                                    string EmpName = string.Empty;

                                    bool t = GetEmpData(tEmpUnqID, out EmpName, out WrkGrp, out Dept, out Stat);
                                    if (t)
                                    {
                                        mr["EmpUnqID"] = tEmpUnqID;
                                        mr["EmpName"] = EmpName;
                                        mr["WrkGrp"] = WrkGrp;
                                        mr["Department"] = Dept;
                                        mr["Station"] = Stat;
                                        mr["Action"] = "Block";
                                        mr["ActionBy"] = Utils.User.GUserID;
                                        mr["Reason"] = dr["Reason"].ToString();
                                        mr["ActionTime"] = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                                        mr["Remarks"] = "Employee has been blocked";
                                        mailtb.Rows.Add(mr);
                                        mailtb.AcceptChanges();
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                    dr["Remarks"] = "Error" + ex.Message;
                                }
                            }
                        }
                        
                    }//using foreach

                    

                    con.Close();
                }//using connection

                //send-mail
                if (mailtb.Rows.Count > 0)
                {
                    sendmail(mailtb);
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("file uploaded Successfully, please check the remarks for indivisual record status...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            DataSet ds = new DataSet();
            ds.Tables.Add(sortedDT);
            grd_view.DataSource = ds;
            grd_view.DataMember = ds.Tables[0].TableName;
            grd_view.Refresh();

            Cursor.Current = Cursors.Default;
        }

        private void sendmail(DataTable tb)
        {
            if (tb.Rows.Count <= 0)
            {
                return;
            }
            
            
            
            string tsubject = string.Empty;
            string tbody = string.Empty;
            string to = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='EMPBLOCKNOTIFICATION'", Utils.Helper.constr);
            string cc = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='EMPBLOCKNOTIFICATION_CC'", Utils.Helper.constr);
            string bcc = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='EMPBLOCKNOTIFICATION_BCC'", Utils.Helper.constr);


            
            tsubject = "Notification : Bulk Employee Card Blocked";
            
            
            

            string thead = "<html> " +
                    "<head>" +
                    "<style>" +
                    " table { " +
                        " font-family: arial, sans-serif; " +
                        " border-collapse: collapse; " +
                        " width: 100%; " +
                    "} " +

                    " td, th { " +
                    "    border: 1px solid #dddddd; " +
                    "    text-align: left; " +
                    "    padding: 8px; " +
                    "} " +

                    " tr:nth-child(even) { " +
                    "    background-color: #dddddd;" +
                    "}" +
                    "</style>" +
                    "</head>" +
                    "<body>";
            
            tbody = "Sir, <br/><p>" + "Subjected Action Performed as per below details:" + "</p>  " +
                " <p>Action Time : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "</p> " +
                " <p>Action Performed By : " + Utils.User.GUserName + "</p> <br/>" + 
                "<table>" +
                "<tr><td>EmpCode</td><td>EmpName</td><td>WrkGrp</td><td>Department</td><td>Station</td><td>Reason</td><td>Remark</td></tr>";

            foreach (DataRow dr in tb.Rows)
            {
                tbody += "<tr>" +
                    "<td>" + dr["EmpUnqID"].ToString() + "</td>" +
                    "<td>" + dr["EmpName"].ToString() + "</td>" +
                    "<td>" + dr["WrkGrp"].ToString() + "</td>" +
                    "<td>" + dr["Department"].ToString() + "</td>" +
                    "<td>" + dr["Station"].ToString() + "</td>" +
                    "<td>" + dr["Reason"].ToString() + "</td>" +                    
                    "<td>" + dr["Remarks"].ToString() + "</td></tr>";
            }
                
            tbody += "</table><br/><br/> " +
                 "*This is Auto-generated notification, do not reply on this e-mail id. </body></html>";


            string err = EmailHelper.Email(to, cc, bcc, thead + tbody, tsubject, Globals.G_DefaultMailID,
                        Globals.G_DefaultMailID, "", "");

            

        }

        private bool GetEmpData(string EmpUnqID,out string EmpName,out string WrkGrp,out string Dept,out string Stat ){
            string err3 = string.Empty;
            WrkGrp = string.Empty;
            Dept = string.Empty;
            Stat = string.Empty;
            EmpName = string.Empty;
            bool t = false;

            DataSet ds = Utils.Helper.GetData("Select EmpUnqID,EmpName,WrkGrp,DeptDesc,StatDesc from V_EmpMast where EmpUnqID='" + EmpUnqID + "'",Utils.Helper.constr,out err3);
        
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    WrkGrp = dr["WrkGrp"].ToString();
                    Dept = dr["DeptDesc"].ToString();
                    Stat = dr["StatDesc"].ToString();
                    EmpName = dr["EmpName"].ToString();
                    t = true;
                }
            }

            return t;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBrowse.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Select Excel File First...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnBrowse.Enabled = false;

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }

            

            Cursor.Current = Cursors.WaitCursor;
            grd_view.DataSource = null;
            string filePath = txtBrowse.Text.ToString();

            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath + ";extended properties=" + "\"excel 8.0;hdr=yes;IMEX=1;\"";

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            List<SheetName> sheets = ExcelHelper.GetSheetNames(oledbconn);
            string sheetname = "[" + sheets[0].sheetName.Replace("'", "") + "]";

            try
            {
                oledbconn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            


            try
            {
                string myexceldataquery = "select EmpUnqID,Reason, '' as Remarks from " + sheetname;
                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt.Clear();
                oledbda.Fill(dt);
                
                dt.AcceptChanges();
                foreach (DataRow row in dt.Rows)
                {
                    if (string.IsNullOrEmpty(row["EmpUnqID"].ToString().Trim()))
                        row.Delete();
                }
                dt.AcceptChanges();

                oledbconn.Close();
            }
            catch (Exception ex)
            {
                oledbconn.Close();
                MessageBox.Show("Please Check upload template.." + Environment.NewLine + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnImport.Enabled = false;
                oledbconn.Close();
                return;
            }
            

            DataView dv = dt.DefaultView;
            dv.Sort = "EmpUnqID asc";
            DataTable sortedDT = dv.ToTable();




            grd_view.DataSource = sortedDT;

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            grd_view.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            grd_view.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            grd_view.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            grd_view.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            grd_view.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            grd_view.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmBulkEmpBlockPunching_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            
            //string s = Utils.Helper.GetDescription("Select SanDayLimit from MastBCFlg", Utils.Helper.constr);
            //if(string.IsNullOrEmpty(s)){
            //    rSanDayLimit = 0;
            //    MessageBox.Show("Please Contact to Admin : for some confuguraiton required.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}else{
            //    rSanDayLimit = Convert.ToInt32(s);

            //    if(rSanDayLimit == 0)
            //    {
            //        MessageBox.Show("Please Contact to Admin : for some confuguraiton required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}

            GFormID = Convert.ToInt32(Utils.Helper.GetDescription("Select FormId from MastFrm Where FormName ='" + this.Name + "'",Utils.Helper.constr));

                
            grd_view.DataSource = null;
            btnImport.Enabled = false;
        }
    }
}