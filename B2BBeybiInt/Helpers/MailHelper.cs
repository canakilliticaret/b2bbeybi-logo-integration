using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace INT.BeybiB2B.Helpers
{
    public class MailHelper
    {
        public static bool SendMail(string smtpSender, string smtpPassword, string smtpHost, int smtpPort, bool enableSsl, bool skipCertificateValidation, string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(smtpSender, smtpPassword);
                smtpClient.Host = smtpHost;
                smtpClient.Port = smtpPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = enableSsl;
                if (skipCertificateValidation)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                }


                MailMessage message = new MailMessage();

                foreach (var address in to.Split(';'))
                {
                    message.To.Add(address);
                }

                if (!String.IsNullOrEmpty(cc.Trim()))
                {
                    foreach (var address in cc.Split(';'))
                    {
                        message.CC.Add(address);
                    }
                }

                if (!String.IsNullOrEmpty(bcc.Trim()))
                {
                    foreach (var address in bcc.Split(';'))
                    {
                        message.Bcc.Add(address);
                    }
                }

                message.Subject = subject;


                message.From = new MailAddress(from);
                message.IsBodyHtml = isBodyHtml;
                message.Body = body;

                try
                {
                    smtpClient.Send(message);
                    //MailLogger.Append(smtpSender, smtpHost, smtpPort, from, to, cc, bcc, subject, body, isBodyHtml, isMailTest, CommonHelper.GetIpAddress(), "");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                return true;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MailHelper", "SendMail4");
                return false;
            }
        }


        public static bool SendMail(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml)
        {
            try
            {
                string smtpSender = "entegrasyon@akilliticaretim.com";
                string smtpPassword = "c6flmSvt";
                string smtpHost = "mail.akilliticaretim.com";
                int smtpPort = 587;
                bool enableSsl = false;
                bool skipCertificateValidation = false;
                return SendMail(smtpSender, smtpPassword, smtpHost, smtpPort, enableSsl, skipCertificateValidation, from, to, cc, bcc, subject, body, isBodyHtml);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
