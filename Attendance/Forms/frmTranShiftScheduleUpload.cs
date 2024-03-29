﻿using System;
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
 

    public partial class frmTranShiftScheduleUpload : DevExpress.XtraEditors.XtraForm
    {
        public string GRights = "XXXV";
        
        private int GFormID = 0;

        private class LeaveData
        {
            public DateTime FromDt;
            public DateTime ToDt;
            public string LeaveTyp;

            public LeaveData()
            {
                FromDt = new DateTime();
                ToDt = new DateTime();
                LeaveTyp = "";
            }

        }

        DataTable dt = new DataTable();

        public frmTranShiftScheduleUpload()
        {
            InitializeComponent();
            
        }

        
        private void btnBrowse_Click(object sender, EventArgs e)
        {

            if (txtYearMT.DateTime == null || txtYearMT.DateTime == DateTime.MinValue)
            {
                MessageBox.Show("Please Select Year Month First...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBrowse.Text = "";
                return;
            }

            
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
            t.CompCode = "01";
            t.EmpUnqID = tdr["EmpUnqID"].ToString();
            if (!t.GetEmpDetails(t.EmpUnqID))
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }

            return err;
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            txtYearMT.Enabled = false;
            btnBrowse.Enabled = false;
            btnImport.Enabled = false;
            btnPreview.Enabled = false;

            if (grd_view1.DataRowCount <= 0)
            {
                MessageBox.Show("No Rows to Upload", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //added on 2020-10-01
            int sheduleymt = Convert.ToInt32(txtYearMT.DateTime.ToString("yyyyMM"));
            int curymt = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

            DateTime LastWODt = new DateTime();


            if(sheduleymt >= curymt)
            {
                if((sheduleymt - curymt) >= 2)
                {
                    MessageBox.Show("future month shedule will not be allowed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }

            if(sheduleymt < curymt)
            {
                MessageBox.Show("Previous month shedule would not be allowed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ///

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

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                   
                    DateTime StartDt = new DateTime(), EndDt = new DateTime();
                    

                    string sql = string.Empty;
                    string tYearMt = txtYearMT.DateTime.ToString("yyyyMM");
                    string stdt = Utils.Helper.GetDescription("Select CAL_MthStart From V_CAL0815 where Cal_Date ='" + txtYearMT.DateTime.ToString("yyyy-MM-dd") + "'", Utils.Helper.constr);
                    string endt = Utils.Helper.GetDescription("Select CAL_MthEnd From V_CAL0815 where Cal_Date ='" + txtYearMT.DateTime.ToString("yyyy-MM-dd") + "'", Utils.Helper.constr);


                    StartDt = Convert.ToDateTime(stdt);
                    EndDt = Convert.ToDateTime(endt);
                    

                    foreach (DataRow dr in sortedDT.Rows)
                    {

                        if (cn.State == ConnectionState.Open)
                        {
                            cn.Close();
                        }

                        bool ProcessFlg = chkProcessFlg.Checked;

                        cn.Open();

                        bool brkflg = false;
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        string tCompCode = Utils.Helper.GetDescription("Select CompCode From MastEmp where EmpUnqID ='" + tEmpUnqID + "'", Utils.Helper.constr);
                        clsEmp Emp = new clsEmp();
                        Emp.GetEmpDetails(tEmpUnqID);
                        if (!Emp.Active)
                        {
                            dr["Remarks"] = "Invalid/InActive Employee..";
                            continue;
                        }

                        //if (!Globals.GetWrkGrpRights(GFormID, Emp.WrkGrp, Emp.EmpUnqID))
                        //{
                        //    dr["Remarks"] = "you are not authorised..";
                        //    continue;
                        //}
                        List<int> woidx = new List<int>();

                        #region Chk_ValidShift
                        
                        for (int i = 1; i <= EndDt.Day; i++)
                        {
                            string fldnm = "D" + i.ToString("00");
                            string fldval = dr[fldnm].ToString().Trim().ToUpper();

                            if(fldval != "WO")
                            {
                                if (!Globals.G_ShiftList.Contains(fldval))
                                {
                                    dr["Remarks"] = dr["Remarks"].ToString() + " Invalid Shift..";
                                    brkflg = true;
                                    break;
                                    
                                }
                            }
                            else
                            {
                                woidx.Add(i);
                            }

                            if (fldval.Trim() == string.Empty)
                            {
                                dr["Remarks"] = dr["Remarks"].ToString() + " Invalid Shift..";
                                brkflg = true;
                                break;
                            }
                        }

                        if(woidx.Count == 0){
                            dr["Remarks"] = dr["Remarks"].ToString() + " No Week of has been defined..";
                            brkflg = true;                            
                        }

                        if (brkflg)
                        {
                            continue;
                        }


                        #region Check_WO_Order

                        //int firstWOon = woidx.First();

                        //int tchk = 0;
                        //for (int i = 1; i <= woidx.Count - 1; i++)
                        //{
                        //    tchk = (7 * i) + firstWOon;
                        //    if(woidx.ElementAt(i) != tchk)
                        //    {
                        //        dr["Remarks"] = dr["Remarks"].ToString() + string.Format("{0} WeekOff is not in correct order..",i);
                        //        //brkflg = true;
                        //        break;
                        //    }
                        //}

                        //if (brkflg)
                        //{
                        //    continue;
                        //}
                        #endregion
                        
                        #endregion

                        SqlTransaction trn = cn.BeginTransaction("ShiftSch-" + Emp.EmpUnqID);
                        SqlCommand cmd = new SqlCommand();
                        

                        #region Chk_ShiftSchedule_Rec
                        try
                        {
                            int cnt = Convert.ToInt32(Utils.Helper.GetDescription("Select Count(*) from MastShiftSchedule where EmpUnqID = '" + tEmpUnqID + "' And YearMt ='" + tYearMt + "'", Utils.Helper.constr));
                            if (cnt == 0)
                            {
                                sql = "Insert into MastShiftSchedule (YearMt,EmpUnqID,AddDt,AddId) Values ('" + tYearMt + "','" + tEmpUnqID + "',GetDate(),'" + Utils.User.GUserID + "')";
                                cmd = new SqlCommand(sql,cn,trn);
                                cmd.ExecuteNonQuery();

                            }
                            else
                            {

                                sql = "insert into MastShiftScheduleHistory ( [YearMt],[EmpUnqID], " +
                                        " [D01],[D02],[D03],[D04],[D05],[D06],[D07],[D08],[D09],[D10]," +
                                        " [D11],[D12],[D13],[D14],[D15],[D16],[D17],[D18],[D19],[D20],[D21], " +
                                        " [D22],[D23],[D24],[D25],[D26],[D27],[D28],[D29],[D30],[D31],[ADDID], " +
                                        " [ADDDT],[UPDID],[UPDDT]) " +
                                        " Select  " +
                                        " [YearMt],[EmpUnqID]," +
                                        " [D01],[D02],[D03],[D04],[D05],[D06],[D07],[D08],[D09],[D10]," +
                                        " [D11],[D12],[D13],[D14],[D15],[D16],[D17],[D18],[D19],[D20],[D21], " +
                                        " [D22],[D23],[D24],[D25],[D26],[D27],[D28],[D29],[D30],[D31],[ADDID]," +
                                        " [ADDDT],[UPDID],[UPDDT] from MastShiftSchedule " +
                                        " where YearMt = '" + tYearMt + "' " +
                                        " and EmpUnqID = '" + tEmpUnqID + "' ";

                                cmd = new SqlCommand(sql, cn, trn);
                                cmd.ExecuteNonQuery();

                                sql = "Update MastShiftSchedule Set UpdDt = GetDate() , UpdID = '" + Utils.User.GUserID + "' Where YearMt = '" + tYearMt + "' And EmpUnqID ='" + tEmpUnqID + "'";
                                cmd = new SqlCommand(sql, cn, trn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            brkflg = true;
                            dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                        }
                        
                        if (brkflg)
                        {
                            trn.Rollback();
                            cn.Close();
                            trn.Dispose();
                            continue;
                        }

                        #endregion
                        
                        #region Get_IFAnyLeavePosted

                        sql = "Select * from LeaveEntry Where " +
                           " compcode = '" + Emp.CompCode + "'" +
                          // " and WrkGrp ='" + Emp.WrkGrp + "'" +
                           " And tYear ='" + tYearMt.Substring(0,4) + "'" +
                           " And EmpUnqID='" + tEmpUnqID + "'" +
                           " And (     FromDt between '" + StartDt.ToString("yyyy-MM-dd") + "'   And '" + EndDt.ToString("yyyy-MM-dd") + "' " +
                           "  OR       ToDt Between '" + StartDt.ToString("yyyy-MM-dd") + "'   And '" + EndDt.ToString("yyyy-MM-dd") + "' " +
                           "     ) Order by FromDt ";

                        DataSet dsLeave = Utils.Helper.GetData(sql, Utils.Helper.constr);
                        List<LeaveData> leave = new List<LeaveData>();
                        bool hasRows = dsLeave.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if(hasRows)
                        {
                            
                            foreach(DataRow drl in dsLeave.Tables[0].Rows)
                            {
                                LeaveData t = new LeaveData();
                                t.FromDt = Convert.ToDateTime(drl["FromDt"]);
                                t.ToDt = ((Convert.ToDateTime(drl["ToDt"]) > EndDt) ? EndDt : Convert.ToDateTime(drl["ToDt"]));
                                t.LeaveTyp = drl["LeaveTyp"].ToString();
                                leave.Add(t);
                               
                            }
                        }
                        #endregion
                        
                        #region Upd_ShiftSchedule_AttdData
                        
                        string ShiftSql = string.Empty;

                        for(DateTime date = StartDt; date.Date <= EndDt.Date; date = date.AddDays(1))
                        {
                            string fldnm = "D" + date.ToString("dd");
                            string fldval = dr[fldnm].ToString().Trim().ToUpper();

                            if (fldval == "WO")
                                LastWODt = date;

                            try
                            {
                                string sqlattd = "Update AttdData " +
                                     " Set ScheduleShift ='" + fldval + "' Where tYear = '" + date.Year.ToString() + "' And tDate ='" + date.ToString("yyyy-MM-dd") + "' " +
                                    " And CompCode = '" + Emp.CompCode + "' And WrkGrp ='" + Emp.WrkGrp + "' And EmpUnqID='" + Emp.EmpUnqID + "'";

                                cmd = new SqlCommand(sqlattd,cn,trn);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                
                                brkflg = true;
                                dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                break;
                            }

                            
                            ShiftSql += fldnm + " = '" + fldval + "',";
                        } //end foreach date

                        if (brkflg)
                        {
                            trn.Rollback();
                            cn.Close();
                            trn.Dispose();
                            continue;
                        }

                        try
                        {
                            //update shiftschedule
                            sql = "Update MastShiftSchedule Set " + ShiftSql.Substring(0, ShiftSql.Length - 1) + " Where Yearmt = '" + tYearMt + "' and EmpUnqId ='" + Emp.EmpUnqID + "' ";
                            cmd = new SqlCommand(sql,cn,trn);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            brkflg = true;                            
                            dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                        }
                        if (brkflg)
                        {
                            trn.Rollback();
                            cn.Close();
                            trn.Dispose();
                            continue;
                        }

                        #endregion

                        #region chk_LeavePosted
                        
                        if (leave.Count == 0)
                        {
                            #region ifnoleavefound

                            //delete schedule wo
                            string delsql = "Delete From MastLeaveSchedule Where tYear ='" + StartDt.Year.ToString() + "' " +
                             " And tYearMT='" + tYearMt + "' " +
                             " And EmpUnqID ='" + Emp.EmpUnqID + "' And SchLeave='WO' ";

                            try
                            {
                                cmd = new SqlCommand(delsql,cn,trn);                              
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                brkflg = true;
                            }

                            if (brkflg)
                            {
                                trn.Rollback();
                                cn.Close();
                                trn.Dispose();
                                continue;
                            }

                            //'Insert "WO" MastLeaveSchedule Table
                            for (DateTime date = StartDt; date.Date <= EndDt.Date; date = date.AddDays(1))
                            {
                                string fldnm = "D" + date.ToString("dd");
                                string fldval = dr[fldnm].ToString().Trim().ToUpper();

                                if (fldval == "WO")
                                {
                                    LastWODt = date;
                                    
                                    string inssql = " Insert into MastLeaveSchedule " +
                                    " ( EmpUnqID,WrkGrp,tDate,SchLeave,Adddt,AddId )" +
                                    " Values ('" + Emp.EmpUnqID + "','" + Emp.WrkGrp + "','" + date.ToString("yyyy-MM-dd") + "','WO',GetDate(),'ShiftSch')";

                                    try
                                    {
                                        cmd = new SqlCommand(inssql,cn,trn);
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                        brkflg = true;
                                        break;
                                    }
                                    
                                }
                            }

                            if (brkflg)
                            {
                                trn.Rollback();
                                cn.Close();
                                trn.Dispose();
                                continue;
                            }
                            #endregion
                        }
                        else
                        {
                            #region ifleavefound

                            DateTime LastWO = new DateTime();
                            for (DateTime date = StartDt; date.Date <= EndDt.Date; date = date.AddDays(1))
                            {
                                string fldnm = "D" + date.ToString("dd");
                                string fldval = dr[fldnm].ToString().Trim().ToUpper();
                                
                                bool LeavPos = false;
                                if (fldval == "WO")
                                {
                                    LastWODt = date;
                                    foreach (LeaveData t in leave)
                                    {
                                        //check if date is fall between leave posted
                                        if (date >= t.FromDt && date <= t.ToDt)
                                        {
                                            LeavPos = true;
                                            
                                            date = t.ToDt;
                                            LastWO = t.ToDt;

                                            
                                            break;                                            
                                        }
                                    }

                                    if (LeavPos)
                                    {
                                        continue;
                                    }

                                    //'if there is no leave posted on tdate
                                    if (!LeavPos)
                                    {
                                        if (LastWO != DateTime.MinValue)
                                        {
                                            try
                                            {
                                                string delwo = "Delete from MastLeaveSchedule Where EmpUnqID = '" + Emp.EmpUnqID + "' "
                                                + " and tDate > '" + LastWO.ToString("yyyy-MM-dd") + "' and tDate <= '" + date.ToString("yyyy-MM-dd") + "' and SchLeave ='WO'";
                                                LastWO = date;

                                                cmd = new SqlCommand(delwo,cn,trn);
                                                cmd.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                                brkflg = true;
                                                break;
                                            }
                                            
                                            
                                        }
                                        else
                                        {
                                            try
                                            {
                                                string delwo = "Delete from MastLeaveSchedule Where EmpUnqID = '" + Emp.EmpUnqID + "' and tDate = '" + date.ToString("yyyy-MM-dd") + "' and SchLeave ='WO'";
                                                cmd = new SqlCommand(delwo,cn,trn);
                                                cmd.ExecuteNonQuery();
                                            }                                            
                                            catch (Exception ex)
                                            {
                                                dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                                brkflg = true;
                                                break;
                                            }
                                        }


                                        try
                                        {
                                            string inssql = " Insert into MastLeaveSchedule ( EmpUnqID,WrkGrp,tDate,SchLeave,Adddt,AddId )" +
                                            " Values ('" + Emp.EmpUnqID + "','" + Emp.WrkGrp + "','" + date.ToString("yyyy-MM-dd") + "','WO',GetDate(),'ShiftSch')";

                                            cmd = new SqlCommand(inssql,cn,trn);
                                            cmd.ExecuteNonQuery();
                                        
                                        }catch(Exception ex)
                                        {
                                            dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                            brkflg = true;
                                            break;
                                        }

                                    }


                                }//end if WO
                                else
                                {

                                    LeavPos = false;
                                    foreach (LeaveData t in leave)
                                    {
                                        //check if date is fall between leave posted
                                        if (date >= t.FromDt && date <= t.ToDt)
                                        {
                                            LeavPos = true;
                                            break;
                                        }
                                    }

                                    if (!LeavPos)
                                    {
                                        try
                                        {
                                            sql = "Delete from MastLeaveSchedule Where EmpUnqID = '" + Emp.EmpUnqID + "' " +
                                                        " and tDate = '" + date.ToString("yyyy-MM-dd") + "' and SchLeave ='WO'";

                                            cmd = new SqlCommand(sql, cn, trn);
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                            brkflg = true;
                                            break;
                                        }

                                    }

                                    
                                }

                            } //end of eachday

                            if (brkflg)
                            {
                                trn.Rollback();
                                cn.Close();
                                trn.Dispose();
                                continue;
                            }

                            //remove any extra "WO" if
                            try
                            {
                                if (LastWO != DateTime.MinValue)
                                {
                                    sql = "Delete from MastLeaveSchedule Where EmpUnqID = '" + Emp.EmpUnqID + "' and tDate > '" + LastWO.ToString("yyyy-MM-dd") + "' " +
                                        " and tDate <= '" + EndDt.ToString("yyyy-MM-dd") + "' and SchLeave ='WO'";
                                    cmd = new SqlCommand(sql,cn,trn);                                    
                                    cmd.ExecuteNonQuery();

                                }
                            }                            
                            catch (Exception ex)
                            {
                                dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                                brkflg = true;
                                break;
                            }

                            #endregion
                        } //end if leavefound

                        if (brkflg)
                        {
                            trn.Rollback();
                            cn.Close();
                            trn.Dispose();
                            continue;
                        }

                        #endregion

                        try
                        {
                           

                            //new process : adjust wo from next month
                            //and change default wo on empmaster
                            // and re-create muster table from next month
                            //added on 2021-08-31
                            if (LastWODt != DateTime.MinValue)
                            {
                                cmd.Transaction = trn;

                                //cmd.CommandText = "Update MastEmp Set WeekOff ='" +LastWODt.ToString("ddd").ToUpper() + "' Where EmpUnqID ='" + tEmpUnqID + "'";
                                //cmd.ExecuteNonQuery();

                                if(Convert.ToInt32(EndDt.ToString("yyyy")) != Convert.ToInt32(EndDt.AddDays(1).ToString("yyyy")))
                                {
                                    goto SkipNextYear;
                                }

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "CreateMuster";

                                SqlParameter spout = new SqlParameter();
                                spout.Direction = ParameterDirection.Output;
                                spout.DbType = DbType.Int32;
                                spout.ParameterName = "@result";
                                int tout = 0;
                                spout.Value = tout;
                                cmd.Parameters.AddWithValue("@pWrkGrp", Emp.WrkGrp);
                                cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                                cmd.Parameters.AddWithValue("@pFromDt", EndDt.AddDays(1).ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@pToDt", EndDt.AddDays(1).ToString("yyyy") + "-12-31");
                                cmd.Parameters.AddWithValue("@pWoDay", LastWODt.ToString("ddd"));
                                cmd.Parameters.Add(spout);
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();
                                //get the output
                                int result = (int)cmd.Parameters["@result"].Value;

                            }
SkipNextYear:
                            trn.Commit();
                            dr["Remarks"] = "Uploded";

                            //process data
                            if (ProcessFlg)
                            {
                                clsProcess pro = new clsProcess();
                                int result = 0;
                                string proerr = string.Empty;
                                pro.AttdProcess(Emp.EmpUnqID, StartDt, EndDt, out result, out proerr);
                                dr["remarks"] = dr["remarks"].ToString() + "Record updated...";
                                
                                //if (result > 0)
                                //{
                                //    pro.LunchInOutProcess(Emp.EmpUnqID, StartDt, EndDt, out result);
                                //    //dr["remarks"] = dr["remarks"].ToString() + "Record updated...";
                                //}
                            }                            

                        }
                        catch (Exception ex)
                        {
                            trn.Rollback();
                            
                            dr["Remarks"] = dr["Remarks"].ToString() + Environment.NewLine + ex.ToString();
                            brkflg = true;                            
                        }
                        trn.Dispose();
                        cn.Close();
                        
                    }//using foreach of all employee

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

            txtYearMT.Enabled = true;
            btnBrowse.Enabled = true;
            btnPreview.Enabled = true;

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (txtYearMT.DateTime == null || txtYearMT.DateTime == DateTime.MinValue)
            {
                MessageBox.Show("Please Select Year Month First...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBrowse.Text = "";
                return;
            }


            
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

            grd_view1.Columns.Clear();
           
            

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
                string myexceldataquery = BuildExcelSelect(txtYearMT.DateTime);
                myexceldataquery += " From " + sheetname;
               
                
                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt = new DataTable();
                dt.Clear();
               
                oledbda.Fill(dt);
                dt.AcceptChanges();
                foreach (DataRow row in dt.Rows)
                {
                    if(string.IsNullOrEmpty(row["EmpUnqID"].ToString().Trim()))
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
            grd_view.Refresh();

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }
            
            btnBrowse.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private string BuildExcelSelect(DateTime tDate)
        {
            string sql = string.Empty;
            string YearMt = string.Empty;

            if(tDate == DateTime.MinValue){
                return sql;
            }
            string tempsql = "Select Top  31 CAL_YearMth as YearMt, CAL_Date as tDate from V_CAL0815 where CAL_YearMth = '" + tDate.ToString("yyyyMM") + "' Order By Cal_Date ASC ";
            DataSet ds = Utils.Helper.GetData(tempsql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                sql = "Select EmpUnqID,";
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    sql += "D" + Convert.ToDateTime(dr["tDate"]).ToString("dd") + ",";
                }
            }
            sql += " '' as  Remarks ";

            return sql;
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

        private void frmUploadShiftSchedule_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            GFormID = Convert.ToInt32(Utils.Helper.GetDescription("Select FormId from MastFrm Where FormName ='" + this.Name + "'",Utils.Helper.constr));
            grd_view.DataSource = null;
            btnImport.Enabled = false;


        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtBrowse.Text = "";
            btnBrowse.Enabled = true;
            btnImport.Enabled = false;
            btnPreview.Enabled = true;
            grd_view.DataSource = null;
           
            dt = dt = new DataTable();
            grd_view.DataSource = dt;

            grd_view1.Columns.Clear();
            grd_view.Refresh();
        }

        private void txtYearMT_EditValueChanged(object sender, EventArgs e)
        {
            if (txtYearMT.DateTime == null || txtYearMT.DateTime == DateTime.MinValue)
            {
                return;
            }


            if(txtYearMT.DateTime.ToString("yyyyMM") != Convert.ToDateTime(txtYearMT.OldEditValue).ToString("yyyyMM"))
            {
                //reset
                dt = dt = new DataTable();
                grd_view.DataSource = dt;

                grd_view1.Columns.Clear();
                grd_view.Refresh();
                btnPreview.Enabled = true;
                btnImport.Enabled = false;

            }           
        }

        

    }
}