using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using game_market_API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace game_market_API.Services.NotifyingService
{
    public class VendorNotifyingService : INotifyingService, IDisposable
    {
        private readonly IConfiguration _configuration;
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }

        private IModel _channel;

        public VendorNotifyingService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeExchange();
        }

        private void InitializeExchange()
        {
            var credentials = _configuration.GetSection("Settings").GetSection("RabbitMQCredentials");
            var host = credentials.GetSection("Host").Value;
            ExchangeName = credentials.GetSection("Exchange").Value;
            RoutingKey = credentials.GetSection("RoutingKey").Value;
            _channel = new ConnectionFactory{HostName = host}.CreateConnection().CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);
        }
        
        
        public void Dispose()
        {
            _channel?.Dispose();
        }

        public async Task Notify(PaymentSession session)
        {
            var messageModel = MapToMessage(session);
            var payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageModel));
            messageModel.Hash = GenerateHash(payload);
            var bodyWithHash = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageModel));
            await Task.Run(() => _channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey, basicProperties: null, body: bodyWithHash));
            Console.WriteLine(" [x] Sent {0}", messageModel);
        }

        private string GenerateHash(byte[] message)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] data = sha256Hash.ComputeHash(message);
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();        
        } 
        
        private Message MapToMessage(PaymentSession session)
        {
            var game = session.GameKeys.First().Game;
            var marketShare = double.Parse(_configuration.GetSection("Settings").GetSection("MarketShare").Value);
            var message = new Message();
            message.VendorUrl = game.Vendor.Username;
            message.GameName = game.Name;
            message.KeysCount = session.GameKeys.Count;
            message.Take = session.GameKeys.Count * game.Price * (1 - marketShare);
            message.Commission = marketShare;
            return message;
        }
    }
}