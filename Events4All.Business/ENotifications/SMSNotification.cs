using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Events4All.Business.Resource;

namespace Events4All.Business.ENotifications
{
    public class SMSNotification
    {
        public async Task<bool> SendSMSAsync(int phone, string msg, string subject = "")
        {
            bool isSend = false;

            try
            {
                var body = msg;
                var message = new MailMessage();

                var att = phone.ToString() + "@txt.att.net";
                var vzon = phone.ToString() + "@vtext.com";
                var sprint = phone.ToString() + "@messaging.sprintpcs.com";
                var tmob = phone.ToString() + "@tmomail.net";

                message.To.Add(new MailAddress(att));
                message.To.Add(new MailAddress(vzon));
                message.To.Add(new MailAddress(sprint));
                message.To.Add(new MailAddress(tmob));

                message.From = new MailAddress(EmailInfo.FROM_EMAIL_ACCOUNT);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : EmailInfo.EMAIL_SUBJECT_DEFAULT;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                        Password = EmailInfo.FROM_EMAIL_PASSWORD
                    };

                    smtp.Credentials = credential;
                    smtp.Host = EmailInfo.SMTP_HOST_GMAIL;
                    smtp.Port = Convert.ToInt32(EmailInfo.SMTP_PORT_GMAIL);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);

                    isSend = true;
                    //var Tcs = new TaskCompletionSource<bool>(SendEmailAsync("jrhos97@gmail.com", "Hello", "Test"));
                    //return Tcs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSend;
        }
    }
}

