using SendBootstrapEmail.Configuration;
using System.Net.Mail;

namespace SendBootstrapEmail.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailSender(IEmailConfiguration emailConfiguration)
        {
            this._emailConfiguration = emailConfiguration;
        }
        public async Task SendEmailsAsync(List<string> emails, string subject, string message)
        {
            try
            {

                var smtClient = new SmtpClient()
                {
                    Port = this._emailConfiguration.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = this._emailConfiguration.MailServer,
                    Credentials = new System.Net.NetworkCredential(this._emailConfiguration.SenderName, this._emailConfiguration.Password)
                };

                foreach (var email in emails)
                {
                    var myMail = new MailMessage(this._emailConfiguration.Sender, email)
                    {
                        Subject = subject,
                        Body = message
                    };
                    await smtClient.SendMailAsync(myMail);
                }

                smtClient.Dispose();
            }
            catch (Exception ex)
            {
               // TODO LOG ex
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
