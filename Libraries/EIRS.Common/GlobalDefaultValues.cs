using System;
using System.Web.Configuration;

namespace EIRS.Common
{
    public static class GlobalDefaultValues
    {
        public static string cdnLink = WebConfigurationManager.AppSettings["cdnLink"] ?? "";
        public static string DocumentLink = WebConfigurationManager.AppSettings["documentLink"] ?? "";
        public static string DocumentLocation = WebConfigurationManager.AppSettings["documentLocation"] ?? "";

        public static string SiteLink = WebConfigurationManager.AppSettings["siteLink"] ?? "";

        public static string EmailSenderName = (WebConfigurationManager.AppSettings["EmailSenderName"] ?? "");
        public static string EmailSenderEmail = (WebConfigurationManager.AppSettings["EmailSenderEmail"] ?? "");

        public static string AntiForgeryCookieName = (WebConfigurationManager.AppSettings["AntiForgeryCookieName"] ?? "");
        public static string APIURL = (WebConfigurationManager.AppSettings["APIURL"] ?? "");


        public static string Twilio_AccountSID = (WebConfigurationManager.AppSettings["Twilio_AccountSID"] ?? "");
        public static string Twilio_SenderID = (WebConfigurationManager.AppSettings["Twilio_SenderID"] ?? "");
        public static string Twilio_AuthToken = (WebConfigurationManager.AppSettings["Twilio_AuthToken"] ?? "");
        public static string CountryCode = (WebConfigurationManager.AppSettings["CountryCode"] ?? "");

        public static string EnvironmentMode = (WebConfigurationManager.AppSettings["EnvironmentMode"] ?? "Demo");
        public static string VersionNumber = (EnvironmentMode == "Live" ? (WebConfigurationManager.AppSettings["VersionNumber"] ?? "1.0.0.1") : DateTime.Now.ToString("ddMMyyhhmmssfff"));
        public static bool SendNotification = (WebConfigurationManager.AppSettings["SendNotification"] != null ? TrynParse.parseBool(WebConfigurationManager.AppSettings["SendNotification"]) : false);
        public static int TCC_MDAServiceID = (WebConfigurationManager.AppSettings["TCC_MDAServiceID"] != null ? TrynParse.parseInt(WebConfigurationManager.AppSettings["TCC_MDAServiceID"]) : -1);
        public static int TCC_PDFTemplateID = (WebConfigurationManager.AppSettings["TCC_PDFTemplateID"] != null ? TrynParse.parseInt(WebConfigurationManager.AppSettings["TCC_PDFTemplateID"]) : -1);
        public static int TCC_SEDEOrganizationID = (WebConfigurationManager.AppSettings["TCC_SEDEOrganizationID"] != null ? TrynParse.parseInt(WebConfigurationManager.AppSettings["TCC_SEDEOrganizationID"]) : -1);


        public static string JTB_APIURL = (WebConfigurationManager.AppSettings["JTB_APIURL"] ?? "");
        public static string JTB_Username = (WebConfigurationManager.AppSettings["JTB_Username"] ?? "");
        public static string JTB_Password = (WebConfigurationManager.AppSettings["JTB_Password"] ?? "");
        public static string JTB_LOGINURL = (WebConfigurationManager.AppSettings["JTB_LOGINURL"] ?? "");
        public static string JTB_INDIVIDUALURL = (WebConfigurationManager.AppSettings["JTB_INDIVIDUALURL"] ?? "");
        public static string JTB_NONINDIVIDUALURL = (WebConfigurationManager.AppSettings["JTB_NONINDIVIDUALURL"] ?? "");

        public static string SEDE_APIURL = (WebConfigurationManager.AppSettings["SEDE_APIURL"] ?? "");
        public static string SEDE_APIToken = (WebConfigurationManager.AppSettings["SEDE_APIToken"] ?? "");

        public static string SEDE_API_PDFTemplateUrl = "Reference/PDFTemplate";
        public static string SEDE_API_PDFTemplateFieldUrl = "Reference/PDFTemplateField";
        public static string SEDE_API_PDFTemplateDetailUrl = "Reference/PDFTemplateDetails";
        public static string SEDE_API_QRTemplateDetailUrl = "Reference/QRTemplateDetails";
        public static string SEDE_API_OverlayTemplateDetailUrl = "Reference/OverlayTemplateDetails";

        public static string SEDE_API_AddDocument = "ExchangeDocument/InsertDocument";
        public static string SEDE_API_UpdateDocument = "ExchangeDocument/UpdateDocument";
        public static string SEDE_API_DownloadDocument = "ExchangeDocument/DownloadDocument";
        public static string SEDE_API_ProcessDocument = "ExchangeDocument/ProcessDocument";
        public static string SEDE_API_SignDocument = "ExchangeDocument/SignDocument";
        public static string SEDE_API_VisibleSignDocument = "ExchangeDocument/VisibleMultiSignDocument";
        public static string SEDE_API_SealDocument = "ExchangeDocument/SealDocument";
        public static string SEDE_API_CancelDocument = "ExchangeDocument/CancelDocument";
        public static string SEDE_API_VerifyDocument = "ExchangeDocument/VerifyDocument";

        public static string RECEIPT_NOTIFICATION_EMAIL = (WebConfigurationManager.AppSettings["RECEIPT_NOTIFICATION_EMAIL"] ?? "");
        public static string BILL_NOTIFICATION_EMAIL = (WebConfigurationManager.AppSettings["BILL_NOTIFICATION_EMAIL"] ?? "");
    }
}
