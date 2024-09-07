namespace DeloiteAssement
{
    public interface EmailSender
    {
        Task SendEmailAsync (string email, string subject, string message);
    }
}
