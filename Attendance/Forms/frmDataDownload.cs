using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Attendance.Classes;


namespace Attendance.Forms
{
    public partial class frmDataDownload : Form
    {
        DataTable SelDt = new DataTable();
        private string GType = string.Empty;
        private bool SelAllFlg = false;
        private DataSet srcDs = new DataSet();

        public frmDataDownload()
        {
            InitializeComponent();
        }

        private void LoadGrid()
        {
            string sql = "Select CONVERT(BIT,0) as SEL,* From ReaderConfig Where Active = 1 and Master = 0 Order By MachineDesc,IOFLG";
            
            srcDs = Utils.Helper.GetData(sql, Utils.Helper.constr);
            grpGrid.DataSource = srcDs;
            grpGrid.DataMember = srcDs.Tables[0].TableName;

            gv_avbl.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_avbl.Appearance.ViewCaption.Font, FontStyle.Bold);
            gv_avbl.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;


            foreach (GridColumn gc in gv_avbl.Columns)
            {
                gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            gv_avbl.RefreshData();



        }

        private void LockCtrl()
        {
            btnClearMach.Enabled = false;
            btnDownload.Enabled = false;
            btnRestartMach.Enabled = false;
            btnSelAll.Enabled = false;
            btnSetTime.Enabled = false;
            btnUnockMach.Enabled = false;
            grpGrid.Enabled = false;
        }

        private void UnLockCtrl()
        {
            btnClearMach.Enabled = true;
            btnDownload.Enabled = true;
            btnRestartMach.Enabled = true;
            btnSelAll.Enabled = true;
            btnSetTime.Enabled = true;
            btnUnockMach.Enabled = true;
            grpGrid.Enabled = true;
        }

        private void frmDataDownload_Load(object sender, EventArgs e)
        {
            
            SelDt.Columns.Add("MachineDesc", typeof(string));
            SelDt.Columns.Add("MachineIP", typeof(string));
            SelDt.Columns.Add("MachineNo", typeof(string));
            SelDt.Columns.Add("IOFLG", typeof(string));
            SelDt.Columns.Add("AutoClear", typeof(bool));
            SelDt.Columns.Add("CanteenFlg", typeof(bool));
            SelDt.Columns.Add("Remarks", typeof(string));
            SelDt.Columns.Add("Records", typeof(string));
            
            LoadGrid();
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            if (gv_avbl.DataRowCount <= 0)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            SelAllFlg = (!SelAllFlg);

            string tMachineNo = string.Empty,tMachineDesc = string.Empty, tMachineIP = string.Empty,tIOFLG = string.Empty, tLocation = string.Empty;
            int Records = 0;
            bool tAutoClear = false, tMess = false;

            for (int i = 0; i <= gv_avbl.DataRowCount - 1; i++)
            {
                if (SelAllFlg == true)
                {
                    tMachineNo = string.Empty; tMachineDesc = string.Empty; tMachineIP = string.Empty; tIOFLG = string.Empty;
                    tAutoClear = false; tMess = false;

                    gv_avbl.SetRowCellValue(i, "SEL", true);
                    
                    tMachineDesc = gv_avbl.GetRowCellValue(i, "MachineDesc").ToString();
                    tMachineIP = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                    tMachineNo = gv_avbl.GetRowCellValue(i, "MachineNo").ToString();
                    tIOFLG = gv_avbl.GetRowCellValue(i, "IOFLG").ToString();                    
                    tAutoClear = Convert.ToBoolean(gv_avbl.GetRowCellValue(i, "AutoClear").ToString());
                    tMess = Convert.ToBoolean(gv_avbl.GetRowCellValue(i, "CanteenFLG").ToString());

                    DataRow dr = SelDt.NewRow();

                    dr["MachineNo"] = tMachineNo;
                    dr["MachineDesc"] = tMachineDesc;
                    dr["MachineIP"] = tMachineIP;
                    dr["Records"] = Records;
                    dr["IOFLG"] = tIOFLG;
                    dr["AutoClear"] = tAutoClear;
                    dr["CanteenFlg"] = tMess;

                    SelDt.Rows.Add(dr);
                    SelDt.AcceptChanges();

                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "SEL", false);
                    
                }
                
                
            }

