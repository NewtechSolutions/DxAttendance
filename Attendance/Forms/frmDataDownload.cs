using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.Forms
{
    public partial class frmDataDownload : Form
    {
        DataTable SelDt = new DataTable();
        private string GType = string.Empty;
        private bool SelAllFlg = false;
        
        public frmDataDownload()
        {
            InitializeComponent();
        }

        private void LoadGrid()
        {
            string sql = "Select CONVERT(BIT,0) as SEL,* From ReaderConfig Where Active = 1 and Master = 0 Order By MachineDesc,IOFLG";
            
            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            grd_avbl.DataSource = ds;
            grd_avbl.DataMember = ds.Tables[0].TableName;

            gv_avbl.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_avbl.Appearance.ViewCaption.Font, FontStyle.Bold);
            gv_avbl.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;


            foreach (GridColumn gc in gv_avbl.Columns)
            {
                gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            gv_avbl.RefreshData();



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

            grd_avbl.Refresh();
            gv_avbl.RefreshData();

            Cursor.Current = Cursors.Default;
            
        }



    }
}
