using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.GISModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class UtilityController : Controller
    {

        public static Individual getRin(string number)
        {
            EIRSEntities _db = new EIRSEntities();
            var rin = _db.Individuals.FirstOrDefault(o => o.MobileNumber1 == number || o.MobileNumber2 == number);
            return rin;
        }
        public static void BL_TaxPayerCreated(EmailDetails pObjEmailDetails)
        {
            string rin = "";
            if (pObjEmailDetails.TaxPayerRIN == null)
            {
                var res = getRin(pObjEmailDetails.TaxPayerMobileNumber);
                if (res != null)
                {
                    rin = res.IndividualRIN;
                }
            }
            else
            {
                rin = pObjEmailDetails.TaxPayerRIN;
            }

            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(",We welcome you to the Edo State Internal Revenue Administration System. You have been successfully registered as a Tax Payer - ");
            sbSMSContent.Append(pObjEmailDetails.TaxPayerTypeName); sbSMSContent.Append(" and your Revenue Identification Number (RIN) is "); sbSMSContent.Append(rin);
            sbSMSContent.Append(". Please ensure that you quote this number for all your transactions with EIRS.");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.Tax_Payer_Capture,
                EventRefNo = pObjEmailDetails.TaxPayerRIN,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);

        }

        public static void BL_AssetProfileLinked(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(", you have been successfully associated with  ");
            sbSMSContent.Append(pObjEmailDetails.AssetName); sbSMSContent.Append(" as "); sbSMSContent.Append(pObjEmailDetails.TaxPayerRoleName);
            sbSMSContent.Append(" on the Edo State Internal Revenue Administration System. Your Revenue Identification Number (RIN) is ");
            sbSMSContent.Append(pObjEmailDetails.TaxPayerRIN);
            sbSMSContent.Append(" . Please ensure that you quote this number for all your transactions with EIRS.");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.Asset_Profile_Linked,
                EventRefNo = pObjEmailDetails.TaxPayerRIN + "/" + pObjEmailDetails.AssetRIN,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_BillGenerated(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(" a bill has been generated with reference "); sbSMSContent.Append(pObjEmailDetails.BillRefNo);
            if (!string.IsNullOrWhiteSpace(pObjEmailDetails.RuleNames))
            {
                sbSMSContent.Append(" for "); sbSMSContent.Append(pObjEmailDetails.RuleNames);
            }
            sbSMSContent.Append(", kindly pay "); sbSMSContent.Append(pObjEmailDetails.BillAmount);
            sbSMSContent.Append(" at any of the IGR collecting banks or visit https://tyson.eirs.gov.ng/webpay/");
            // sbSMSContent.Append(" at any of the IGR collecting banks or visit https://tyson.eirs.gov.ng/webpay/?billRef=");
            sbSMSContent.Append(pObjEmailDetails.BillRefNo);
            sbSMSContent.Append(" to make payment online.");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.Bill_Generated,
                EventRefNo = pObjEmailDetails.BillRefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_SettlementReceived(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName);
            sbSMSContent.Append(" a payment of ");
            sbSMSContent.Append(pObjEmailDetails.ReceivedAmount);
            sbSMSContent.Append(" has been received and applied against bill reference ");
            sbSMSContent.Append(pObjEmailDetails.BillRefNo);
            sbSMSContent.Append(". Thank you for your payment");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.Settlement_Received,
                EventRefNo = pObjEmailDetails.BillRefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_PaymentonAccount(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(" a payment of "); sbSMSContent.Append(pObjEmailDetails.ReceivedAmount);
            sbSMSContent.Append(" has been received and applied against your tax payer balance wallet on "); sbSMSContent.Append(pObjEmailDetails.TaxPayerRIN);
            sbSMSContent.Append(". Thank you for your payment");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.POA,
                EventRefNo = pObjEmailDetails.PoARefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }


        public bool CheckAccess(string url)
        {
            List<usp_GetScreenUserList_Result> kkk = new List<usp_GetScreenUserList_Result>();
            kkk = SessionManager.LstScreensToSee;
            if (kkk.Any(o => o.ScreenUrl == url))
            {
                return true;
            }
            return false;
        }

        public ActionResult AccessDenied()
        {

            string paths = $"{GlobalDefaultValues.DocumentLocation}/eirs403.jpg";
            string kk = " " + paths + " ";
            ViewBag.Pics = kk;
            Response.StatusCode = 200;
            return View();
        }
        public static bool SendSMS(string pStrToNumber, string pStrBody)
        {
            try
            {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://app.multitexter.com/v2/app/sms");
            httpWebRequest.ContentType = "application/json"; httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string email = NewCommon.SmsSenderUsername;
                //  string email = "tytunji29@gmail.com";
                string password = NewCommon.SmsSenderPassword;
                //string password = "Omoiyatayo01";
                string message = pStrBody;
                string sender_name = "ERAS";
                string recipients = pStrToNumber;
                string forcednd = "1";
                string json = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"message\":\"" + message + "\",\"sender_name\":\"" + sender_name + "\",\"recipients\":\"" + recipients + "\",\"forcednd\":\"" + forcednd + "\"}";
                streamWriter.Write(json); streamWriter.Flush(); streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd(); Console.WriteLine(result);
            }

            return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        // GET: Utility


        public ActionResult Index()
        {
            return View();
        }
    }
}