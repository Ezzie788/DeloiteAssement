using System.Net;
using System.Net.Mail;

namespace DeloiteAssement
{
    public class EmailService : EmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            

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
