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
using System.Data.SqlClient;
using Attendance.Classes;
using System.IO;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Attendance.Forms
{
    public partial class frmMastUserManagement : DevExpress.XtraEditors.XtraForm
    {
       
        public static List<UserBioInfo> tUserList  = new List<UserBioInfo>();
        public string GRights = "XXXV";
        DataTable dt = new DataTable();
        public DataSet srcDs = new DataSet();
        public bool SelAllFlg = false;

        public frmMastUserManagement()
        {
            InitializeComponent();
        }

        private void LoadGrid(int tMachineType)
        {
            DataTable tmpmachine = srcDs.Tables[0].Copy();
            tmpmachine.Rows.Clear();
            DataRow[] dtrs ;

            switch (tMachineType)
            {
                case 1: //attendance
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 0");
                   
                    break;
                case 2: //canteen
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 1 And GateInOut = 0 and LunchInOut = 0");
                    break;
                case 3: //lunch inout
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 1");
                    break;
                case 4: //gate inout
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 1 and LunchInOut = 0");
                    break;
                default:
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 0");
                    break;
            }

            foreach (DataRow dr in dtrs)
            {
                
                tmpmachine.ImportRow(dr);
            }

            
            grpGrid.DataSource = tmpmachine;
            //grpGrid.DataMember = srcDs.Tables[0].TableName;

            gv_avbl.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_avbl.Appearance.ViewCaption.Font, FontStyle.Bold);
            gv_avbl.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            foreach (GridColumn gc in gv_avbl.Columns)
            {
                if (gc.Caption == "Location" || gc.Caption == "Remarks")
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }

                gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            gv_avbl.RefreshData();

        }

        private void txtMessGrpCode_Validated(object sender, EventArgs e)
        {
            if (txtMessGrpCode.Text.Trim() == "" )
            {
                txtMessGrpDesc.Text = "";
                return;
            }

            txtMessGrpCode.Text = txtMessGrpCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastMessGrp where CompCode = '01' and " +
                    " UnitCode ='" + txtUnitCode.Text.Trim() + "' and MessGrpCode= '" + txtMessGrpCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtMessGrpCode.Text = dr["MessGrpCode"].ToString();
                    txtMessGrpDesc.Text = dr["MessGrpDesc"].ToString();
                    

                }
            }
        }

        private void txtUnitCode_Validated(object sender, EventArgs e)
        {
            if (txtUnitCode.Text.Trim() == "" )
            {
                txtUnitDesc.Text = "";
                return;
            }

            txtUnitCode.Text = txtUnitCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastUnit where CompCode ='01' " +
                    " and  UnitCode ='" + txtUnitCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtUnitDesc.Text = dr["UnitName"].ToString();
                   

                }
            }
            else
            {
                txtUnitDesc.Text = "";
            }
        }

        private void txtMessCode_Validated(object sender, EventArgs e)
        {
            if ( txtMessCode.Text.Trim() == "")
            {
                txtMessDesc.Text = "";
                return;
            }

            txtMessCode.Text = txtMessCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastMess where CompCode ='01' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' and MessCode ='" + txtMessCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtMessCode.Text = dr["MessCode"].ToString();
                    txtMessDesc.Text = dr["MessDesc"].ToString();
                    

                }
            }
            else
            {
                txtMessDesc.Text = "";
            }

            
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {

            if (txtWrkGrpCode.Text.Trim() == "")
            {
                txtWrkGrpDesc.Text = "";
                return;
            }
            
            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='01'  and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtWrkGrpDesc.Text = dr["WrkGrpDesc"].ToString();                   
                }
            }
            else
            {
                txtWrkGrpDesc.Text = "";
            }


        }

        private void txtEmpUnqID_Validated(object sender, EventArgs e)
        {
            if (txtEmpUnqID.Text.Trim() == "")
            {
                txtEmpName.Text = "";
                txtUnitCode.Text = "";
                txtUnitDesc.Text = "";

                txtWrkGrpCode.Text = "";
                txtWrkGrpDesc.Text = "";

                txtMessCode.Text = "";
                txtMessDesc.Text = "";

                txtMessGrpCode.Text = "";
                txtMessGrpDesc.Text = "";
                txtOldRFID.Text = "";

                chkActive.Checked = false;
                chkComp.Checked = false;
                chkCont.Checked = false;
                chkFace.Checked = false;
                chkFinger.Checked = false;
                chkRFID.Checked = false;
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select EmpUnqID,EmpName,WrkGrp,UnitCode,MessCode,MessGrpCode,PayrollFlg,ContractFlg,Active From MastEmp where EmpUnqID ='" + txtEmpUnqID.Text.Trim() + "' And Active = 1";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtMessCode.Text = dr["MessCode"].ToString();
                    txtMessGrpCode.Text = dr["MessGrpCode"].ToString();
                    txtEmpName.Text = dr["EmpName"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();

                    chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                    chkComp.Checked = Convert.ToBoolean(dr["PayrollFlg"]);
                    chkCont.Checked = Convert.ToBoolean(dr["ContractFlg"]);
                    
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);
                    txtMessCode_Validated(sender, e);
                    txtMessGrpCode_Validated(sender, e);
                                   

                } //foreach employee
            }
            else
            {
                txtEmpName.Text = "";
                txtWrkGrpCode.Text = "";
                txtWrkGrpDesc.Text = "";
                txtMessCode.Text = "";
                txtMessDesc.Text = "";
                txtMessGrpCode.Text = "";
                txtMessGrpDesc.Text = "";
                txtOldRFID.Text = "";
                chkActive.Checked = false;
                chkComp.Checked = false;
                chkCont.Checked = false;
                chkFace.Checked = false;
                chkFinger.Checked = false;
                chkRFID.Checked = false;
            }

            UserBioInfo x = new UserBioInfo();
            x.GetBioInfoFromDB(txtEmpUnqID.Text.Trim());
            chkRFID.Checked = ((!string.IsNullOrEmpty(x.CardNumber)) ? true : false);
            chkFace.Checked = ((!string.IsNullOrEmpty(x.FaceTemp)) ? true : false);
            chkFinger.Checked = ((!string.IsNullOrEmpty(x.FingerTemp)) ? true : false);
            txtOldRFID.Text = x.CardNumber;
            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            DataTable dtMaterial = new DataTable();
            DataTable sortedDT = new DataTable();
            try
            {


                foreach (GridColumn column in gv_Upload.VisibleColumns)
                {
                    if (column.FieldName != string.Empty)
                        dtMaterial.Columns.Add(column.FieldName, column.ColumnType);
                }


                for (int i = 0; i < gv_Upload.DataRowCount; i++)
                {
                    DataRow row = dtMaterial.NewRow();

                    foreach (GridColumn column in gv_Upload.VisibleColumns)
                    {
                        row[column.FieldName] = gv_Upload.GetRowCellValue(i, column);
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
                        


                        


                        
                    }

                    con.Close();
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("file uploaded Successfully, please check the remarks for indivisual record status...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            DataSet ds = new DataSet();
            ds.Tables.Add(sortedDT);
            grd_Upload.DataSource = ds;
            grd_Upload.DataMember = ds.Tables[0].TableName;
            grd_Upload.Refresh();

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
            grd_Upload.DataSource = null;
            string filePath = txtBrowse.Text.ToString();

            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath + ";extended properties=" + "\"excel 8.0;hdr=yes;IMEX=1;\"";

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            oledbconn.Open();
            string str = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
            string sheetname = "[" + str.Replace("'", "") + "]";

            try
            {
                string myexceldataquery = "select EmpUnqID,'' as Remarks from " + sheetname;
                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt.Clear();
                oledbda.Fill(dt);
                oledbconn.Close();
            }
            catch (Exception ex)
            {
                oledbconn.Close();
                MessageBox.Show("Please Check upload template..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnImport.Enabled = false;
                oledbconn.Close();
                return;
            }


            DataView dv = dt.DefaultView;
            dv.Sort = "EmpUnqID asc";
            DataTable sortedDT = dv.ToTable();




            grd_Upload.DataSource = sortedDT;

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
                            grd_Upload.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            grd_Upload.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            grd_Upload.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            grd_Upload.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            grd_Upload.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            grd_Upload.ExportToMht(exportFilePath);
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

        private void frmMastUserManagement_Load(object sender, EventArgs e)
        {
            
            cmbListMachine1.Properties.Items.Add(Globals.MasterMachineIP + "-" + "Master");

            //load all machine ip in combo
            DataSet ds = new DataSet();
            ds = Utils.Helper.GetData("Select MachineIP From ReaderConfig where master = 0 and active = 1 Order By MachineNo", Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cmbListMachine1.Properties.Items.Add(dr["MachineIP"].ToString());
                    cmbListMachine2.Properties.Items.Add(dr["MachineIP"].ToString());
                }
            }
            cmbListMachine1.SelectedIndex = 0;

            string sql = "Select CONVERT(BIT,0) as SEL,MachineDesc,MachineIP,MachineNo,'' as Remarks,AutoClear,IOFLG,RFID,FACE,CanteenFLG,GateInOut,LunchInOut From ReaderConfig Where Active = 1 and Master = 0 Order By MachineDesc,IOFLG";

            srcDs = Utils.Helper.GetData(sql, Utils.Helper.constr);

            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            grd_Upload.DataSource = null;
            btnImport.Enabled = false;
            optMachineType.EditValue = 1;
        }

        private void optMachineType_EditValueChanged(object sender, EventArgs e)
        {
            //load machine type to grid...
            LoadGrid(Convert.ToInt32(optMachineType.EditValue));
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            if (gv_avbl.DataRowCount <= 0)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            SelAllFlg = (!SelAllFlg);

            for (int i = 0; i <= gv_avbl.DataRowCount - 1; i++)
            {
                if (SelAllFlg == true)
                {
                    gv_avbl.SetRowCellValue(i, "SEL", true);

                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "SEL", false);

                }

            }

            Cursor.Current = Cursors.Default;
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            if(txtEmpUnqID.Text.Trim() == string.Empty || txtEmpName.Text.Trim() == string.Empty)
            {
                //MessageBox.Show("Invalid Employee...","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            string tEmpUnqID = txtEmpUnqID.Text.Trim();
            UserBioInfo user = new UserBioInfo();            
            user.GetBioInfoFromDB(tEmpUnqID);
            if (user.UserID == tEmpUnqID)
            {
                tUserList.RemoveAll(tmpuser => tmpuser.UserID == tEmpUnqID);
                tUserList.Add(user);   
                grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();
            }

            txtEmpUnqID.Text = "";
            txtEmpUnqID_Validated(sender, e);
            txtEmpUnqID.Focus();
        }

        private void btnDelEmp_Click(object sender, EventArgs e)
        {
            string tEmpUnqID = "";
            if (gv_Emp.SelectedRowsCount <= 0)
                return;

            foreach (int i in gv_Emp.GetSelectedRows())
            {
                tEmpUnqID = gv_Emp.GetRowCellValue(i, "UserID").ToString();
                tUserList.RemoveAll(tmpuser => tmpuser.UserID == tEmpUnqID);
            }

            grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();
        }

        private void btn_MasterDownload_Click(object sender, EventArgs e)
        {
            if (cmbListMachine1.Text.Trim() == string.Empty)
            {
                return;
            }
            lblDownAll.Text = "";

            string err;
            //machine selection
            if (cmbListMachine1.Text.Contains(Globals.MasterMachineIP))
            {
                clsMachine tmach = new clsMachine(Globals.MasterMachineIP, "B");
                tmach.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    lblDownAll.Text = err;
                    return;
                }
               
                lblDownAll.Text = "Downloading...";
                lblDownAll.Update();

                this.Cursor = Cursors.WaitCursor;
                List<UserBioInfo> tusers = new List<UserBioInfo>();
                
                tmach.DownloadALLUsers(true,out err, out tusers);
                lblDownAll.Text = tusers.Count().ToString() + " Downloaded Users";
                lblDownAll.Update();
                this.Cursor = Cursors.Default;
                MessageBox.Show("Download Complete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                clsMachine tmach = new clsMachine(cmbListMachine1.Text.Trim(), "B");
                tmach.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    lblDownAll.Text = err;
                    return;
                }


                lblDownAll.Text = "Downloading...";
                lblDownAll.Update();

                this.Cursor = Cursors.WaitCursor;
                List<UserBioInfo> tusers = new List<UserBioInfo>();

                tmach.DownloadALLUsers(true,out err, out tusers);
                tmach.DisConnect(out err);
                lblDownAll.Text = tusers.Count().ToString() + " Downloaded Users";
                lblDownAll.Update();

                this.Cursor = Cursors.Default;
                MessageBox.Show("Download Complete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            
            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                
                string allerr = "";
                foreach (UserBioInfo emp in tUserList)
                {
                    
                    m.Register(emp.UserID, out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        allerr += err + Environment.NewLine;
                    }
                }

                

                if (string.IsNullOrEmpty(allerr))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);

            }


            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void ResetRemarks()
        {
            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                gv_avbl.SetRowCellValue(i, "Remarks", "");
            }
        }
        private void LockCtrl()
        {
            grpButtons1.Enabled = false;
            grpButtons2.Enabled = false;

            grpButtons3.Enabled = false;
            grpButtons4.Enabled = false;

            grpButtons5.Enabled = false;
            grpButtons6.Enabled = false;

            grpButtons7.Enabled = false;
            grpButtons8.Enabled = false;

            grpButtons9.Enabled = false;
            grpButtons10.Enabled = false;

            grpButtons11.Enabled = false;
            grpButtons12.Enabled = false;
            grpButtons13.Enabled = false;

           
            grpGrid.Enabled = false;
        }
        private void UnLockCtrl()
        {
            grpButtons1.Enabled = true;
            grpButtons2.Enabled = true;
            
            grpButtons3.Enabled = true;
            grpButtons4.Enabled = true;

            grpButtons5.Enabled = true;
            grpButtons6.Enabled = true;

            grpButtons7.Enabled = true;
            grpButtons8.Enabled = true;

            grpButtons9.Enabled = true;
            grpButtons10.Enabled = true;

            grpButtons11.Enabled = true;
            grpButtons12.Enabled = true;

            grpButtons13.Enabled = true;

            grpGrid.Enabled = true;
        }

        private void btnDownTemp_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                //grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

                //user bulk method
                List<UserBioInfo> tempusers = new List<UserBioInfo>();
                m.DownloadTemplate(tUserList, out err, out tempusers);
                
                string allerr = "";
                foreach (UserBioInfo emp in tempusers)
                {
                    allerr +=  (emp.err.Length > 0 ? emp.UserID + emp.err : "");                  
                }

                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine,"")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);
                grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }


            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                //user bulk method
                List<UserBioInfo> tempusers = new List<UserBioInfo>();
                m.DeleteUser(tUserList, out err, out tempusers);

                string allerr = "";
                foreach (UserBioInfo emp in tempusers)
                {
                    allerr += (emp.err.Length > 0 ? emp.UserID + emp.err : "");
                }

                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);
                grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }


            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                string allerr = "";

                foreach (UserBioInfo emp in tUserList)
                {
                    m.BlockUser(emp.UserID, out err);
                    allerr += (err.Length > 0 ? emp.UserID + err + Environment.NewLine : "");
                }
                
                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);
                //grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }

            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnUnBlock_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                string allerr = "";

                foreach (UserBioInfo emp in tUserList)
                {
                    m.UnBlockUser(emp.UserID, out err);
                    allerr += (err.Length > 0 ? emp.UserID + err + Environment.NewLine : "");
                }

                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);
                //grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }

            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnUnlockMaster_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            
            Cursor.Current = Cursors.WaitCursor;

            string ip = Globals.MasterMachineIP;
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            //try to connect
            m.Connect(out err);
            if(!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m.Unlock(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Master Machine Unlocked", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            m.DisConnect(out err);
            Cursor.Current = Cursors.Default;
            
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remraks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                m.Unlock(out err);
                
                if (string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                }

                m.DisConnect(out err);
               

            }

            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnUpdateRFID_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtEmpUnqID.Text.Trim()))
            {
                MessageBox.Show("Please Enter EmpUnqID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtNewRFID.Text.Trim()))
            {
                MessageBox.Show("New RFID Card No is required..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!Globals.GetWrkGrpRights(655,txtWrkGrpCode.Text.Trim(),txtEmpUnqID.Text.Trim()))
            {
                MessageBox.Show("You are not authorised for this kind of employee..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    con.Open();
                    string tEmpUnqID = txtEmpUnqID.Text.Trim();
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(txtOldRFID.Text.Trim()))
                    {
                        MessageBox.Show("Old RFID Detais not Found, System is Adding new RFID Card", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sql = "Insert Into EmpBioData (EmpUnqID,MachineIP,Type,idx,MachineNo,EmpName,Pass,Privilege,Blocked,RFIDNO,AddDt,AddID)" +
                                 " Values ('" + tEmpUnqID + "' " +
                                 " ,'" + "Master" + "'" +
                                 " ,'RFID',10 " +
                                 " ,'9999'  " +
                                 " ,'" + txtEmpName.Text.Trim() + "' " +
                                 " ,'' " +
                                 " ,'0'" +
                                 " ,'1'" +
                                 " ,'" + txtNewRFID.Text.Trim() + "',GetDate(),'" + Utils.User.GUserID + "')";                        
                    }
                    else
                    {
                        sql = "Update EmpBioData Set RFIDNO = '" + txtNewRFID.Text.Trim() + "' where Type = 'RFID' And EmpUnqID ='" + tEmpUnqID + "' and MachineIP='Master'";
                    
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("RFID Card No is changed..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                con.Close();
            }

        }

        private void btnGetExcelUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd1.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ip = txtIPAdd1.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LockCtrl();
            
            this.Cursor = Cursors.WaitCursor;

            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadALLUsers(false, out err, out tmpuser);

            if (tmpuser.Count > 0)
            {
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                        string reqno;
                        reqno = Utils.Helper.GetDescription("Select isnull(Max(ReqNo),0) + 1 from tmp_machineusers", Utils.Helper.constr);
                        string sql = string.Empty;

                        SqlCommand cmd = new SqlCommand();

                        foreach (UserBioInfo emp in tmpuser)
                        {
                            sql = "Insert Into tmp_machineusers (ReqNo,MachineIP,EmpUnqID,RFID,AMthD,AddDt,AddID )" +
                            " Values ('" + reqno + "','" + ip + "','" + emp.UserID + "','" + emp.CardNumber + "','0',GetDate(),'" + Utils.User.GUserID +  "')";

                            cmd = new SqlCommand(sql, cn);
                            cmd.ExecuteNonQuery();
                        }
                        
                        //update other info in tmp_machineusers
                        sql = "Update a " +
                              " Set a.WrkGrp = b.WrkGrp , a.EmpName = b.EmpName , a.Active = b.Active " +
                              " From tmp_machineusers a , MastEmp b " +
                              " Where a.EmpUnqID = b.EmpUnqID and ReqNo = '" + reqno + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.ExecuteNonQuery();

                        lblReqNo.Text = reqno;

                    }
                    catch (Exception ex)
                    {
                        UnLockCtrl();
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        return;
                    }
                }
            }

            UnLockCtrl();
            this.Cursor = Cursors.Default;
            MessageBox.Show("Request Generated : " + lblReqNo.Text.Trim(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


    }
}