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
        private string _EmpUnqID, _EmpName, _FatherName, _Gender,
            _CompCode, _CompDesc,
            _WrkGrp, _WrkGrpDesc,
            _UnitCode, _UnitDesc,
            _DeptCode, _DeptDesc,
            _StatCode, _StatDesc,
            _GradeCode, _GradeDesc,
            _DesgCode, _DesgDesc,
            _CatCode, _CatDesc,
            _ContCode, _ContDesc,
            _CostCode, _CostDesc,
            _WeekOffDay;



        private DateTime? _JoinDt, _BirthDt;
        private DateTime? _ProfileValidFrom, _ValidTo,_LeftDt;
        private bool _IsHOD, _Active, _OTFLG,  _IsNew;


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

        

        public string GradeCode { get { return _GradeCode; } set { _GradeCode = value; } }
        public string GradeDesc { get { return _GradeDesc; }}  

        public string DesgCode { get { return _DesgCode; } set { _DesgCode = value; } }
        public string DesgDesc { get { return _DesgDesc; }}  
   
        public string CatCode { get { return _CatCode; } set { _CatCode = value; } }
        public string CatDesc { get { return _CatDesc; }}  
   
        public string ContCode { get { return _ContCode; } set { _ContCode = value; } }
        public string ContDesc { get { return _ContDesc; }}  
   
       
    
        public string CostCode { get { return _CostCode; } set { _CostCode = value; } }
        public string CostDesc { get { return _CostDesc; }}  
    
        
        public string WeekOffDay { get { return _WeekOffDay; } set { _WeekOffDay = value; } }      
       
        public DateTime? ProfileValidFrom { get { return _ProfileValidFrom; } set { _ProfileValidFrom = value; } }
        public DateTime? JoinDt { get { return _JoinDt; } set { _JoinDt = value; } }
        public DateTime? BirthDt { get { return _BirthDt; } set { _BirthDt = value; } }
        
        public DateTime? LeftDt { get { return _LeftDt; } set { _LeftDt = value; } }

        public bool Active { get { return _Active; } set { _Active = value; } }
        public bool OTFLG { get { return _OTFLG; } set { _OTFLG = value; } }
        public string Gender { get { return _Gender; } set { _Gender = value; } }
      
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string CardNo { get; set; }


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
            _Gender = string.Empty;
            _GradeCode = string.Empty; _GradeDesc = string.Empty;
            _DesgCode = string.Empty; _DesgDesc = string.Empty;
            _CatCode = string.Empty; _CatDesc = string.Empty;
            _ContCode = string.Empty; _ContDesc = string.Empty;
           
            _CostCode = string.Empty; _CostDesc = string.Empty;
            _WeekOffDay = string.Empty;
            _IsNew = false;
            _IsHOD = false;

            DateTime? dt = new DateTime?();
            _JoinDt = dt; _BirthDt = dt;
            _Active = false; _OTFLG = false; 
            
        }

        public bool GetEmpDetails(string tEmpUnqID,DateTime tDate)
        {
            bool returnval = false;

            DataSet ds = new DataSet();
            string sql = "select * From MastEmp where EmpUnqID ='" + tEmpUnqID.Trim() + "'";

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

                    this.Gender = dr["Gender"].ToString();
                    this.Active = Convert.ToBoolean(dr["Active"]);
                    this.CardNo = dr["CardNo"].ToString();
                    this.ContactNo = dr["ContactNo"].ToString();
                    this.Email = dr["EmailId"].ToString();
                    string err = string.Empty;

                    
                    DataSet dts = Utils.Helper.GetData("select * from dbo.fn_EmpMast('" + tEmpUnqID + "','" + tDate.ToString("yyyy-MM-dd") + "')", Utils.Helper.constr, out err);
                    hasRows = dts.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (hasRows)
                    {
                        DataRow tdr = dts.Tables[0].Rows[0];
                        this.OTFLG = Convert.ToBoolean(tdr["OTFLG"]);
                    }
                    this.JoinDt = (dr["JoinDt"] != DBNull.Value) ? Convert.ToDateTime(dr["JoinDt"]) : new DateTime?();
                    this.BirthDt = (dr["BirthDt"] != DBNull.Value) ? Convert.ToDateTime(dr["BirthDt"]) : new DateTime?();
                    this.LeftDt = (dr["LeftDt"] != DBNull.Value) ? Convert.ToDateTime(dr["LeftDt"]) : new DateTime?();
                    GetWrkDesc(this.CompCode, this.WrkGrp);
                    SetAllDesc();
                    returnval = true;
                }
            }

            return returnval;
        }


        public bool GetEmpDetails(string tEmpUnqID)
        {
            bool returnval = false;

            DataSet ds = new DataSet();
            string sql = "select * From MastEmp where EmpUnqID ='" + tEmpUnqID.Trim() + "'";

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

                    this.Gender = dr["Gender"].ToString();
                    this.Active = Convert.ToBoolean(dr["Active"]);
                    this.CardNo = dr["CardNo"].ToString();
                    this.ContactNo = dr["ContactNo"].ToString();
                    this.Email = dr["EmailId"].ToString();
                    
                    
            
                    
                    this.JoinDt = (dr["JoinDt"] != DBNull.Value) ? Convert.ToDateTime(dr["JoinDt"]) : new DateTime?();
                    this.BirthDt = (dr["BirthDt"] != DBNull.Value) ? Convert.ToDateTime(dr["BirthDt"]) : new DateTime?();
                    this.LeftDt = (dr["LeftDt"] != DBNull.Value) ? Convert.ToDateTime(dr["LeftDt"]) : new DateTime?();
                    GetWrkDesc(this.CompCode, this.WrkGrp);
                    SetAllDesc();
                    returnval = true;
                }
            }

            return returnval;
        }

        private void SetAllDesc()
        {
            string sql = "Select top 1 * from MastEmpJobProfile where EmpUnqId='" + this.EmpUnqID + "' and ValidFrom <=getdate() order by ValidFrom Desc";
            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                this.UnitCode = dr["UnitCode"].ToString();
                this.DeptCode = dr["DeptCode"].ToString();
                this.StatCode = dr["StatCode"].ToString();
                this.CatCode = dr["CatCode"].ToString();
                this.DesgCode = dr["DesgCode"].ToString();
                this.GradeCode = dr["GradeCode"].ToString();
                this.ContCode = dr["ContCode"].ToString();
                this.CostCode = dr["CostCode"].ToString();
                this.WeekOffDay = dr["WeekOff"].ToString();
                this.OTFLG = Convert.ToBoolean(dr["OTFLG"]);

                this.ProfileValidFrom = Convert.ToDateTime(dr["ValidFrom"]);

                GetCompDesc(this.CompCode);
                GetWrkDesc(this.CompCode, this.WrkGrp);
                GetUnitDesc(this.CompCode, this.WrkGrp, this.UnitCode);
                GetDeptDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.DeptCode);
                GetStatDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.DeptCode, this.StatCode);
                GetGradeDesc(this.CompCode, this.WrkGrp, this.GradeCode);
                GetDesgDesc(this.CompCode, this.WrkGrp, this.DesgCode);
                GetCatDesc(this.CompCode, this.WrkGrp, this.CatCode);

                GetCostDesc(this.CostCode);
                GetContDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.ContCode);
            }


            
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

            



            if (_JoinDt.HasValue == false)
            {
                err += "Join Date is Required...." + Environment.NewLine;
            }
            if (_BirthDt.HasValue == false)
            {
                err += "Birth Date is Required...." + Environment.NewLine;
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


            
            
            return err;
        }

        private void GetCompDesc(string tCompCode)
        {
             Utils.MastCodeValidate.GetCompDesc(tCompCode,out _CompDesc);            
        }
               
        private void GetWrkDesc(string tCompCode, string tWrkGrp)
        {
            Utils.MastCodeValidate.GetWrkGrpDesc(tCompCode,tWrkGrp, out _WrkGrpDesc);            
        }

        private void GetUnitDesc(string tCompCode, string tWrkGrp,string tUnitCode)
        {
            Utils.MastCodeValidate.GetUnitDesc(tCompCode, tWrkGrp, tUnitCode ,out _UnitDesc);

        }

        private void GetDeptDesc(string tCompCode, string tWrkGrp,string tUnitCode,string tDeptCode)
        {
            Utils.MastCodeValidate.GetDeptDesc(tCompCode, tWrkGrp, tUnitCode,tDeptCode, out _DeptDesc);            
        }

        private void GetStatDesc(string tCompCode, string tWrkGrp, string tUnitCode, string tDeptCode, string tStatCode)
        {
            Utils.MastCodeValidate.GetStatDesc(tCompCode, tWrkGrp, tUnitCode, tDeptCode,tStatCode ,out _StatDesc);
            
        }

        private void GetGradeDesc(string tCompCode, string tWrkGrp, string tGradeCode)
        {
            Utils.MastCodeValidate.GetGradeDesc(tCompCode, tWrkGrp, tGradeCode, out _GradeDesc);
        
        }

        private void GetDesgDesc(string tCompCode, string tWrkGrp, string tDesgCode)
        {

            Utils.MastCodeValidate.GetDesgDesc(tCompCode, tWrkGrp, tDesgCode, out _DesgDesc);            
        
        }

        private void GetCatDesc(string tCompCode, string tWrkGrp, string tCatCode)
        {

            Utils.MastCodeValidate.GetCatDesc(tCompCode, tWrkGrp, tCatCode, out _CatDesc);
        
        }
                
        public void GetCostDesc(string tCostCode)
        {
            Utils.MastCodeValidate.GetCostCode( tCostCode, out _CostDesc);
        }

        private void GetContDesc(string tCompCode, string tWrkGrp, string tUnitCode, string tContCode)
        {
            Utils.MastCodeValidate.GetContDesc(tCompCode, tWrkGrp, tUnitCode, tContCode, out _ContDesc);
            
        }

        public bool CreateMuster(string tEmpUnqID,DateTime tFromDt, DateTime tToDt, out string err)
        {
            bool returnval = false;
            //save in db for accountibility
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "CreateMuster";

                    int result = 0;

                    ////Creating instance of SqlParameter
                    SqlParameter sPfdate = new SqlParameter();
                    sPfdate.ParameterName = "@pFromDt";// Defining Name
                    sPfdate.SqlDbType = SqlDbType.DateTime; // Defining DataType
                    sPfdate.Direction = ParameterDirection.Input;// Setting the direction
                    sPfdate.Value = tFromDt;

                    ////Creating instance of SqlParameter
                    SqlParameter sPtdate = new SqlParameter();
                    sPtdate.ParameterName = "@pToDt";// Defining Name
                    sPtdate.SqlDbType = SqlDbType.DateTime; // Defining DataType
                    sPtdate.Direction = ParameterDirection.Input;// Setting the direction
                    sPtdate.Value = tToDt;

                    ////Creating instance of SqlParameter
                    SqlParameter sPEmpUnqID = new SqlParameter();
                    sPEmpUnqID.ParameterName = "@pEmpUnqID";// Defining Name
                    sPEmpUnqID.SqlDbType = SqlDbType.VarChar; // Defining DataType
                    sPEmpUnqID.Size = 10;
                    sPEmpUnqID.Direction = ParameterDirection.Input;// Setting the direction
                    sPEmpUnqID.Value = tEmpUnqID;

                    ////Creating instance of SqlParameter
                    SqlParameter sPWoDay = new SqlParameter();
                    sPWoDay.ParameterName = "@pWoDay";// Defining Name
                    sPWoDay.SqlDbType = SqlDbType.VarChar; // Defining DataType
                    sPWoDay.Size = 3;
                    sPWoDay.Direction = ParameterDirection.Input;// Setting the direction
                    sPWoDay.Value = this.WeekOffDay;

                    ////Creating instance of SqlParameter
                    SqlParameter sPWrkGrp = new SqlParameter();
                    sPWrkGrp.ParameterName = "@pWrkGrp";// Defining Name
                    sPWrkGrp.SqlDbType = SqlDbType.VarChar; // Defining DataType
                    sPWrkGrp.Size = 10;
                    sPWrkGrp.Direction = ParameterDirection.Input;// Setting the direction
                    sPWrkGrp.Value = "";

                    ////Creating instance of SqlParameter
                    SqlParameter sPresult = new SqlParameter();
                    sPresult.ParameterName = "@result"; // Defining Name
                    sPresult.SqlDbType = SqlDbType.Int; // Defining DataType
                    sPresult.Direction = ParameterDirection.Output;// Setting the direction 
                    sPresult.Value = result;

                    cmd.Parameters.Add(sPWrkGrp);
                    cmd.Parameters.Add(sPEmpUnqID);
                    cmd.Parameters.Add(sPfdate);
                    cmd.Parameters.Add(sPtdate);
                    cmd.Parameters.Add(sPWoDay);
                    cmd.Parameters.Add(sPresult);

                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    //get the output
                    int t = (int)cmd.Parameters["@result"].Value;

                    err = string.Empty;
                    returnval= true;
                }
                catch (Exception ex)
                {
                    err = ex.ToString();
                    returnval = false;
                }

            }//using connection

            return returnval;
        }

        public bool CreateEmployee(string tCompCode,string tEmpUnqID, string tWrkGrp,
            string tUnitCode, string tEmpName, string tFatherName , 
                string tSex , string tContactNo, string tEmail, string tCardNo, bool tActive,DateTime tBirthDt, DateTime tJoinDt,
                      out string err)
        {
            bool retval = false;
            err = "";

            //check if already exist
            this.CompCode = tCompCode;
            this.EmpUnqID = tEmpUnqID;
            if (this.GetEmpDetails( this.EmpUnqID))
            {
                err = "Employee Already Exist...";
                retval = false;
                return retval;

            }

            err = string.Empty;

            this.EmpName = tEmpName;
            this.FatherName = tFatherName;
            this.BirthDt = tBirthDt;
            this.JoinDt = tJoinDt;
            this.WrkGrp = tWrkGrp;            
            this.ContactNo = tContactNo;
            this.Email = tEmail;   
            this.CardNo = tCardNo;
            this.Gender = tSex;
           

            err = this.BasicValidation();
            if (!string.IsNullOrEmpty(err))
            {
                retval = false;
                return retval;
            }
            
            this.Gender = tSex;
            this.Active = tActive;
            

            /*** check for CostCode ..
             * 
            if(this.CostCode.Trim() != "")
            {
                string tsql1 = "select CostCode from MastCostCode where CostCode ='" + this.CostCode + "' and active = 1 " ;
                string t3 = Utils.Helper.GetDescription(tsql1,Utils.Helper.constr);
                if(string.IsNullOrEmpty(t3)){
                    err += "Invalid CostCode.." + Environment.NewLine;
                    retval = false;
                    return retval;
                }            
            }
            
            if (this.CostCode.Trim() == "")
            {
                err += "Please Enter CostCode.." + Environment.NewLine;
                retval = false;
                return retval;
            }


            string weekoff = "SUN,MON,TUE,WED,THU,FRI,SAT";
            if (!weekoff.Contains(this.WeekOffDay))
            {
                err += "Invalid Weekoff Days.." + Environment.NewLine;
                retval = false;
                return retval;
            }


            if (!this.Active)
            {
                err += "System Only Allow Active Employee to upload.." + Environment.NewLine;
                retval = false;
                return retval;
            }
            
            //nullify wrong values.

            this.GetDeptDesc(this.CompCode, this.WrkGrp, this.UnitCode,this.DeptCode);
            if (this.DeptDesc.Trim() == "")
            {
                err += "Invalid DeptCode.." + Environment.NewLine;
                this.DeptCode = "";
            }

            this.GetStatDesc(this.CompCode, this.WrkGrp, this.UnitCode, this.DeptCode,this.StatCode);
            if (this.StatDesc.Trim() == "")
            {
                err += "Invalid Station/Section Code.." + Environment.NewLine;
                this.StatCode = "";
            }

            this.GetDesgDesc(this.CompCode, this.WrkGrp, this.DesgCode);
            if (this.DesgDesc.Trim() == "")
            {
                err += "Invalid DesgCode.." + Environment.NewLine;
                this.DesgCode = "";
            }

            this.GetGradeDesc(this.CompCode, this.WrkGrp, this.GradeCode);
            if (this.GradeDesc.Trim() == "")
            {
                err += "Invalid Grade Code.." + Environment.NewLine;
                this.GradeCode = "";
            }

            

            this.GetCatDesc(this.CompCode, this.WrkGrp, this.CatCode);
            if (this.CatDesc.Trim() == "")
            {
                err += "Invalid Emp Cat Code.." + Environment.NewLine;
                this.CatCode = "";
            }

           
            this.CostCode = tCostCode;
            ***/

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    SqlTransaction tr = cn.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = tr;
                        cmd.CommandType = CommandType.Text;


                        string sql = "Insert into MastEmp (CompCode,WrkGrp,EmpUnqID,EmpName,FatherName," +
                            " BirthDt,JoinDt,Gender,Active," +
                            " ContactNo,EmailID,CardNo,AddDt,AddID ) Values (" +
                            "'{0}','{1}','{2}','{3}','{4}' ," +
                            " '{5}','{6}','{7}','{8}',"+
                            " '{9}','{10}','{11}',GetDate(),'{12}')";

                        sql = string.Format(sql, this.CompCode, this.WrkGrp, this.EmpUnqID, this.EmpName, this.FatherName,
                             Convert.ToDateTime(this.BirthDt).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.JoinDt).ToString("yyyy-MM-dd"),tSex,1,
                             tContactNo,tEmail,tCardNo,
                             Utils.User.GUserID
                            
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        retval = true;


                        

                        tr.Commit();

                       /*
                        
                        try
                        {
                            //createmuster
                            clsEmp t = new clsEmp();
                            string err2 = string.Empty;
                            if (t.GetEmpDetails(this.CompCode.Trim(), this.EmpUnqID.Trim()))
                            {
                                DateTime sFromDt, sToDt, sCurDt;
                                sCurDt = Convert.ToDateTime(Utils.Helper.GetDescription("Select GetDate()", Utils.Helper.constr));
                                if (Convert.ToDateTime(this.JoinDt).Year < sCurDt.Year)
                                {
                                    sFromDt = Convert.ToDateTime(Utils.Helper.GetDescription("Select CalendarStartOfYearDate from dbo.F_TABLE_DATE(GetDate(),GetDate())", Utils.Helper.constr));
                                    sToDt = Convert.ToDateTime(Utils.Helper.GetDescription("Select CalendarEndOfYearDate from dbo.F_TABLE_DATE(GetDate(),GetDate())", Utils.Helper.constr));
                                }
                                else
                                {
                                    sFromDt = Convert.ToDateTime(this.JoinDt);
                                    sToDt = Convert.ToDateTime(Utils.Helper.GetDescription("Select CalendarEndOfYearDate from dbo.F_TABLE_DATE('" + sFromDt.ToString("yyyy-MM-dd") + "','" + sFromDt.ToString("yyyy-MM-dd") + "')", Utils.Helper.constr));
                                }


                                if (!t.CreateMuster(sFromDt, sToDt, out err2))
                                {
                                    err += "Error While Creating Muster Table : " + err2;
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            err += "Error While Creating Muster Table :" + ex.ToString();
                        }
                        */


                    }

                }catch(Exception ex)
                {
                    err += ex.ToString();
                }
                
            }

            if (retval && !string.IsNullOrEmpty(err))
            {
                string err2 = "Employee Created.. With Errors : Please check->" + err;
                err = err2;
            }


            return retval;
        }

    }
}
