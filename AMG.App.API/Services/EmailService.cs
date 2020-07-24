using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AMG.App.Infrastructure.Models.Settings;
using Microsoft.Extensions.Options;

namespace AMG.App.API.Services
{
    public class EmailService
    {
        private SmtpSettings smtpSettings;
        private EmailSettings emailSettings;
        public EmailService(IOptions<SmtpSettings> smtpSettings, IOptions<EmailSettings> emailSettings)
        {
            this.smtpSettings = smtpSettings.Value;
            this.emailSettings = emailSettings.Value;
        }
        public Task SendMail(string senderName, string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml)
        {
            senderName = senderName ?? "";
            from = from ?? "";
            to = to ?? "";
            cc = cc ?? "";
            bcc = bcc ?? "";
            subject = subject ?? "";
            body = body ?? "";
            MailMessage mailMessage = new MailMessage();
            if (string.IsNullOrWhiteSpace(senderName))
                mailMessage.From = new MailAddress(from);
            else
                mailMessage.From = new MailAddress(from, senderName);
            List<string> receivedUsers = to.Split(";").Where(ww => !string.IsNullOrWhiteSpace(ww)).ToList();
            foreach (var receivedUser in receivedUsers)
            {
                mailMessage.To.Add(receivedUser);
            }
            List<string> cCedUsers = cc.Split(";").Where(ww => !string.IsNullOrWhiteSpace(ww)).ToList();
            foreach (var cCedUser in cCedUsers)
            {
                mailMessage.CC.Add(cCedUser);
            }
            List<string> bCCedUsers = bcc.Split(";").Where(ww => !string.IsNullOrWhiteSpace(ww)).ToList();
            foreach (var bCCedUser in bCCedUsers)
            {
                mailMessage.CC.Add(bCCedUser);
            }
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isBodyHtml;
            using (var client = new SmtpClient(smtpSettings.Server))
            {
                client.Port = smtpSettings.Port;
                client.Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
                client.EnableSsl = smtpSettings.EnableSsl;
                client.Send(mailMessage);
            }
            return Task.CompletedTask;
        }
    }
}