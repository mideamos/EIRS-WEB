using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.PeerToPeer;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace EIRS.Common
{
    public class EmailHandler
    {
        public static bool SendEmail(string p_strEmailRecepientName, string p_strEmailRecepientEmail, string p_strEmailSubject, bool p_blnEmailBodyHtml, string p_strEmailContent, ArrayList p_arlEmailAttachments)
        {
            try
            {
                string strEmailSenderName = GlobalDefaultValues.EmailSenderName;
                string strEmailSenderEmail = GlobalDefaultValues.EmailSenderEmail;

                SmtpClient scEmailSender = new SmtpClient();
                MailMessage mmEmailContainer = new MailMessage
                {
                    From = new MailAddress(strEmailSenderEmail, strEmailSenderName)
                };

                StringBuilder sbReceipient = new StringBuilder();
                sbReceipient.Append(p_strEmailRecepientName.Replace(".", " "));
                sbReceipient.Append(" <");
                sbReceipient.Append(p_strEmailRecepientEmail);
                sbReceipient.Append(">");
                mmEmailContainer.To.Add(sbReceipient.ToString());

                mmEmailContainer.Subject = p_strEmailSubject;
                mmEmailContainer.IsBodyHtml = p_blnEmailBodyHtml;
                mmEmailContainer.Body = p_strEmailContent;

                if (p_arlEmailAttachments != null)
                {
                    if (p_arlEmailAttachments.Count > 0)
                    {
                        foreach (Attachment at in p_arlEmailAttachments)
                        {
                            mmEmailContainer.Attachments.Add(at);
                        }
                    }
                }


                scEmailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
                scEmailSender.Send(mmEmailContainer);
            }
            catch (Exception Ex)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> SendEmail(string p_strEmailRecepientEmail, string p_strEmailSubject, string p_strEmailContent, ArrayList p_arlEmailAttachments)
        {
            string strEmailSenderName = GlobalDefaultValues.EmailSenderName;
            string strEmailSenderEmail = GlobalDefaultValues.EmailSenderEmail;
            try
            {
                var host = "smtp.mailtrap.io";
                var port = 2525;

                //var client = new SmtpClient(host, port)
                //{
                //    Credentials = new NetworkCredential("tytunji29@gmail.com", "Omoiyatayo00"),
                //    EnableSsl = true
                //};
                var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("f660450dc3aab9", "e068060c2f0925"),
                    EnableSsl = true
                };

                client.Send(strEmailSenderEmail, p_strEmailRecepientEmail, p_strEmailSubject, p_strEmailContent);
            }
            catch (Exception Ex)
            {
                return false;
            }

            return true;
        }
       
        public static bool SendEmail(string p_strSenderName, string p_strSenderEmail, string p_strEmailRecepientName, string p_strEmailRecepientEmail, string p_strEmailSubject, bool p_blnEmailBodyHtml, string p_strEmailContent, ArrayList p_arlEmailAttachments)
        {
            try
            {
                string strEmailSenderName = p_strSenderName;
                string strEmailSenderEmail = p_strSenderEmail;

                SmtpClient scEmailSender = new SmtpClient();
                MailMessage mmEmailContainer = new MailMessage
                {
                    From = new MailAddress(strEmailSenderEmail, strEmailSenderName)
                };

                StringBuilder sbReceipient = new StringBuilder();
                sbReceipient.Append(p_strEmailRecepientName.Replace(".", " "));
                sbReceipient.Append(" <");
                sbReceipient.Append(p_strEmailRecepientEmail);
                sbReceipient.Append(">");
                mmEmailContainer.To.Add(sbReceipient.ToString());

                mmEmailContainer.Subject = p_strEmailSubject;
                mmEmailContainer.IsBodyHtml = p_blnEmailBodyHtml;
                mmEmailContainer.Body = p_strEmailContent;

                if (p_arlEmailAttachments != null)
                {
                    if (p_arlEmailAttachments.Count > 0)
                    {
                        foreach (Attachment at in p_arlEmailAttachments)
                        {
                            mmEmailContainer.Attachments.Add(at);
                        }
                    }
                }

                scEmailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
                scEmailSender.Send(mmEmailContainer);
            }
            catch (Exception Ex)
            {
                return false;
            }

            return true;
        }
    }
}
