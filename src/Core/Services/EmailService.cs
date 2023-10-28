using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string from, string to, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(null, from));
            message.To.Add(new MailboxAddress(null, to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("localhost", 25, MailKit.Security.SecureSocketOptions.None);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
