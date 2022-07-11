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

namespace Attendance.Forms
{
    public partial class frmMastEmpBulkEmployeeImport : DevExpress.XtraEditors.XtraForm
    {
        public string GRights = "XXXV";

        DataTable dt = new DataTable();

        public frmMastEmpBulkEmployeeImport()
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

        private string DataValidate(string tEmpUnqID,string tCostCode,DateTime tValidFrom)
        {
            string err = string.Empty;
            clsEmp t = new clsEmp();
            
            t.EmpUnqID = tEmpUnqID;
            if (!t.GetEmpDetails(t.EmpUnqID))
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }

            t.CostCode = tCostCode;
            t.GetCostDesc(tCostCode);
            if (string.IsNullOrEmpty(t.CostDesc))
            {
                err = err + "Invalid CostCode..." + Environment.NewLine;
            }

            string tsql = "Select Active from MastCostCode where CostCode = '" + t.CostCode + "'";
            string ifcostact = Utils.Helper.GetDescription(tsql, Utils.Helper.constr, out err);
            if (string.IsNullOrEmpty(ifcostact))
            {
                ifcostact = "false";
            }

            if (!Convert.ToBoolean(ifcostact))
            {
                err = err + "CostCode not active..." + Environment.NewLine;
                return err;
            }

            


            return err;
        }


        private void btnImport_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

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

                using (SqlConnection con = new SqlConnection(Utils.Helper.constr))
                {   
                    con.Open();
                    foreach (DataRow tdr in sortedDT.Rows)
                    {
                        
                        
                        string tEmpUnqID = tdr["EmpUnqID"].ToString();
                        if (string.IsNullOrEmpty(tEmpUnqID))
                        {
                            tdr["Remarks"] = "EmpUnqID is blank...";
                            continue;
                        }

                        string err = string.Empty;
                        string t = Utils.Helper.GetDescription("select Count(*) from MastEmp Where EmpUnqID='" + tEmpUnqID + "'", Utils.Helper.constr, out err);
                        if (!string.IsNullOrEmpty(err))
                        {
                            tdr["Remarks"] = err;
                            continue;
                        }

                        if (Convert.ToInt32(t) > 0)
                        {
                            tdr["Remarks"] = "Employee Already Exist...";
                            continue;
                        }

                        string tCompCode = tdr["CompCode"].ToString().Trim();
                        string tUnitCode = tdr["UnitCode"].ToString().Trim().ToUpper();
                        string tWrkGrp = tdr["WrkGrp"].ToString();
                        string tEmpName = tdr["EmpName"].ToString();
                        string tFatherName = tdr["FatherName"].ToString();
                        string tSex = tdr["Gender"].ToString().Substring(0, 1);
                        bool tActive = (tdr["Active"].ToString() == "Y" ? true : false);
                        bool tOTFLG = (tdr["OTFLG"].ToString().Trim() == "Y"?true:false);
                        
                        string tCatCode = tdr["CatCode"].ToString().Trim().ToUpper();
                        
                        string tDeptcode = tdr["DeptCode"].ToString().Trim().ToUpper();
                        string tStatCode = tdr["StatCode"].ToString().Trim().ToUpper();
                        string tDesgCode = tdr["DesgCode"].ToString().Trim().ToUpper();
                        string tGradeCode = tdr["GradeCode"].ToString().Trim().ToUpper();
                        string tContCode = tdr["ContCode"].ToString().Trim().ToUpper();
                        string tWeekoff = tdr["WeekOff"].ToString().Trim().ToUpper();
                        string tCardNo = tdr["CardNo"].ToString().Trim().ToUpper();
                        string tContactNo = tdr["ContactNo"].ToString ().Trim().ToUpper();
                        string tEmail = tdr["EmailID"].ToString().Trim();
                        string tCostCode = tdr["CostCode"].ToString().Trim().ToUpper();

                        DateTime tBirthDt = Convert.ToDateTime(tdr["BirthDt"]);
                        DateTime tJoinDt = Convert.ToDateTime(tdr["JoinDt"]);

                        clsEmp emp = new clsEmp();
                        err = string.Empty;

                        bool iscreated = emp.CreateEmployee(tCompCode, tEmpUnqID, tWrkGrp, tUnitCode, tEmpName, tFatherName,tSex, tContactNo, tEmail, tCardNo, tActive, tBirthDt, tJoinDt, out err);

                        if (string.IsNullOrEmpty(err))
                            tdr["Remarks"] = "Employee Created...";
                        else
                            tdr["Remarks"] = err;

                        if (iscreated)
                        {

                            //insert into job profile
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                string sql = "Insert into MastEmpJobProfile " +
                                    " ( EmpUnqiD,ValidFrom,UnitCode,DeptCode,StatCode,CatCode,DesgCode,GradeCode,ContCode,CostCode,WeekOff,OTFlg,AddDt,AddID ) " +
                                    " Values " +
                                    " ('" + tEmpUnqID + "','" + tJoinDt.ToString("yyyy-MM-dd") + "','" + tUnitCode + "','" + tDeptcode + "','" + tStatCode + "'," +
                                    " '" + tCatCode + "','" + tDesgCode + "','" + tGradeCode + "','" + tContCode + "','" + tCostCode + "','" + tWeekoff + "'," +
                                    " '" + (tOTFLG ? 1 : 0).ToString() + "',GetDate(),'" + Utils.User.GUserID + "')";


                                cmd.Connection = con;
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();


                                sql = "Insert into MastEmpIdentity ( EmpUnqID, AddDt,AddID ) Values " +
                                      "('" + tEmpUnqID + "',getdate(),'" + Utils.User.GUserID + "')";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                sql = "Insert into MastEmpAddress ( EmpUnqID,AddressType, AddDt,AddID ) Values " +
                                    " select '" + tEmpUnqID + "','PER',getdate(),'" + Utils.User.GUserID + "' union " +
                                    " select '" + tEmpUnqID + "','PRE',getdate(),'" + Utils.User.GUserID + "' ";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                //create muster

                                emp.EmpUnqID = tEmpUnqID;
                                emp.GetEmpDetails(tEmpUnqID);
                                string err1 = string.Empty;
                                if(tJoinDt.Year < DateTime.Now.Year)
                                    emp.CreateMuster(tEmpUnqID, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-01-01"),Convert.ToDateTime( DateTime.Now.Year.ToString("yyyy") + "-12-31"),out err1);
                                else
                                    emp.CreateMuster(tEmpUnqID, tJoinDt, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-12-31"), out err1);


                            }

                           
                        }
                    }

                    con.Close();
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
            catch(Exception ex){
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                string myexceldataquery = "select CompCode,EmpUnqID,WrkGrp,EmpName,FatherName,Gender,Active," +
                    " BirthDt,JoinDt,WeekOff,OTFLG," +
                    " ContCode,CatCode,UnitCode,DeptCode,StatCode,DesgCode,GradeCode," +
                    " CostCode, ContactNo,EmailID,CardNo, '' as Remarks from " + sheetname;

                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt = new DataTable();
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
                MessageBox.Show("Please Check upload template..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnImport.Enabled = false;
                grd_view.DataSource = null;
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

        private void frmMastEmpBulkEmployeeImport_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            grd_view.DataSource = null;
            btnImport.Enabled = false;
        }
    }
}