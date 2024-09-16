using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EIRS.Web.Utility
{
    public class Logger
    {
        //public static string MapPath(string path)
        //{
        //    return Path.Combine(
        //        (string)AppDomain.CurrentDomain.GetData("ContentRootPath") ?? string.Empty,
        //        path);
        //}


        public static string RootPath()
        {
            return (string)AppDomain.CurrentDomain.GetData("ContentRootPath") ?? string.Empty;
        }
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.ToString();
            Errormsg = ex.GetType().Name;
            extype = ex.GetType().ToString();
            ErrorLocation = ex.Message;

            try
            {

                 string filepath = WebConfigurationManager.AppSettings["errorLogLocation"] ?? "";
               // string filepath = Path.Combine(RootPath(), "ErrorLog/");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MMM-yyyy") + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    File.AppendText(filepath);
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + line +
                                   "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line +
                                   "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation +
                                   line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

    }
}