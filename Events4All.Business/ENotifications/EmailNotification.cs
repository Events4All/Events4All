using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Web;
using System.Net;
using System.IO;
using Events4All.Business.Resource;


namespace Events4All.Business.ENotifications
{
    public class EmailNotification
    {
        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            bool isSend = false;

            try
            {
                var body = msg;
                var message = new MailMessage();

                message.To.Add(new MailAddress(email));
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSend;
        }
        /*public void main()
        {
            var task = SendEmailAsync("jrhos97@gmail.com", "Hello", "test");
             task.RunSynchronously();
        }*/
    }
    
}


