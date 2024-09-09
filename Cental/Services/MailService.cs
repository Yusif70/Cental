using Cental.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Cental.Services
{
    public class MailService : IMailService
    {
        public void SendMail(string to, string subject, string body)
        {
            string from = "yusifpirquliyev7@gmail.com";
            MailMessage mail = new();
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(from);

            SmtpClient smtpClient = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, "mwdb oave bkga rjoh"),
                EnableSsl = true
            };
            smtpClient.Send(mail);
        }
    }
}
