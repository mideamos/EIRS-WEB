using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EIRS.Web.GISModels
{
    public class NewCommon
    {
        public static string GISApi = WebConfigurationManager.AppSettings["GISApi"] ?? "";
        public static string GISLogin = WebConfigurationManager.AppSettings["GISLogin"] ?? "";
        public static string SmsSender = WebConfigurationManager.AppSettings["SmsSender"] ?? "";
        public static string SmsSenderUsername = WebConfigurationManager.AppSettings["SmsSenderUsername"] ?? "";
        public static string SmsSenderPassword = WebConfigurationManager.AppSettings["SmsSenderPassword"] ?? "";
    }
}