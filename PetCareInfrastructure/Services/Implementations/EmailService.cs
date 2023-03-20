using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string emailTo, string Subject, string Body)
        {
            // Content Message
            var Message = new MailMessage();
            Message.From = new MailAddress("petcompanionsystem@gmail.com", "Pet Care");
            Message.Subject = Subject;
            Message.Body = Body;
            Message.IsBodyHtml = true;
            foreach (var email in emailTo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                Message.To.Add(email);

            // Credentails (Authentication Project With MY Email)
            var EamilClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true, // Security Layer To Send Mail
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("petcompanionsystem@gmail.com", "nyqffqqskakqfjqj")
            };
            await EamilClient.SendMailAsync(Message);
        }
    }
}
