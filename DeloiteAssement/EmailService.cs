using System.Net;
using System.Net.Mail;

namespace DeloiteAssement
{
    public class EmailService : EmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "ezekiel79@outlook.com";
            var pw = "Deloittetestpw1234";

            var client = new SmtpClient("smtp-mail.outlook.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(mail, pw);

            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                message));
        }
    }
}
