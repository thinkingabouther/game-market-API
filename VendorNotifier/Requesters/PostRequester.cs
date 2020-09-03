using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Consumer.DTOs;
using Consumer.FailurePostProcessors;
using Newtonsoft.Json;

namespace Consumer.Requesters
{
    public class PostRequester : IRequester
    {
        public PostRequester(){}
        public PostRequester(IFailurePostProcessor postProcessor)
        {
            PostProcessor = postProcessor;
        }

        private IFailurePostProcessor PostProcessor { get;  }
        public async Task<bool> TryRequest(Message message)
        {
            HttpClient client = new HttpClient();
            var requestBody = GetRequestBody(message);
            var data = new StringContent(requestBody, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("Hash", message.Hash);
            var responseMessage = await client.PostAsync(message.VendorUrl, data);
            if (CheckStatusCode(responseMessage))
            {
                Console.WriteLine(" Message was acknowledged");
                Console.WriteLine(" [Response] {0}", responseMessage);
                return true;
            }
            Console.WriteLine(" Message was acknowledged negatively");
            PostProcessor?.Process(message);
            return false;
        }

        private string GetRequestBody(Message message)
        {
            var body = new
            {
                GameName = message.GameName,
                KeysCount = message.KeysCount,
                Take = message.Take,
                Commission = message.Commission
            };
            return JsonConvert.SerializeObject(body);
        }
        private static bool CheckStatusCode(HttpResponseMessage responseMessage)
        {
            var responseSuccessfulPattern = @"20.";
            return Regex.IsMatch(((int)responseMessage.StatusCode).ToString(), responseSuccessfulPattern);
        }
    }
}