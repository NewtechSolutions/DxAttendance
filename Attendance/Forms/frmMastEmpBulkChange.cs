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
using System.Globalization;

namespace Attendance.Forms
{
    public partial class frmMastEmpBulkChange : DevExpress.XtraEditors.XtraForm
    {
        public string GRights = "XXXV";
       


        DataTable dt = new DataTable();

        public frmMastEmpBulkChange()
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
            clsEmp t = new clsEmp();
            
            t.EmpUnqID = tdr["EmpUnqID"].ToString();
            if (!t.GetEmpDetails(t.EmpUnqID))
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
                return err;
            }
            
            if(tdr["ValidFrom"] == DBNull.Value)
            {
                err = "Valid From Date is required.." + Environment.NewLine;
                return err;
            }

            //validfrom
            string expectedFormat = "yyyy-MM-dd";
            DateTime theDate;
            bool result = DateTime.TryParseExact(
                tdr["ValidFrom"].ToString(),
                expectedFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out theDate);

            if (!result)
            {
                err = "Valid From Date...format error (use yyyy-MM-dd format)" + Environment.NewLine;
                return err;
            }

            if(theDate < t.JoinDt)
            {
                err = "Valid From Date...could not be less then join date" + Environment.NewLine;
                
            }

            string tUnitCode = tdr["UnitCode"].ToString();
            string tDeptCode = tdr["DeptCode"].ToString();
            string tStatCode = tdr["StatCode"].ToString();
            string tDesgCode = tdr["DesgCode"].ToString();
            string tGradeCode = tdr["GradeCode"].ToString();
            string tCatCode = tdr["CatCode"].ToString();

            string tContCode = tdr["ContCode"].ToString();
            string tCostCode = tdr["CostCode"].ToString();
            string tWeekOff = tdr["WeekOff"].ToString();

            string desc = "";
            if (!Utils.MastCodeValidate.GetUnitDesc(t.CompCode, t.WrkGrp, tUnitCode, out desc) && tUnitCode != "")
                err += "Invalid UnitCode.." + Environment.NewLine;
            if (!Utils.MastCodeValidate.GetDeptDesc(t.CompCode, t.WrkGrp, tUnitCode,tDeptCode, out desc) && tDeptCode != "")
                err += "Invalid DeptCode.." + Environment.NewLine;
            if (!Utils.MastCodeValidate.GetStatDesc(t.CompCode, t.WrkGrp, tUnitCode, tDeptCode,tStatCode ,out desc) && tStatCode != "")
                err += "Invalid StatCode.." + Environment.NewLine;
            if (!Utils.MastCodeValidate.GetCatDesc(t.CompCode, t.WrkGrp, tCatCode,  out desc) && tCatCode != "")
                err += "Invalid CatCode.." + Environment.NewLine;
            if (!Utils.MastCodeValidate.GetDesgDesc(t.CompCode, t.WrkGrp, tDesgCode, out desc) && tDesgCode != "")
                err += "Invalid StatCode.." + Environment.NewLine;
            if (!Utils.MastCodeValidate.GetGradeDesc(t.CompCode, t.WrkGrp, tGradeCode, out desc) && tGradeCode != "")
                err += "Invalid StatCode.." + Environment.NewLine;

            if (!Utils.MastCodeValidate.GetContDesc(t.CompCode, t.WrkGrp, tContCode,tUnitCode ,out desc) && tContCode != "")
                err += "Invalid ContCode.." + Environment.NewLine;

            if (!Utils.MastCodeValidate.GetCostCode(tCostCode, out desc) && tCostCode != "")
                err += "Invalid CostCode.." + Environment.NewLine;

