using System.Net;
using System.Net.Mail;

namespace DianaApp.Services
{
    public static  class SendMailService
    {
        public static void SendEmail(string to, string name)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("maleykaheybatova1011@gmail.com", "nabl gtzb crzb fbla");
                client.EnableSsl = true;


                var mailMessage = new MailMessage()
                {
                    From = new MailAddress("maleykaheybatova1011@gmail.com"),
                    Subject = "Welcome to DianaApp",
                    Body = $"<h1>Welcome to our Website :) {name} </h1>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                client.Send(mailMessage);

            }
        }

    }
}
