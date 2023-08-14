using System.Net;
using System.Net.Mail;

namespace ASP111.Services.Email
{
    public class GmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public GmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string Email, string Message, string Subject)
        {
            var GmailSection = _configuration.GetSection("Smtp").GetSection("Gmail");
            String? Host = GmailSection.GetValue<String>("Host");
            int? Port = GmailSection.GetValue<int>("Port");
            String? box = GmailSection.GetValue<String>("Email");
            String? Password = GmailSection.GetValue<String>("Password");
            bool? Ssl = GmailSection.GetValue<bool>("Ssl");
            if (Host == null || Port == null || box == null || Password == null || Ssl == null)
            {
                throw new ApplicationException("Configuration for SMTP::Gmail not full");
            }
            SmtpClient smtpClient = new SmtpClient(Host) {
                Port = Port.Value,
                EnableSsl = Ssl.Value,
                Credentials = new NetworkCredential(box, Password)
            };
            MailMessage mailMessage = new()
            {
                From = new MailAddress(box),
                Body = Message,
                IsBodyHtml = false,
                Subject = Subject
            };
            mailMessage.To.Add(new MailAddress(Email));
            smtpClient.Send(mailMessage);
        }
    }
}
