using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Consumer.FailurePostProcessors;
using Consumer.Requesters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    static class Program
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            ;
            var host = Environment.GetEnvironmentVariable("CLOUDAMQP_URL");
            if (string.IsNullOrEmpty(host)) host = config["Settings:RabbitMQCredentials:Host"];
            var exchange = config["Settings:RabbitMQCredentials:Exchange"];
            var retryExchange = exchange + ".retry";
            var queue = config["Settings:RabbitMQCredentials:Queue"];
            var retryQueue = queue + ".retry";
            var routingKey = config["Settings:RabbitMQCredentials:RoutingKey"];
            var postProcessor = new RabbitMessageRePublisher(host, retryExchange, retryQueue, exchange, routingKey);
            var requester = new PostRequester(postProcessor);
            using var consumer = new MessageConsumer(host, exchange, queue, routingKey, requester);
            consumer.Initialize();
            Console.ReadLine();
        }

    }
}