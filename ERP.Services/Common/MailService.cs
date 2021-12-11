using ERP.Models.Common;
using ERP.Services.Common.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Threading.Tasks;

namespace ERP.Services.Common
{
    public class MailService : IMailService
    {
        private readonly MailSettingsModel _mailSettingsModel;

        public MailService(IOptions<MailSettingsModel> mailSettingsModel)
        {
            _mailSettingsModel = mailSettingsModel.Value;
        }

        public async Task SendEmailAsync(MailRequestModel mailRequestModel)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettingsModel.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequestModel.ToEmail));
            email.Subject = mailRequestModel.Subject;

            var builder = new BodyBuilder();

            if (mailRequestModel.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequestModel.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequestModel.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_mailSettingsModel.Host, _mailSettingsModel.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettingsModel.Mail, _mailSettingsModel.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

        public async Task SendWelcomeEmailAsync(WelcomeRequestModel request)
        {
            //string FilePath =  "\\MailTemplate\\WelcomeTemplate.html";
            string FilePath = Directory.GetCurrentDirectory() + "\\Areas\\MailTemplate\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettingsModel.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettingsModel.Host, _mailSettingsModel.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettingsModel.Mail, _mailSettingsModel.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
