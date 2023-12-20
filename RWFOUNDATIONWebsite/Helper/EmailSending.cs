using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Helper
{
    public class EmailSending
    {
        private static string EMAILSERVER = "webmail.rwfoundation.org";       
        private static string EMAIL_PASSWORD = "7Cude82$";       
        private static string FromEmail = "web@rwfoundation.org";
        private static string From_DisplayName = "RW Foundation";
        public static bool SendMail(string SendTo, string subject, string message)
        {
            if (string.IsNullOrEmpty(subject))
            {
                subject = "RW Foundation";
            }
            var fromAddress = new MailAddress(FromEmail, From_DisplayName);
            var toAddress = new MailAddress(SendTo, "");
            string fromPassword = EMAIL_PASSWORD;
            var smtp = new SmtpClient
            {
                Host = EMAILSERVER,
                Port = 2525,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var result = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })
            {
                try
                {
                    smtp.Send(result);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
