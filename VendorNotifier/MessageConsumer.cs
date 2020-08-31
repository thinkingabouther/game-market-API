using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Consumer.Requesters;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class MessageConsumer : IDisposable
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        
        public string HostName { get; set; }

        public IRequester Requester { get; set; }

        public int Delay = 10000;

        private IModel _channel;

        public MessageConsumer(string hostName, string exchangeName, string queueName, string routingKey, IRequester requester)
        {
            HostName = hostName;
            ExchangeName = exchangeName;
            QueueName = queueName;
            RoutingKey = routingKey;
            Requester = requester;
            InitializeExchange();
        }
        private void InitializeExchange()
        {
            _channel = new ConnectionFactory{HostName = HostName}.CreateConnection().CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
        }

        public void Initialize()
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, args) =>
            {
                var message = GetMessageModelFromDelivery(args);
                Requester.TryRequest(message);
            };
            _channel.BasicConsume(QueueName, true, consumer);
            Console.WriteLine($"VendorNotifier ready! Listening to exchange \"{ExchangeName}\", queue \"{QueueName}\" with routing key \"{RoutingKey}\"");
        }

        private Message GetMessageModelFromDelivery(BasicDeliverEventArgs eventArgs)
        {
            byte[] body = eventArgs.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            return JsonConvert.DeserializeObject<Message>(messageString);
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}