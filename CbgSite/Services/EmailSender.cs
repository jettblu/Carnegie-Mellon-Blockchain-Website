using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CbgSite.Services
{
    public class EmailSender:IEmailSender
    {
        private string apiKey;
        private string fromAddress;
        private string fromDisplayName;
        public EmailSender(string key, string fromEmail, string fromName)
        {
            apiKey = key;
            fromAddress = fromEmail;
            fromDisplayName = fromName;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromAddress, fromDisplayName),
                Subject = subject,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