            if (SelAllFlg == false)
            {
                SelDt.Rows.Clear();
                SelDt.AcceptChanges();
            }

            grpGrid.Refresh();
            gv_avbl.RefreshData();

            Cursor.Current = Cursors.Default;
            
        }

        private void SetTime()
        {

        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;
            SetTime();

            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            foreach (DataRow dr in SelDt.Rows)
            {
                clsMachine m = new clsMachine(dr["MachineIP"].ToString(), dr["IOFLG"].ToString().Substring(0, 1));
                string err = string.Empty;
                List<AttdLog> records = new List<AttdLog>();
                
                //try to connect
                m.Connect(out err);
                dr["Remarks"] = err;
                int tRow = 0;
                for (int i = 0; i < gv_avbl.DataRowCount; i++) {
                    object b = gv_avbl.GetRowCellValue(i, "MachineIP");
                    if (b != null && b.Equals(dr["MachineIP"].ToString()))
                    {
                        gv_avbl.FocusedRowHandle = i;
                        tRow = i;
                        gv_avbl.SetRowCellValue(tRow,"Records", 0);
                        gv_avbl.SetRowCellValue(tRow,"Remarks", err);
                    }
                }
                


                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                //get records
                m.GetAttdRec(out records,out err);
                gv_avbl.SetRowCellValue(tRow, "Remarks", err);

                dr["Records"] = records.Count();
                gv_avbl.SetRowCellValue(tRow, "Records", records.Count());

                if (string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(tRow, "Download Completed...", err);
                }
                
            }

            UnLockCtrl();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void gv_avbl_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var gv = sender as GridView;
            //int rowHandle = gv.GetRowHandle(gv.DataRowCount);

            if (e.RowHandle == -2147483647)
            {
                return;
            }
            if (e.Column.FieldName == "SEL")
            {

                string tMachineNo = string.Empty, tMachineDesc = string.Empty, tMachineIP = string.Empty, tIOFLG = string.Empty, tLocation = string.Empty;
                int Records = 0;
                bool tAutoClear = false, tMess = false;

                if (Convert.ToBoolean(e.Value) == true)
                {
                    tMachineNo = string.Empty; tMachineDesc = string.Empty; tMachineIP = string.Empty; tIOFLG = string.Empty;
                    tAutoClear = false; tMess = false;

                    gv_avbl.SetRowCellValue(e.RowHandle, "SEL", true);

                    tMachineDesc = gv_avbl.GetRowCellValue(e.RowHandle, "MachineDesc").ToString();
                    tMachineIP = gv_avbl.GetRowCellValue(e.RowHandle, "MachineIP").ToString();
                    tMachineNo = gv_avbl.GetRowCellValue(e.RowHandle, "MachineNo").ToString();
                    tIOFLG = gv_avbl.GetRowCellValue(e.RowHandle, "IOFLG").ToString();
                    tAutoClear = Convert.ToBoolean(gv_avbl.GetRowCellValue(e.RowHandle, "AutoClear").ToString());
                    tMess = Convert.ToBoolean(gv_avbl.GetRowCellValue(e.RowHandle, "CanteenFLG").ToString());

                    DataRow dr = SelDt.NewRow();

                    dr["MachineNo"] = tMachineNo;
                    dr["MachineDesc"] = tMachineDesc;
                    dr["MachineIP"] = tMachineIP;
                    dr["Records"] = Records;
                    dr["IOFLG"] = tIOFLG;
                    dr["AutoClear"] = tAutoClear;
                    dr["CanteenFlg"] = tMess;

                    SelDt.Rows.Add(dr);
                    SelDt.AcceptChanges();

                }
                else
                {
                    tMachineIP = gv_avbl.GetRowCellValue(e.RowHandle, "MachineIP").ToString();
                    var rows = SelDt.Select("MachineIP = '" + tMachineIP + "'");
                    foreach (DataRow r in rows)
                    {
                        r.Delete();
                    }
                    SelDt.AcceptChanges();

                }
            }
        }

    }
}
