using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using FluentFTP;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace SapManPowerFTPClient
{
   

    class Program
    {
        public static string sqlhost = "172.16.12.47";
        public static string database = "Attendance";
        public static string sqluser = "attd";
        public static string sqlpass = "attd123";
        public static string cnstr;
        static void Main(string[] args)
        {
            cnstr = "Data Source = " + sqlhost + ";Initial Catalog=" + database + ";User ID=" + sqluser + ";Password=" + sqlpass + ";";
            
            //required parameters
            //-h "172.16.12.11" -u "smgdc\ipuit" --password "it123" --path "ipuit"
            
            CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
	           
        }

        static void RunOptions(Options opts)
        {
            GetFiles(opts.InputHost.ToString(), opts.InputUser.ToString(), opts.InputPassword.ToString() , opts.InputPath.ToString());
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("invalid args, use --help to view arguments");
        }

        //get predefined files.
        private static void GetFiles(string host,string user,string pass,string path)
        {
            int counter = 0;

            try
            {

                using (FtpClient conn = new FtpClient())
                {
                    string ftpPath = host + "\\" + path;

                    string downloadFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FTPTest");

                    downloadFileName += "\\";

                    conn.Host = host;
                    conn.Credentials = new NetworkCredential(user, pass);
                    conn.Connect();

              
                    foreach (FtpListItem item in conn.GetListing(conn.GetWorkingDirectory(),
                        FtpListOption.Modify ))
                    {
                        // if this is a file
                        if (item.Type == FtpFileSystemObjectType.File && item.Name.Contains("_.TXT"))
                        {
                            string localFilePath = downloadFileName + item.FullName;


                            //File name Pattern search
                            //WcProduction2021080220210802_.TXT -- file created/uploaded
                            //this file contains previous date days data.

                            if (item.Name.IndexOf("WcProduction",0) == 0 && item.Name.Length > 25 )
                            {
                                string udate = item.Name.Substring(12, 8);
                                DateTime UploadDate;
                                if (!DateTime.TryParseExact(udate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out UploadDate))
                                {
                                    Utility.WriteLog("Invalid template .. date parsing issue.." + item.Name);
                                    continue;
                                }

                                //Only newly created files will be downloaded.
                                if (!File.Exists(localFilePath))
                                {
                                    counter++;
                                    conn.DownloadFile(localFilePath, item.FullName);
                                    //Do any action here.
                                    Console.WriteLine("Processing .." + item.Name);
                                    Utility.WriteLog("Processing .." + item.Name);
                                    
                                    //get record date
                                    DateTime RecDate = UploadDate;

                                    Console.WriteLine("Processing Records for.." + RecDate.ToString("yyyy-MM-dd"));
                                    Utility.WriteLog("Processing Records for.." + RecDate.ToString("yyyy-MM-dd"));

                                    List<Output> t = new List<Output>();
                                    string[] lines = System.IO.File.ReadAllLines(localFilePath);

                                    if (lines.Length > 0 )
                                    {
                                        if (!lines[0].Contains("Work Center|Cost Center|Quantity(MT)"))
                                            continue;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    foreach (string l in lines)
                                    {
                                        if (l.Contains("Work Center"))
                                            continue;

                                        string err = string.Empty;
                                        Output o = new Output();
                                        o.WorkCenter = l.Split('|')[0].ToString();
                                        o.CostCenter = l.Split('|')[1].ToString();
                                        o.OutPut = Convert.ToDecimal(l.Split('|')[2].ToString());
                                        o.CostGroup = Utility.GetDescription("select CostGroup from MastCostCode where CostCode = '" + o.CostCenter.Trim() + "'", cnstr, out err);
                                        o.RecDate = RecDate;
                                        
                                        //o.update(cnstr, out err);
                                        //if (!string.IsNullOrEmpty(err))
                                        //{
                                        //    Utility.WriteLog(err);
                                        //}

                                        if (string.IsNullOrEmpty(o.CostGroup))
                                        {
                                            Console.WriteLine("CostGroup not Defined ->" + o.WorkCenter + "->" + o.CostCenter + "->" + o.OutPut.ToString());
                                            Utility.WriteLog("CostGroup not Defined ->" + o.WorkCenter + "->" + o.CostCenter + "->" + o.OutPut.ToString());
                                        }
                                            
                                        t.Add(o);
                                    }

                                    //group by costcenter to calculate output1
                                    List<Output> result = t
                                    .GroupBy(l => l.CostCenter)
                                    .Select(cl => new Output
                                    {
                                        RecDate = RecDate,
                                        CostCenter = cl.First().CostCenter,                                       
                                        OutPut = cl.Sum(c => c.OutPut),
                                    }).ToList();

                                    if (result.Count > 0)
                                    {
                                        foreach (Output g in result)
                                        {
                                            string err;
                                            bool f = g.update(cnstr, out err);

                                            if (!f)
                                                Utility.WriteLog(g.CostCenter + g.OutPut.ToString() + "->" + err);
                                            else
                                                Utility.WriteLog(g.CostCenter + g.OutPut.ToString() + "->" + err);

                                        }
                                    }

                                   

                                } // if localfile not exist

                            } //if filename starts with WcProduction
                        
                        } //if _.TXT

                    } //foreach loop
                }//using ftpconnection
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message.ToString());
                Utility.WriteLog("Error :" + ex.Message.ToString());
            }
            Console.WriteLine("Nos of File Processed.." + counter.ToString());
            Utility.WriteLog("Nos of File Processed.." + counter.ToString());
            
            Console.WriteLine("Process completed...");
            Utility.WriteLog("Process completed...");
        }

        

       
    }

    
    
}
