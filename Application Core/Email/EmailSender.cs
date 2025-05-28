using System.Net.Mail;
using MailBodyPack;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ShopOwnerCore.Application_Core.Email
{
    public class EmailSender:IEmailSender
    {
        private readonly IOptions<EmailSetting> _settings;

        public EmailSender(IOptions<EmailSetting> settings)
        {
            _settings = settings;
        }

        private string AccountCreationConfirm(EmailOptions options)
        {
            var emailInfo = new[]
            {
                $"UserName: { options.UserName}",
                $"Password: {options.Password}"
            };

            var emailInfoFormat = emailInfo.Select(item => MailBody.CreateBlock().Text(item));

            var Body = MailBody.CreateBody().Paragraph($"Hi{options.UserName} please confirm your email address by clicking the link below")
                .Paragraph("Here is your login information").UnorderedList(emailInfoFormat).Paragraph("Please Change it After Login").Button($"{options.Url}", "Confirm Email Address")
                .Paragraph("ShopCoreOwner").ToString();
            return Body;
        }

        private string AccountForgotPassword(EmailOptions options)
        {
            const string appName = "ShopOwnerCore";

            var body = MailBody.CreateBody().Paragraph("Hi,").Paragraph("You are receiving this email because someone requested a password reset for your user account at" + appName + ".")
                .Button(options.Url, "Reset password").Paragraph("Thanks for  using " + appName + "!").Paragraph("ShopOwnerCore Support Team").ToString();

            return body;
        }

        public Task SendEmailAsync(string receiver, string message,EmailOptions options, EmailType emailType)
        {
            var email = new MimeMessage();
            var builder = new BodyBuilder();

            email.From.Add(new MailboxAddress(_settings.Value.SmtpSenderName, _settings.Value.SmtpAddress));
            email.To.Add(new MailboxAddress(receiver, receiver));

            switch (emailType)
            {
                case EmailType.AccountConfirm:
                    email.Subject = "Account confirm";
                    builder.HtmlBody = AccountCreationConfirm(options);
                    email.Body = builder.ToMessageBody();
                    break;
                case EmailType.ForgotPassword:
                    email.Subject = "Your account reset password";
                    builder.HtmlBody = AccountForgotPassword(options);
                    email.Body = builder.ToMessageBody();
                    break;
                default:
                    break;

                        
            }

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_settings.Value.SmtpHost, _settings.Value.SmtpPort,false);
                client.Authenticate(_settings.Value.SysEmail, _settings.Value.SmtpPassword);

                client.Send(email);
                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
