using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace Attendance.Classes
{
    public class clsEmp
    {
        private string _EmpUnqID, _EmpName, _FatherName,
            _CompCode, _CompDesc,
            _WrkGrp, _WrkGrpDesc,
            _UnitCode, _UnitDesc,
            _DeptCode, _DeptDesc,
            _StatCode, _StatDesc,
            _EmpTypeCode, _EmpTypeDesc,
            _GradeCode, _GradeDesc,
            _DesgCode, _DesgDesc,
            _CatCode, _CatDesc,
            _ContCode, _ContDesc,
            _MessGrpCode, _MessGrpDesc,
            _MessCode, _MessDesc,
            _CostCode, _CostDesc,
            _ShiftCode,_ShiftDesc,
            _SAPID,_AdharNo, _EmpCode, _OLDEmpCode,_WeekOffDay;



        private DateTime? _JoinDt, _BirthDt;
        private DateTime? _ValidFrom, _ValidTo,_LeftDt;
        private bool _Active, _OTFLG, _Gender, _ContFlg, _PayrollFlg,_AutoShift,_IsNew ;


        public string EmpUnqID { get { return _EmpUnqID; } set { _EmpUnqID = value; } }
        public string EmpName { get { return _EmpName; } set { _EmpName = value; } }
        public string FatherName {get { return _FatherName; } set { _FatherName = value; }}
        
        public string CompCode {get { return _CompCode; } set { _CompCode = value; }}           
        public string CompDesc { get { return _CompDesc; } }
        
        public string WrkGrp { get { return _WrkGrp; } set { _WrkGrp = value; }}
        public string WrkGrpDesc { get { return _WrkGrpDesc; }}

        public string UnitCode { get { return _UnitCode; } set { _UnitCode = value; } }        
        public string UnitDesc { get { return _UnitDesc; }}    
        
        public string DeptCode { get { return _DeptCode; } set { _DeptCode = value; } }
        public string DeptDesc { get { return _DeptDesc; }}  

        public string StatCode { get { return _StatCode; } set { _StatCode = value; } }
        public string StatDesc { get { return _StatDesc; }}  

        public string EmpTypeCode { get { return _EmpTypeCode; } set { _EmpTypeCode = value; } }
        public string EmpTypeDesc { get { return _EmpTypeDesc; }}  

        public string GradeCode { get { return _GradeCode; } set { _GradeCode = value; } }
        public string GradeDesc { get { return _GradeDesc; }}  

        public string DesgCode { get { return _DesgCode; } set { _DesgCode = value; } }
        public string DesgDesc { get { return _DesgDesc; }}  
   
        public string CatCode { get { return _CatCode; } set { _CatCode = value; } }
        public string CatDesc { get { return _CatDesc; }}  
   
        public string ContCode { get { return _ContCode; } set { _ContCode = value; } }
        public string ContDesc { get { return _ContDesc; }}  
   
        public string MessGrpCode { get { return _MessGrpCode; } set { _MessGrpCode = value; } }
        public string MessGrpDesc { get { return _MessGrpDesc; }}  
    
        public string MessCode { get { return _MessCode; } set { _MessCode = value; } }
        public string MessDesc { get { return _MessDesc; }}  
    
        public string CostCode { get { return _CostCode; } set { _CostCode = value; } }
        public string CostDesc { get { return _CostDesc; }}  
    
        public string ShiftCode { get { return _ShiftCode; } set { _ShiftCode = value; } }
        public string ShiftDesc { get { return _ShiftDesc; }}  
    
        public string SAPID { get { return _SAPID; } set { _SAPID = value; } }
        public string AdharNo { get { return _AdharNo; } set { _AdharNo = value; } }
        public string EmpCode { get { return _EmpCode; } set { _EmpCode = value; } }   
        public string OLDEmpCode { get { return _OLDEmpCode; } set { _OLDEmpCode = value; } }   
        public string WeekOffDay { get { return _WeekOffDay; } set { _WeekOffDay = value; } }      
       

        public DateTime? JoinDt { get { return _JoinDt; } set { _JoinDt = value; } }
        public DateTime? BirthDt { get { return _BirthDt; } set { _BirthDt = value; } }
        public DateTime? ValidFrom { get { return _ValidFrom; } set { _ValidFrom = value; } }
        public DateTime? ValidTo { get { return _ValidTo; } set { _ValidTo = value; } }
        public DateTime? LeftDt { get { return _LeftDt; } set { _LeftDt = value; } }

        public bool Active { get { return _Active; } set { _Active = value; } }
        public bool OTFLG { get { return _OTFLG; } set { _OTFLG = value; } }
        public bool Gender { get { return _Gender; } set { _Gender = value; } }
        public bool ContFlg { get { return _ContFlg; } set { _ContFlg = value; } }
        public bool PayrollFlg { get { return _PayrollFlg; } set { _PayrollFlg = value; } }
        public bool AutoShift { get { return _AutoShift; } set { _AutoShift = value; } }

        public bool IsValid { 
            get 
            {

                if (IsNew)
                {
                    string err = this.BasicValidation();
                    if (string.IsNullOrEmpty(err))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return _Active; 
                } 
            } 
        }
        public bool IsNew { get { return _IsNew; } set { _IsNew = value; } }

        public clsEmp()
        {
            _EmpUnqID = string.Empty; _EmpName = string.Empty; _FatherName = string.Empty;
            _CompCode = string.Empty; _CompDesc = string.Empty;
            _WrkGrp = string.Empty; _WrkGrpDesc = string.Empty;
            _UnitCode = string.Empty; _UnitDesc = string.Empty;
            _DeptCode = string.Empty; _DeptDesc = string.Empty;
            _StatCode = string.Empty; _StatDesc = string.Empty;
            _EmpTypeCode = string.Empty; _EmpTypeDesc = string.Empty;
            _GradeCode = string.Empty; _GradeDesc = string.Empty;
            _DesgCode = string.Empty; _DesgDesc = string.Empty;
            _CatCode = string.Empty; _CatDesc = string.Empty;
            _ContCode = string.Empty; _ContDesc = string.Empty;
            _MessGrpCode = string.Empty; _MessGrpDesc = string.Empty;
            _MessCode = string.Empty; _MessDesc = string.Empty;
            _CostCode = string.Empty; _CostDesc = string.Empty;
            _ShiftCode = string.Empty;_ShiftDesc = string.Empty;
            _SAPID = string.Empty; _AdharNo = string.Empty; _EmpCode = string.Empty; _OLDEmpCode = string.Empty; _WeekOffDay = string.Empty;
            _IsNew = false;

            DateTime? dt = new DateTime?();
            _JoinDt = dt; _BirthDt = dt;
            _ValidFrom = dt; _ValidTo = dt;_LeftDt = dt;
            _Active = false; _OTFLG = false; _Gender = false; _ContFlg = false; _PayrollFlg = false; _AutoShift = false;

        }

        public bool GetEmpDetails(string tCompCode,string tEmpUnqID)
        {
            bool returnval = false;

            DataSet ds = new DataSet();
            string sql = "select * From MastEmp where CompCode ='" + tCompCode.Trim() 
                + "' and EmpUnqID ='" + tEmpUnqID.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _CostDesc = string.Empty;
            if (hasRows)
            {

                _IsNew = true;
                _Active = false;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    IsNew = false;
                    this.CompCode = dr["CompCode"].ToString();
                    this.EmpUnqID = dr["EmpUnqID"].ToString();
                    this.EmpName = dr["EmpName"].ToString();
                    this.FatherName = dr["FatherName"].ToString();
                    this.WrkGrp = dr["WrkGrp"].ToString();
                    this.UnitCode = dr["UnitCode"].ToString();
                    this.DeptCode = dr["DeptCode"].ToString();
                    this.StatCode = dr["StatCode"].ToString();
                    this.CatCode = dr["CatCode"].ToString();
                    this.DesgCode = dr["DesgCode"].ToString();
                    this.GradeCode = dr["GradCode"].ToString();
                    this.EmpTypeCode = dr["EmpTypeCode"].ToString();
                    this.ContCode = dr["ContCode"].ToString();
                    this.MessCode = dr["MessCode"].ToString();
                    this.MessGrpCode = dr["MessGrpCode"].ToString();
                    this.CostCode = dr["CostCode"].ToString();
                    this.ShiftCode = dr["ShiftCode"].ToString();

                    this.SAPID = dr["SAPID"].ToString();
                    this.EmpCode = dr["EmpCode"].ToString();
                    this.OLDEmpCode = dr["OldEmpCode"].ToString();
                    this.AdharNo = dr["AdharNo"].ToString();
                    this.WeekOffDay = dr["WeekOff"].ToString();

                    Gender = Convert.ToBoolean(dr["Sex"]);
                    Active = Convert.ToBoolean(dr["Active"]);
                    OTFLG = Convert.ToBoolean(dr["OTFLG"]);
                    ContFlg = Convert.ToBoolean(dr["ContractFlg"]);
                    PayrollFlg = Convert.ToBoolean(dr["PayrollFlg"]);
                    AutoShift = Convert.ToBoolean(dr["ShiftType"]);

                    ValidFrom = (dr["ValidFrom"] != DBNull.Value) ? Convert.ToDateTime(dr["ValidFrom"]): new DateTime?();
                    ValidTo = (dr["ValidTo"] != DBNull.Value) ? Convert.ToDateTime(dr["ValidTo"]) : new DateTime?();

                    JoinDt = (dr["JoinDt"] != DBNull.Value) ? Convert.ToDateTime(dr["JoinDt"]) : new DateTime?();
                    BirthDt = (dr["BirthDt"] != DBNull.Value) ? Convert.ToDateTime(dr["BirthDt"]) : new DateTime?();
                    LeftDt = (dr["LeftDt"] != DBNull.Value) ? Convert.ToDateTime(dr["LeftDt"]) : new DateTime?();

                    SetAllDesc();
                    returnval = true;
                }
            }

            return returnval;
        }

        private void SetAllDesc()
        {
            GetCompDesc(this.CompCode);
            GetWrkDesc(this.CompCode, this.WrkGrp);
            GetUnitDesc(this.CompCode, this.WrkGrp,this.UnitCode);
            GetDeptDesc(this.CompCode, this.WrkGrp, this.UnitCode,this.DeptCode);
            GetStatDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.DeptCode,this.StatCode);
            GetGradeDesc(this.CompCode, this.WrkGrp, this.GradeCode);
            GetDesgDesc(this.CompCode, this.WrkGrp, this.DesgCode);
            GetCatDesc(this.CompCode, this.WrkGrp, this.CatCode);
            GetEmpTypeDesc(this.CompCode, this.WrkGrp, this.EmpTypeCode);
            GetMessDesc(this.CompCode, this.UnitCode, this.MessCode);
            GetMessGrpDesc(this.CompCode, this.UnitCode, this.MessGrpCode);
            GetCostDesc(this.CostCode);
            GetContDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.ContCode);
        }
        
        public string BasicValidation()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(_EmpUnqID))
            {
                err += "EmpUnqID is Required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(_EmpName))
            {
                err += "EmpName is Required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(_FatherName))
            {
                err += "Father Name is Required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(_CompCode))
            {
                err += "CompCode is Required...." + Environment.NewLine;
            }
            else
            {
                GetCompDesc(_CompCode);

                if (string.IsNullOrEmpty(_CompDesc))
                {
                    err += "Invalid CompCode...." + Environment.NewLine;
                }
            }

            if (string.IsNullOrEmpty(_WrkGrp))
            {
                err += "WrkGrp is Required...." + Environment.NewLine;
            }
            else
            {
                GetWrkDesc(_CompCode,_WrkGrp);

                if (string.IsNullOrEmpty(_WrkGrpDesc))
                {
                    err += "Invalid WrkGrpCode...." + Environment.NewLine;
                }
            }

            if (string.IsNullOrEmpty(_UnitCode))
            {
                err += "UnitCode is Required...." + Environment.NewLine;
            }
            else
            {
                GetUnitDesc(_CompCode, _WrkGrp,_UnitCode);

                if (string.IsNullOrEmpty(_UnitDesc))
                {
                    err += "Invalid UnitCode...." + Environment.NewLine;
                }
            }



            if (_JoinDt.HasValue == false)
            {
                err += "Join Date is Required...." + Environment.NewLine;
            }
            if (_BirthDt.HasValue == false)
            {
                err += "Birth Date is Required...." + Environment.NewLine;
            }

            if (_ValidFrom.HasValue == false)
            {
                err += "Valid From is Required...." + Environment.NewLine;
            }

            if (_ValidTo.HasValue == false)
            {
                err += "Valid To is Required...." + Environment.NewLine;
            }

            if (_ValidFrom.HasValue && _ValidTo.HasValue)
            {
                DateTime tFrom, tTo;
                
                tFrom = _ValidFrom.Value;
                tTo = _ValidTo.Value;

                if (tFrom > tTo)
                {
                    err += "ValidFrom Must be Less than Valid To ...." + Environment.NewLine;
                }
            }
            
            if (_BirthDt.HasValue && _JoinDt.HasValue)
            {
                DateTime tJoin, tBirth;

                tJoin = _JoinDt.Value;
                tBirth = _BirthDt.Value;

                if (tJoin < tBirth)
                {
                    err += "Join Date Must be Greator than Birth Date ...." + Environment.NewLine;
                }
            }


            if (string.IsNullOrEmpty(_AdharNo))
            {
                err += "Adhar No is Required...." + Environment.NewLine;
            }

            return err;
        }

        private void GetCompDesc(string tCompCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastComp where CompCode ='" + tCompCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _CompDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._CompDesc = dr["CompName"].ToString();

                }
            }
        }
               
        private void GetWrkDesc(string tCompCode, string tWrkGrp)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);
            
            _WrkGrpDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {   
                    this._WrkGrpDesc = dr["WrkGrpDesc"].ToString();
                    
                }
            }
        }

        private void GetUnitDesc(string tCompCode, string tWrkGrp,string tUnitCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastUnit where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim() + "' and UnitCode = '" + tUnitCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _UnitDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._UnitDesc = dr["UnitName"].ToString();

                }
            }
        }

        private void GetDeptDesc(string tCompCode, string tWrkGrp,string tUnitCode,string tDeptCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastDept where CompCode ='" + tCompCode.Trim() 
                + "' and WrkGrp='" + tWrkGrp.Trim() + "' and UnitCode = '" + tUnitCode.Trim() 
                + "' and DeptCode='" + tDeptCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _DeptDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._DeptDesc = dr["DeptDesc"].ToString();

                }
            }
        }

        private void GetStatDesc(string tCompCode, string tWrkGrp, string tUnitCode, string tDeptCode, string tStatCode)
        {

            DataSet ds = new DataSet();
            string sql = "select * From MastStat where CompCode ='" + tCompCode.Trim()
                + "' and WrkGrp='" + tWrkGrp.Trim() + "' and UnitCode = '" + tUnitCode.Trim()
                + "' and DeptCode='" + tDeptCode.Trim() 
                + "' and StatCode='" + tStatCode.Trim() + "'" ;

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _StatDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._StatDesc = dr["StatDesc"].ToString();

                }
            }
        
        }

        private void GetGradeDesc(string tCompCode, string tWrkGrp, string tGradeCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastGrade where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim()
                + "' and GradeCode = '" + tGradeCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _GradeDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._GradeDesc = dr["GradeDesc"].ToString();

                }
            }
        
        }

        private void GetDesgDesc(string tCompCode, string tWrkGrp, string tDesgCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastDesg where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim()
                + "' and DesgCode = '" + tDesgCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _DesgDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._DesgDesc = dr["DesgDesc"].ToString();

                }
            }
        
        
        }

        private void GetCatDesc(string tCompCode, string tWrkGrp, string tCatCode)
        {

            DataSet ds = new DataSet();
            string sql = "select * From MastCat where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim()
                + "' and CatCode = '" + tCatCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _CatDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._CatDesc = dr["CatDesc"].ToString();

                }
            }
        
        }

        private void GetEmpTypeDesc(string tCompCode, string tWrkGrp, string tEmpTypeCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastEmpType where CompCode ='" + tCompCode.Trim() + "' and WrkGrp='" + tWrkGrp.Trim()
                + "' and EmpTypeCode = '" + tEmpTypeCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _EmpTypeDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._EmpTypeDesc = dr["EmpTypeDesc"].ToString();

                }
            }
        
        
        }

        private void GetMessDesc(string tCompCode, string tUnitCode, string tMessCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastMess where CompCode ='" + tCompCode.Trim() + "' and UnitCode='" + tUnitCode.Trim()
                + "' and MessCode = '" + tMessCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _MessDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._MessDesc = dr["MessDesc"].ToString();

                }
            }
        
        }

        private void GetMessGrpDesc(string tCompCode, string tUnitCode, string tMessGrpCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastMessGrp where CompCode ='" + tCompCode.Trim() + "' and UnitCode='" + tUnitCode.Trim()
                + "' and MessGrpCode = '" + tMessGrpCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _MessGrpDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._MessGrpDesc = dr["MessGrpDesc"].ToString();

                }
            }
        
        }
        
        public void GetCostDesc(string tCostCode)
        {

            DataSet ds = new DataSet();
            string sql = "select * From MastCostCode where CostCode ='" + tCostCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _CostDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._CostDesc = dr["CostDesc"].ToString();

                }
            }
        
        }

        private void GetContDesc(string tCompCode, string tWrkGrp, string tUnitCode, string tContCode)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastCont where CompCode ='" + tCompCode.Trim()
                + "' and WrkGrp='" + tWrkGrp.Trim() + "' and UnitCode = '" + tUnitCode.Trim()
                + "' and ContCode='" + tContCode.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            _ContDesc = string.Empty;
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this._ContDesc = dr["ContName"].ToString();

                }
            }
        }
    }
}
