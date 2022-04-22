using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Net.Mail;

namespace Attendance.Classes
{
        
    class Globals
    {

        public static string MasterMachineIP = string.Empty;
        
        //used for scheduling jobs
        
        public static string G_ReportServiceURL;
        public static string G_ReportSerExeUrl;
        public static string G_DefaultMailID;
        public static string G_SmtpHostIP;
        public static string G_ServerWorkerIP;

        public static string G_NetworkDomain;
        public static string G_NetworkUser;
        public static string G_NetworkPass;

      

        public static bool GetGlobalVars()
        {
            bool tset = false;

            DataSet ds = new DataSet();

            

            string sql = "Select top 1 * From MastNetwork ";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                tset = true;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                  
                    G_ReportServiceURL = dr["ReportServiceURL"].ToString();
                    G_ReportSerExeUrl = dr["ReportSerExeURL"].ToString();
                    G_DefaultMailID = dr["DefaultMailID"].ToString();
                    G_SmtpHostIP = dr["SmtpHostIP"].ToString();
                    G_ServerWorkerIP = dr["ServerWorkerIP"].ToString();                    
                    G_NetworkDomain = dr["NetworkDomain"].ToString();
                    G_NetworkUser = dr["NetworkUser"].ToString();
                    G_NetworkPass = dr["NetworkPass"].ToString();
                    
                    Utils.DomainUserConfig.DomainName = dr["NetworkDomain"].ToString();
                    Utils.DomainUserConfig.DomainUser = dr["NetworkUser"].ToString();
                    Utils.DomainUserConfig.DomainPassword = dr["NetworkPass"].ToString();

                }


                
                
            }
            else
            {
                tset = false ;
            }

           
            return tset;


        }

     

        public static DateTime GetSystemDateTime()
        {
            DateTime dt = new DateTime();

            DataSet ds = new DataSet();
            string sql = "Select GetDate() as CurrentDate ";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            dt = DateTime.Now;
            if (hasRows)
            {
               

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt = (DateTime)dr["CurrentDate"];
                }

            }
            return dt;
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }

   
    
}
