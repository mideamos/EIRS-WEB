using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class Utility : IUtility
    {
        EIRSEntities _db;
       
        public bool BL_TaxPayerCreated(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(", welcome to the Edo State Internal Revenue Administration System. You have been successfully registered as a Tax Payer - ");
            sbSMSContent.Append(pObjEmailDetails.TaxPayerTypeName); sbSMSContent.Append(" and your Revenue Identification Number (RIN) is "); sbSMSContent.Append(pObjEmailDetails.TaxPayerRIN);
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
            var ret = AddNotification(mObjNotification);
            return ret;
        }

        public bool BL_AssetProfileLinked(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(", you have been successfully associated with  ");
            sbSMSContent.Append(pObjEmailDetails.AssetName); sbSMSContent.Append(" as "); sbSMSContent.Append(pObjEmailDetails.TaxPayerRoleName);
            sbSMSContent.Append(" on the Edo State Internal Revenue Administration System. Your Revenue Identification Number (RIN) is "); sbSMSContent.Append(pObjEmailDetails.TaxPayerRIN);
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
            var ret = AddNotification(mObjNotification);
            return ret;
        }

        public bool BL_BillGenerated(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(" a bill has been generated with reference "); sbSMSContent.Append(pObjEmailDetails.BillRefNo);
            if (!string.IsNullOrWhiteSpace(pObjEmailDetails.RuleNames))
            {
                sbSMSContent.Append(" for "); sbSMSContent.Append(pObjEmailDetails.RuleNames);
            }
            sbSMSContent.Append(", kindly pay "); sbSMSContent.Append(pObjEmailDetails.BillAmount);
            sbSMSContent.Append(" at any of the IGR collecting banks or visit https://eras.eirs.gov.ng/BillDisplay.aspx?refno=");
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
            var ret = AddNotification(mObjNotification);
            return ret;
        }

        public bool BL_SettlementReceived(EmailDetails pObjEmailDetails)
        {
            StringBuilder sbSMSContent = new StringBuilder();
            sbSMSContent.Append("Dear "); sbSMSContent.Append(pObjEmailDetails.TaxPayerName); sbSMSContent.Append(" a payment of "); sbSMSContent.Append(pObjEmailDetails.ReceivedAmount);
            sbSMSContent.Append(" has been received and applied against bill reference "); sbSMSContent.Append(pObjEmailDetails.BillRefNo);
            sbSMSContent.Append(". Thank you for your payment");

            bool blnSMSSent = SendSMS(pObjEmailDetails.TaxPayerMobileNumber, sbSMSContent.ToString());

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.SMS,
                NotificationTypeID = (int)EnumList.NotificationType.Settlement_Received,
                EventRefNo = pObjEmailDetails.SettlementRefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnSMSSent,
                NotificationContent = sbSMSContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };
            var ret = AddNotification(mObjNotification);
            return ret;
        }

        public bool BL_PaymentonAccount(EmailDetails pObjEmailDetails)
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

            var ret = AddNotification(mObjNotification);
            return ret;
        }

        public bool AddNotification(Notification notification)
        {
            using (_db = new EIRSEntities())
            {
                _db.Notifications.Add(notification);
                _db.SaveChanges();
            }
            return true;
        }
        public static bool SendSMS(string pStrToNumber, string pStrBody)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://app.multitexter.com/v2/app/sms");
            httpWebRequest.ContentType = "application/json"; httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string email = "tytunji29@gmail.com";
                string password = "Omoiyatayo01";
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
                var result = streamReader.ReadToEnd();
            }
           
            return true;
        }
    }
}