            if(tWeekOff != "")
            {
                string tdays = "SUN,MON,TUE,WED,THU,FRI,SAT";
                if (!tdays.Contains(tWeekOff.ToUpper()))
                {
                    err += "Invalid Weekoff.use any from (SUN,MON,TUE,WED,THU,FRI,SAT)." + Environment.NewLine;
                }
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
                    foreach (DataRow dr in sortedDT.Rows)
                    {
                        string tEmpUnqID = dr["EmpUnqID"].ToString();

                        string err = DataValidate(dr);

                        if (!string.IsNullOrEmpty(err))
                        {
                            dr["Remarks"] = err;
                            continue;
                        }

                        
                        DateTime ValidFrom = Convert.ToDateTime(dr["ValidFrom"]);
                        string tsql = "Select top 1 * from MastEmpJobProfile where ValidFrom <='" + ValidFrom.ToString("yyyy-MM-dd") + "' and EmpUnqID ='" + tEmpUnqID + "' order by ValidFrom Desc";
                        DataSet rds = Utils.Helper.GetData(tsql, Utils.Helper.constr, out string terr);
                        if (!string.IsNullOrEmpty(terr))
                        {
                            dr["Remarks"] = terr;
                            return;
                        }

                        #region PrevData
                        DataRow prvrow;
                        string pUnitCode = "", pDeptCode = "", pStatCode = "", pCatCode = "", pDesgCode = "", pGradeCode = "", pContCode = "", pCostCode = "", pWeekOff = "";
                        bool pOtFLG = false;
                        DateTime pValidFrom;
                        bool hasRows = rds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (!hasRows)
                        {
                            pValidFrom = new DateTime(2000, 1, 1);
                            ValidFrom = Convert.ToDateTime(Utils.Helper.GetDescription("Select JoinDt from MastEmp Where EmpUnqID='" + tEmpUnqID + "'", Utils.Helper.constr));
                        }
                        else
                        {
                            prvrow = rds.Tables[0].Rows[0];
                            pValidFrom = Convert.ToDateTime(prvrow["ValidFrom"]);
                            pUnitCode = prvrow["UnitCode"].ToString();
                            pDeptCode = prvrow["DeptCode"].ToString();
                            pStatCode = prvrow["StatCode"].ToString();
                            pCatCode = prvrow["CatCode"].ToString();
                            pDesgCode = prvrow["DesgCode"].ToString();
                            pGradeCode = prvrow["GradeCode"].ToString();
                            pContCode = prvrow["ContCode"].ToString();
                            pCostCode = prvrow["CostCode"].ToString();
                            pWeekOff = prvrow["WeekOff"].ToString();
                            pOtFLG = Convert.ToBoolean(prvrow["OTFLG"]);
                        }

                        #endregion

                        string tUnitCode = dr["UnitCode"].ToString();
                        string tDeptCode = dr["DeptCode"].ToString();
                        string tStatCode = dr["StatCode"].ToString();                       
                        string tCatCode = dr["CatCode"].ToString();
                        string tDesgCode = dr["DesgCode"].ToString();
                        string tGradeCode = dr["GradeCode"].ToString();
                        string tContCode = dr["ContCode"].ToString();
                        string tCostCode = dr["CostCode"].ToString();
                        string tWeekOff = dr["WeekOff"].ToString();
                        string tOtFLG = dr["OTFLG"].ToString() ;

                        #region prvSet_if_blank
                        if (string.IsNullOrEmpty(tUnitCode))
                        {
                            tUnitCode = pUnitCode;
                        }

                        if (string.IsNullOrEmpty(tDeptCode))
                        {
                            tDeptCode = pDeptCode;
                        }

                        if (string.IsNullOrEmpty(tStatCode))
                        {
                            tStatCode = pStatCode;
                        }

                        if (string.IsNullOrEmpty(tGradeCode))
                        {
                            tGradeCode = pGradeCode;
                        }

                        if (string.IsNullOrEmpty(tDesgCode))
                        {
                            tDesgCode = pDesgCode;
                        }

                        if (string.IsNullOrEmpty(tCatCode))
                        {
                            tCatCode = pCatCode;
                        }

                        if (string.IsNullOrEmpty(tContCode))
                        {
                            tContCode = pContCode;
                        }

                        if (string.IsNullOrEmpty(tCostCode))
                        {
                            tCostCode = pCostCode;
                        }

                        if (string.IsNullOrEmpty(tWeekOff))
                        {
                            tWeekOff = pWeekOff;
                        }

                        if (string.IsNullOrEmpty(tWeekOff))
                        {
                            tWeekOff = "SUN";
                        }
                        if (string.IsNullOrEmpty(tOtFLG))
                        {
                            tOtFLG = (pOtFLG?"Y":"N");
                        }

                        #endregion


                        #region Final_Update

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            try
                            {

                                cmd.Connection = con;
                                cmd.CommandType = CommandType.Text;
                                string sql = String.Empty;
                                if (pValidFrom == ValidFrom)
                                {
                                    sql = "Update MastEmpJobProfile set " +
                                        " UnitCode='" + tUnitCode + "', DeptCode = '" + tDeptCode + "',StatCode='" + tStatCode + "'," +
                                        " Desgcode='" + tDesgCode + "', GradeCode='" + tGradeCode + "', CatCode='" + tCatCode + "'," +
                                        " CostCode='" + tCostCode + "',ContCode ='" + tContCode + "',WeekOff='" + tWeekOff + "'," +
                                        " UpdDt =GetDate() , UpdID ='" + Utils.User.GUserID + "'" +
                                        " Where EmpUnqID ='" + tEmpUnqID + "' and ValidFrom ='" + ValidFrom.ToString("yyyy-MM-dd") + "'";
                                }
                                else
                                {
                                    sql = "insert into MastEmpJobProfile (EmpUnqID,ValidFrom,UnitCode,DeptCode,StatCode,DesgCode,GradeCode,CatCode,CostCode,ContCode,WeekOff,OTFLG,AddDt,AddID)" +
                                    " Values ('" + tEmpUnqID + "','" + ValidFrom.ToString("yyyy-MM-dd") + "','" + tUnitCode + "','" + tDeptCode + "'," +
                                    "'" + tStatCode + "','" + tDesgCode + "','" + tGradeCode + "','" + tCatCode + "'," +
                                    "'" + tCostCode + "','" + tContCode + "','" + tWeekOff + "','" + (tOtFLG == "Y" ? 1 : 0).ToString() + "',Getdate(),'" + Utils.User.GUserID + "')";

                                }

                                cmd.CommandText = sql;
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();

                                dr["remarks"] = "Updated..";

                            }
                            catch (Exception ex)
                            {
                                dr["remarks"] = dr["remarks"].ToString() + ex.ToString();
                                continue;
                            }

                        }//using sqlcommand
                        #endregion
                    }//using foreach

                    con.Close();
                }//using connection

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            


            try
            {
                string myexceldataquery = "select EmpUnqID,ValidFrom,CatCode,GradeCode,DesgCode,UnitCode,DeptCode,StatCode,ContCode,CostCode,WeekOff,OTFLG, " +
                    " '' as Remarks from " + sheetname;

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

        private void frmMastEmpBulkChange_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
                
            grd_view.DataSource = null;
            btnImport.Enabled = false;
        }
    }
}