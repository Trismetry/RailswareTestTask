using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RailswareTestTask.Configuration;
using RailswareTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RailswareTestTask
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public async Task<bool> SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {

                    var emailFrom = Helper.FindEmail(mailData.SenderNameAndEmail);
                    if (emailFrom == null)                    
                        return false;

                    var emailTo = Helper.FindEmail(mailData.RecipientNameAndEmail);
                    if (emailTo == null)
                        return false;

                    var emailFromName = mailData.SenderNameAndEmail.Replace(emailFrom, string.Empty).Trim();
                    var emailToName = mailData.RecipientNameAndEmail.Replace(emailTo, string.Empty).Trim();

                    MailboxAddress mailBoxFrom = new MailboxAddress(emailFromName, emailFrom);
                    emailMessage.From.Add(mailBoxFrom);
                    MailboxAddress mailBoxTo = new MailboxAddress(emailToName, emailTo);
                    emailMessage.To.Add(mailBoxTo);
                                       
                    emailMessage.Subject = mailData.Subject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.Body;
                    emailBodyBuilder.HtmlBody = mailData.HTML;
                    if (mailData.Attachments != null)
                    {
                        foreach (var attachmentFile in mailData.Attachments)
                        {
                            if (attachmentFile.Length == 0)
                            {
                                continue;
                            }

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                attachmentFile.CopyTo(memoryStream);
                                var attachmentFileByteArray = memoryStream.ToArray();

                                emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
                            }
                        }
                    }

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //send error
                return false;
            }
        }
    }
}
