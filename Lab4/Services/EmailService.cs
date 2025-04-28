using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;

namespace Lab4.Services
{
    public class EmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string imapServer = "imap.gmail.com";
        private readonly int imapPort = 993;
        private readonly string pop3Server = "pop.gmail.com";
        private readonly int pop3Port = 995;
        private readonly string emailAddress = "labwork4noreply@gmail.com";
        private readonly string emailPassword = "jxyf ygbl sapp jwmy";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Task Manager", emailAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailAddress, emailPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task<List<string>> CheckInboxImapAsync()
        {
            using var client = new ImapClient();
            await client.ConnectAsync(imapServer, imapPort, true);
            await client.AuthenticateAsync(emailAddress, emailPassword);
            await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

            var messages = new List<string>();
            for (int i = 0; i < client.Inbox.Count && i < 10; i++)
            {
                var message = await client.Inbox.GetMessageAsync(i);
                messages.Add($"{message.Date.LocalDateTime}: {message.Subject}");
            }

            await client.DisconnectAsync(true);
            return messages;
        }

        public async Task<List<string>> CheckInboxPop3Async()
        {
            using var client = new Pop3Client();
            await client.ConnectAsync(pop3Server, pop3Port, true);
            await client.AuthenticateAsync(emailAddress, emailPassword);

            var messages = new List<string>();
            for (int i = 0; i < client.Count && i < 10; i++)
            {
                var message = await client.GetMessageAsync(i);
                messages.Add($"{message.Date.LocalDateTime}: {message.Subject}");
            }

            await client.DisconnectAsync(true);
            return messages;
        }
    }
}