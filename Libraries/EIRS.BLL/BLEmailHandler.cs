using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EIRS.BLL
{
    public class BLEmailHandler
    {
        static StringBuilder sbHeader, sbFooter;

        public static void SetHeaderFooter()
        {
            sbHeader = new StringBuilder();
            sbHeader.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sbHeader.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sbHeader.Append("<head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /><meta name='viewport' content='width=device-width, initial-scale=1.0'><meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1'><style type='text/css'>");
            sbHeader.Append(" a img,img{outline:0;text-decoration:none}h1,h2,h3{font-style:normal}#bodyTable,#emailFooter,#emailHeader,body,html{background-color:#ececec}html{margin:0;padding:0}#bodyCellececec#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;width:100%!important;font-family:Helvetica,Arial,'Lucida Grande',sans-serif}.flexibleImage,a img,img{height:auto}table{border-collapse:collapse}table[id=bodyTable]{width:100%!important;margin:auto;max-width:500px!important;color:#7A7A7A;font-weight:400}a img,img{border:0;line-height:100%}a{text-decoration:none!important;border-bottom:1px solid}h1,h2,h3,h4,h5,h6{color:#5F5F5F;font-weight:400;font-family:Helvetica;font-size:20px;line-height:125%;text-align:Left;letter-spacing:normal;margin:0 0 10px;padding:0}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td,h1{line-height:100%}.ExternalClass,.ReadMsgBody{width:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}#outlook a{padding:0}img{-ms-interpolation-mode:bicubic;display:block}a,blockquote,body,li,p,table,td{-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;font-weight:400!important}h1,h2,h3,h4{font-weight:400;display:block}.ExternalClass td[class=ecxflexibleContainerBox] h3{padding-top:10px!important}h1{font-size:26px}h2{font-size:20px;line-height:120%}h3{font-size:17px;line-height:110%}.buttonContent,h4{font-size:18px;line-height:100%}h4{font-style:italic}.linkRemoveBorder{border-bottom:0!important}table[class=flexibleContainerCellDivider]{padding-bottom:0!important;padding-top:0!important}#emailBody{background-color:#FFF}.nestedContainer{background-color:#F8F8F8;border:1px solid #CCC}.emailButton{background-color:#205478;border-collapse:separate}.buttonContent{color:#FFF;font-family:Helvetica;font-weight:700;padding:15px;text-align:center}.emailCalendarDay,.emailCalendarMonth{font-family:Helvetica,Arial,sans-serif;font-weight:700;text-align:center}.buttonContent a{color:#FFF;display:block;text-decoration:none!important;border:0!important}.emailCalendar{background-color:#FFF;border:1px solid #CCC}.emailCalendarMonth{background-color:#205478;color:#FFF;font-size:16px;padding-top:10px;padding-bottom:10px}.emailCalendarDay{color:#205478;font-size:60px;line-height:100%;padding-top:20px;padding-bottom:20px}.imageContentText{margin-top:10px;line-height:0}.imageContentText a{line-height:0}#invisibleIntroduction{display:none!important}span[class=ios-color-hack] a{color:#275100!important;text-decoration:none!important}span[class=ios-color-hack2] a{color:#205478!important;text-decoration:none!important}span[class=ios-color-hack3] a{color:#8B8B8B!important;text-decoration:none!important}.a[href^=tel],a[href^=sms]{text-decoration:none!important;color:#606060!important;pointer-events:none!important;cursor:default!important}.mobile_link a[href^=tel],.mobile_link a[href^=sms]{text-decoration:none!important;color:#606060!important;pointer-events:auto!important;cursor:default!important}@media only screen and (max-width:480px){body,table[class=emailButton],table[class=flexibleContainer],table[id=emailHeader],table[id=emailBody],table[id=emailFooter],td[class=flexibleContainerCell]{width:100%!important}body{min-width:100%!important}td[class=flexibleContainerBox],td[class=flexibleContainerBox] table{display:block;width:100%;text-align:left}img[class=flexibleImage],td[class=imageContent] img{height:auto!important;width:100%!important;max-width:100%!important}img[class=flexibleImageSmall]{height:auto!important;width:auto!important}table[class=flexibleContainerBoxNext]{padding-top:10px!important}td[class=buttonContent]{padding:0!important}td[class=buttonContent] a{padding:15px!important}}");
            sbHeader.Append("<!--[ifmso12]><style type='text/css'>.flexibleContainer,</style><![endif]--><!--[ifmso14]><style type='text/css'>.flexibleContainer{display:block!important;width:100%!important}</style><![endif]--></style></head>");
            sbHeader.Append(" <body bgcolor='#E1E1E1' leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0'><center style='background-color:#E1E1E1;'> ");
            sbHeader.Append("<table border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable' style='table-layout: fixed;max-width:100% !important;width: 100% !important;min-width: 100% !important;'><tr><td align='center' valign='top' id='bodyCell' style='padding-top: 20px'><table bgcolor='#FFFFFF'  border='0' cellpadding='0' cellspacing='0' width='500' id='emailBody'><tr><td align='center' valign='top'>");
            sbHeader.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' style='color:#FFFFFF;' bgcolor='#35876a'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top' class='textContent'><h1 style='color:#FFFFFF;line-height:100%;font-family:Helvetica,Arial,sans-serif;font-size:30px;font-weight:bold; text-transform: uppercase; letter-spacing: 1px; margin-bottom:5px;text-align:center;'>Edo Internal Revenue Service</h1><h2 style='text-align:center;font-weight:normal;font-family:Helvetica,Arial,sans-serif;font-size:20px;margin-bottom:0px; margin-top: 20px; color:#FFF; font-weight: bold;'></h2><div style='text-align:center;font-family:Helvetica,Arial,sans-serif;font-size:17px;margin-bottom:0;color:#FFFFFF;'>My Government is Listening; and Acting</div></td></tr></table></td></tr></table></td></tr></table>");
            sbHeader.Append("</td></tr>");

            sbFooter = new StringBuilder();
            sbFooter.Append("</table><table bgcolor='#E1E1E1' border='0' cellpadding='0' cellspacing='0' width='500' id='emailFooter'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'>");
            sbFooter.Append("<table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td valign='top' bgcolor='#E1E1E1'><div style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#828282;text-align:center;line-height:120%;'><div>Copyright &#169; <span style='color:#828282;'>EIRS-ERAS</span></div></div></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table></center></body></html>");
        }

        public static void BL_TaxPayerCreated(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification of Registration";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear "); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append(", welcome to the Edo State Internal Revenue Administration System. You have been successfully registered as a Tax Payer with the following details:"); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer RIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Type: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Mobile Number: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerMobileNumber); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Email Address: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerEmail); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer TIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Contact Address: </b>"); sbEmailContent.Append(pObjEmailDetails.ContactAddress); sbEmailContent.Append("</p><br/>");
            sbEmailContent.Append("<p>You can view and modify all the details we hold about you by registering and logging into https://eras.eirs.gov.ng or should you need further support, kindly send an email to eras-support@eirs.gov.ng.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = EmailHandler.SendEmail(pObjEmailDetails.TaxPayerName, pObjEmailDetails.TaxPayerEmail, strSubject, true, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.Tax_Payer_Capture,
                EventRefNo = pObjEmailDetails.TaxPayerRIN,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_AssetProfileLinked(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification of Asset Profile";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear "); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append(", you have been successfully associated with an "); sbEmailContent.Append(pObjEmailDetails.AssetName); sbEmailContent.Append(" as  "); sbEmailContent.Append(pObjEmailDetails.TaxPayerRoleName); sbEmailContent.Append(" on the Edo State Internal Revenue Administration System with the following details:</p>");
            sbEmailContent.Append("<p> <b>Tax Payer RIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Type: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Asset Type: </b>"); sbEmailContent.Append(pObjEmailDetails.AssetTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Role: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRoleName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Asset Name: </b>"); sbEmailContent.Append(pObjEmailDetails.AssetName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Asset LGA: </b>"); sbEmailContent.Append(pObjEmailDetails.AssetLGA); sbEmailContent.Append("</p><br/>");
            sbEmailContent.Append("<p>You can view and modify all the details we hold about you and your assets by registering and logging into https://eras.eirs.gov.ng or should you need further support, kindly send an email to eras-support@eirs.gov.ng.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = EmailHandler.SendEmail(pObjEmailDetails.TaxPayerName, pObjEmailDetails.TaxPayerEmail, strSubject, true, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.Asset_Profile_Linked,
                EventRefNo = pObjEmailDetails.TaxPayerRIN + "/" + pObjEmailDetails.AssetRIN,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_BillGenerated(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification of Bill";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append(", a bill has been generated with reference "); sbEmailContent.Append(pObjEmailDetails.BillRefNo); sbEmailContent.Append(", on the Edo State Internal Revenue Administration System with the following details:"); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Bill Type: </b>"); sbEmailContent.Append(pObjEmailDetails.BillTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Reference: </b>"); sbEmailContent.Append(pObjEmailDetails.BillRefNo); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Amount to Pay: </b>"); sbEmailContent.Append(pObjEmailDetails.BillAmount); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer RIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Type: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p><br/>");
            sbEmailContent.Append("<p> kindly pay "); sbEmailContent.Append(pObjEmailDetails.BillAmount); sbEmailContent.Append("at any of the IGR collecting banks or visit www.eras.eirs.gov.ng to make payment online.</p><br/>");
            sbEmailContent.Append("<p>You can also view and modify all the details we hold about you and your assets by registering and logging into https://eras.eirs.gov.ng or should you need further support, kindly send an email to eras-support@eirs.gov.ng.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = EmailHandler.SendEmail(pObjEmailDetails.TaxPayerName, pObjEmailDetails.TaxPayerEmail, strSubject, true, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.Bill_Generated,
                EventRefNo = pObjEmailDetails.BillRefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_SettlementReceived(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification of Payment Receipt";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear "); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append(", a payment of "); sbEmailContent.Append(pObjEmailDetails.ReceivedAmount); sbEmailContent.Append(" has been received and recorded against bill reference "); sbEmailContent.Append(pObjEmailDetails.BillRefNo); sbEmailContent.Append(", on the Edo State Internal Revenue Administration System and with the following details:"); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Bill Type: </b>"); sbEmailContent.Append(pObjEmailDetails.BillTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Reference: </b>"); sbEmailContent.Append(pObjEmailDetails.BillRefNo); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Amount to Pay: </b>"); sbEmailContent.Append(pObjEmailDetails.BillAmount); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Amount Paid: </b>"); sbEmailContent.Append(pObjEmailDetails.BillPaidAmount); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Outstanding Amount: </b>"); sbEmailContent.Append(pObjEmailDetails.BillOutstandingAmount); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Bill Status: </b>"); sbEmailContent.Append(pObjEmailDetails.BillStatusName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer RIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Type: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p><br/>");
            sbEmailContent.Append("<p>You can also view and modify all the details we hold about you and your assets by registering and logging into https://eras.eirs.gov.ng or should you need further support, kindly send an email to eras-support@eirs.gov.ng.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = EmailHandler.SendEmail(pObjEmailDetails.TaxPayerName, pObjEmailDetails.TaxPayerEmail, strSubject, true, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.Settlement_Received,
                EventRefNo = pObjEmailDetails.SettlementRefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }

        public static void BL_PaymentonAccount(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification of Payment on Account";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear "); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append(", a payment of "); sbEmailContent.Append(pObjEmailDetails.ReceivedAmount); sbEmailContent.Append(" has been received and applied against your tax payer balance wallet on  "); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append(" and with the following details:"); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Reference: </b>"); sbEmailContent.Append(pObjEmailDetails.PoARefNo); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Amount Paid: </b>"); sbEmailContent.Append(pObjEmailDetails.ReceivedAmount); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer RIN: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Type: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerTypeName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Payer Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p><br/>");
            sbEmailContent.Append("<p>You can also view and modify all the details we hold about you and your assets by registering and logging into https://eras.eirs.gov.ng or should you need further support, kindly send an email to eras-support@eirs.gov.ng.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = EmailHandler.SendEmail(pObjEmailDetails.TaxPayerName, pObjEmailDetails.TaxPayerEmail, strSubject, true, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.POA,
                EventRefNo = pObjEmailDetails.PoARefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }
        public static async Task BL_TccSignerAsync(EmailDetails pObjEmailDetails)
        {
            SetHeaderFooter();
            string strSubject = "ERAS Notification Of Tax Clearance Certificate";

            StringBuilder sbEmailContent = new StringBuilder();
            sbEmailContent.Append(sbHeader);
            sbEmailContent.Append("<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'><tr><td align='center' valign='top'>");
            sbEmailContent.Append("<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            sbEmailContent.Append("<tr><td valign='top' class='textContent'>");
            sbEmailContent.Append("<div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>");
            sbEmailContent.Append("<p>"); sbEmailContent.Append("Dear "); sbEmailContent.Append(pObjEmailDetails.FirstSignerName); sbEmailContent.Append("</ b >");
            sbEmailContent.Append("We are pleased to inform you that a Tax Clearance Certificate has been successfully generated for the individual/ entity associated with Revenue Identification Number(RIN): "); sbEmailContent.Append(pObjEmailDetails.TaxPayerRIN);
            sbEmailContent.Append(". The certificate has been created within our platform and is now ready for your review and signature.Your official signature is required to validate the authenticity of the certificate and complete the process. ");
            sbEmailContent.Append("<p> Certificate Details:</p><br/>"); 
            sbEmailContent.Append("<p> <b>Certificate ID:  </b>"); sbEmailContent.Append(pObjEmailDetails.CertificateID); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Expiry Date: </b>"); sbEmailContent.Append(pObjEmailDetails.ExpiryDate); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Taxpayer's Name: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxPayerName); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p> <b>Tax Year(s) Covered: </b>"); sbEmailContent.Append(pObjEmailDetails.TaxYearCovered); sbEmailContent.Append("</p>");
            sbEmailContent.Append("<p>  Please follow these steps to review and sign the certificate:</p><br/>");
            sbEmailContent.Append("<p>Log in to  https://eras.eirs.gov.ng using your credentials, Navigate to the \"Process TCC\" section.</p><br/>");
            sbEmailContent.Append("<p>Locate the Tax Clearance Certificate with the ID: </p><br/>"); sbEmailContent.Append(pObjEmailDetails.CertificateID);
            sbEmailContent.Append("<p>Open the certificate to review the details and ensure their accuracy.</p><br/>");
            sbEmailContent.Append("<p>Electronically sign the certificate using the designated signature field.</p><br/>");
            sbEmailContent.Append("<p>Kindly note that your signature is legally binding and confirms the authenticity of the certificate.</p><br/>");
            sbEmailContent.Append("<p>Your prompt attention to this matter is greatly appreciated, as it will enable us to provide the taxpayer with a duly signed Tax Clearance Certificate in a timely manner.</p><br/>");
            sbEmailContent.Append("<p>EIRS Automation Team</p><br/>");

            
            sbEmailContent.Append(sbFooter.ToString());

            bool blnEmailSent = await EmailHandler.SendEmail( pObjEmailDetails.FirstSignerEmail, strSubject, sbEmailContent.ToString(), null);

            //Add In Notification Table
            Notification mObjNotification = new Notification()
            {
                NotificationDate = CommUtil.GetCurrentDateTime(),
                NotificationMethodID = (int)EnumList.NotificationMethod.Email,
                NotificationTypeID = (int)EnumList.NotificationType.POA,
                EventRefNo = pObjEmailDetails.PoARefNo,
                TaxPayerTypeID = pObjEmailDetails.TaxPayerTypeID,
                TaxPayerID = pObjEmailDetails.TaxPayerID,
                NotificationModeID = (int)EnumList.NotificationMode.Auto,
                NotificationStatus = blnEmailSent,
                NotificationContent = sbEmailContent.ToString(),
                CreatedBy = pObjEmailDetails.LoggedInUserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            new BLNotification().BL_InsertUpdateNotification(mObjNotification);
        }
    }
}
