using Elmah;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;

namespace EIRS.API.Utility
{
    public class NewErrorLog
    {
       
        public static void ErrorLogging(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;
            String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
            ErrorlineNo = "";
            exurl = "";
            hostIp = "";
            Errormsg = ex.GetType().Name;
            extype = ex.GetType().ToString();
            ErrorLocation = ex.Message;
            string ErrorLogPath = WebConfigurationManager.AppSettings["NewErrorLogPath"] ?? "";

            string strPath = ErrorLogPath + " ErrorLog.txt";
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw2 = File.AppendText(strPath))
            {
                sw2.WriteLine("=============Error Logging ===========");
                sw2.WriteLine("===========Start============= " + DateTime.Now);
                sw2.WriteLine("Error Message: " + ex.Message);
                sw2.WriteLine("Stack Trace: " + ex.StackTrace);
                sw2.WriteLine("===========End============= " + DateTime.Now);
                sw2.Flush();
                sw2.Close();

            }

            //using StreamWriter sw = File.AppendText(strPath);
            //string error = "Log Written Date:" + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + line +
            //               "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line +
            //               "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation +
            //               line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
            //sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "-----------------");
            //sw.WriteLine("-------------------------------------------------------------------------------------");
            //sw.WriteLine(line);
            //sw.WriteLine(error);
            //sw.WriteLine("--------------------------------*End*------------------------------------------");
            //sw.WriteLine(line);
            
        }

    }
}