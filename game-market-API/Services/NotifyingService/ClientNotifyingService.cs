using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using game_market_API.Models;
using Microsoft.Extensions.Configuration;

namespace game_market_API.Services.NotifyingService
{
    public class ClientNotifyingService : INotifyingService
    {
        private readonly IConfiguration _configuration;
        
        public ClientNotifyingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Notify(PaymentSession session)
        {
            var credentials = _configuration.GetSection("Settings").GetSection("MarketMailCredentials");
            string username = credentials.GetSection("Username").Value;
            string password = credentials.GetSection("Password").Value;
            var message = ConfigureMessage(username, session);
            var client = ConfigureClient(username, password);
            await Task.Run(() => client.Send(message));
        }

        private MailMessage ConfigureMessage(string username, PaymentSession session)
        {
            MailAddress from = new MailAddress(username, "GameKeys Store");
            MailAddress to = new MailAddress(session.Client.Username);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Purchase confirmation";
            m.Body = GenerateMessage(session);
            return m;
        }

        private SmtpClient ConfigureClient(string username, string password)
        {
            var smtp = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            return smtp;
        }
        private string GenerateMessage(PaymentSession session)
        {
            var generateMessage = $"Hello!\nYou've successfully paid for your order №{session.ID}\nYour keys are:\n";
            foreach (GameKey gameKey in session.GameKeys)
            {
                generateMessage += gameKey.Key + "\n";
            }
            return generateMessage;
        }
    }
}