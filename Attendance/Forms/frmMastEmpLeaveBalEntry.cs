using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Attendance.Forms
{
    public partial class frmMastEmpLeaveBalEntry : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";

        DataSet LeaveTypeDS = new DataSet();
        DataTable LeaveTypeTB = new DataTable();

        public frmMastEmpLeaveBalEntry()
        {
            InitializeComponent();
            this.ctrlEmp1.EmpUnqIDValidated += new EventHandler(this.ctrlEmpValidateEvent_Handler);
            //this.ctrlEmp1.CompCodeValidated += new EventHandler(this.ctrlCompValidateEvent_Handler);
        }

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            txtLeaveType.Properties.Items.Clear();
            grid.DataSource = null;

            if (!ctrlEmp1.cEmp.Active)
            {
                grid.DataSource = null;
            }
            else
            {
                //load Leave Types
                if(ctrlEmp1.txtWrkGrpCode.Text.Trim() != "")
                {
                    
                    DataRow[] tdr = LeaveTypeTB.Select("WrkGrp='" + ctrlEmp1.cEmp.WrkGrp + "'");
                    foreach (DataRow dr in tdr)
                    {
                        txtLeaveType.Properties.Items.Add(dr["LeaveTyp"].ToString());
                    }

                }
            } 
        }

        

        private void frmMastEmpLeaveBalEntry_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();

            LeaveTypeDS = Utils.Helper.GetData("Select * from MastLeave Where Compcode='" + ctrlEmp1.txtCompCode.Text.Trim().ToString() +"'  and KeepBal = 1 ", Utils.Helper.constr);
            //select wrkgrp from LeaveTyps
            bool hasRows = LeaveTypeDS.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                LeaveTypeTB = LeaveTypeDS.Tables[0];
            }

        }

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid )
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.WrkGrpDesc.Trim().ToString()))
            {
                err = err + "Invalid WrkGrp Code..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(txtYear.Text))
            {
                err = err + "Please Enter Year..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtLeaveType.Text))
            {
                err = err + "Please Enter LeaveType Description..." + Environment.NewLine;
            }

            if (txtEncDay.Value > 0)
            {
                //check if encash is allowed..

                //load Leave Types
                if (ctrlEmp1.txtWrkGrpCode.Text.Trim() != "")
                {

                    DataRow[] tdr = LeaveTypeTB.Select("WrkGrp='" + ctrlEmp1.cEmp.WrkGrp + "' and AllowEncash = 1 and LeaveTyp ='" + txtLeaveType.Text.Trim() + "'");

                    if (tdr.GetLength(0) == 0)
                    {
                        err = err + "Leave Encash is not Allowed..." + Environment.NewLine;
                    }

                }

            }


            //check of opening leave days in year if employee already taken more than
            if (txtOpnBal.Value >= 0)
            {
                string sql = "SELECT isnull(AVL,0) as AVL FROM LeaveBal "
                    + " where EmpUnqID = '" + ctrlEmp1.cEmp.EmpUnqID + "' "                   
                    + " And tYear ='" + txtYear.Text.Trim() + "' "
                    + " And LeaveTyp ='" + txtLeaveType.Text.Trim() + "'";

                string tcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

                int tavl = 0;
                if (int.TryParse(tcnt, out tavl))
                {
                    if (tavl > txtOpnBal.Value)
                    {
                        err = err + "Employee already taken more Leave than specified..." + Environment.NewLine;
                    }
                }

            }


            

            return err;
        }

        private void ResetCtrl()
        {
            //btnAdd.Enabled = false;

            ctrlEmp1.ResetCtrl();
            
            txtYear.Text = "";

            txtYear.Enabled = true;            
            txtLeaveType.Properties.Items.Clear();
            txtLeaveType.Text = "";
            txtOpnBal.Value = 0;
            txtEncDay.Value = 0;

            grid.DataSource = null;
            
            oldCode = "";
            mode = "NEW";
        }
        
        private void SetRights()
        {
            if ( GRights.Contains("A") )
            {
                btnAdd.Enabled = true;
               
            }
            else if (GRights.Contains("U"))
            {
                btnAdd.Enabled = true;
               
            }

            if (GRights.Contains("XXXV"))
            {
                btnAdd.Enabled = false;
              
            }
        }

        private void txtYear_Validated(object sender, EventArgs e)
        {
            if (txtYear.Text.Trim() == "")
            {
                grid.DataSource = null;
                return;
            }

            LoadGrid();
            SetRights();
        }

       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                //check the encashed master if any and update the encash field
                string tEmpUnqID = ctrlEmp1.cEmp.EmpUnqID;
                string tLeaveTyp = txtLeaveType.Text.Trim().ToString();
                string tYear = txtYear.Text.Trim().ToString();

                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;


                        string sql = "SELECT Count(*) FROM LeaveBal "
                            + " where EmpUnqID = '" + tEmpUnqID + "' "                          
                            + " And tYear ='" + tYear + "' "
                            + " And LeaveTyp ='" + tLeaveTyp + "'";

                        string tcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

                        int tavl = 0;
                        if (int.TryParse(tcnt, out tavl))
                        {
                            if (tavl == 0)
                            {
                                sql = "Insert into LeaveBal (tYear,EmpUnqID,LeaveTyp,OPN,AVL,BAL,ADV,ENC,AddDt,AddID) Values " +
                                     " ('{0}','{1}','{2}','{3}',0,'{4}',0,0,GetDate(),'{5}')";
                                sql = string.Format(sql, tYear,
                                    tEmpUnqID, tLeaveTyp,
                                    txtOpnBal.Value.ToString(),
                                    txtOpnBal.Value.ToString(),
                                    Utils.User.GUserID);


                                if (!GRights.Contains("A"))
                                {
                                    MessageBox.Show("You are not authorised", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                            }
                            else
                            {
                                sql = "Update LeaveBal set OPN = '{0}', UpdDt = GetDate(),UpdID ='{1}' " +
                                     " Where EmpUnqID = '{2}' and tYear ='{3}' " +
                                     " And LeaveTyp ='{4}' ";
                                sql = string.Format(sql, txtOpnBal.Value,   Utils.User.GUserID, 
                                    tEmpUnqID,  tYear, tLeaveTyp                                    
                                   );

                                if (!GRights.Contains("U"))
                                {
                                    MessageBox.Show("You are not authorised", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                            }
                        }
                        else
                        {
                            sql = "Insert into LeaveBal (tYear,EmpUnqID,LeaveTyp,OPN,AVL,BAL,ADV,ENC,AddDt,AddID) Values " +
                                    " ('{0}','{1}','{2}','{3}',0,'{4}',0,0,GetDate(),'{5}')";
                            sql = string.Format(sql,  tYear,tEmpUnqID, tLeaveTyp,
                                txtOpnBal.Value.ToString(),
                                txtOpnBal.Value.ToString(),
                                Utils.User.GUserID);
                        }

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        //check from MastEmpLeaveEncash for the year 
                        int Encash = 0;
                        sql = "select isnull(sum(EncashNo), 0) from MastEmpLeaveEncash where EmpUnqID = '" + tEmpUnqID + "' " +
                            " and LeaveTyp = '" + tLeaveTyp + "' and tYear ='" + tYear + "'";
                        string strEncash = Utils.Helper.GetDescription(sql, Utils.Helper.constr, out err);
                        if (int.TryParse(strEncash,out Encash))
                        {
                            sql = " Update LeaveBal Set ENC = '" + Encash.ToString() + "' Where EmpUnqID = '" + tEmpUnqID + "' " +
                            " and LeaveTyp = '" + tLeaveTyp + "' and tYear ='" + tYear + "'";

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }



                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();
                       
                        return;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadGrid()
        {
            
            if(string.IsNullOrEmpty(ctrlEmp1.txtWrkGrpCode.Text.Trim().ToString())
                || string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString())
                || string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString())
                || string.IsNullOrEmpty(txtYear.Text.Trim())

                )
            {
                return;
            }

            
            
            DataSet ds = new DataSet();
            string sql = 
            " Select tYear, LeaveTyp,Opn,Avl,(OPN-(AVL+ADV+ENC)) as Bal ,Adv,ENC from leaveBal " +
             " where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() + "'" +
             " and tYear ='" + txtYear.Text + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                grid.DataSource = ds;
                grid.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                grid.DataSource = null;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtYear.Text = gridView1.GetRowCellValue(info.RowHandle, "tYear").ToString();
                txtLeaveType.Text = gridView1.GetRowCellValue(info.RowHandle, "LeaveTyp").ToString();
                txtOpnBal.Text = gridView1.GetRowCellValue(info.RowHandle, "Opn").ToString();
                txtEncDay.Text = gridView1.GetRowCellValue(info.RowHandle, "ENC").ToString();
                object o = new object();
                EventArgs e = new EventArgs();                
                mode = "OLD";
                oldCode = txtYear.Text.ToString() + "," + txtLeaveType.Text.ToString();
                txtYear_Validated(o, e);
                
            }


        }

        private void txtLeaveType_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void frmLeaveBalEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }


    }
}
