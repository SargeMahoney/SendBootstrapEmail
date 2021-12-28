namespace SendBootstrapEmail.EmailServices
{
    public interface IEmailSender
    {
        Task SendEmailsAsync(List<string> emails, string subject, string message);
    }
}
