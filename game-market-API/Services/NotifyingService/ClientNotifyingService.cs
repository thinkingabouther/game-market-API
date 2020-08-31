using System;
using game_market_API.Models;

namespace game_market_API.Services.ClientNotifyingService
{
    public class ClientNotifyingService : INotifyingService
    {
        public void Notify(PaymentSession session)
        {
            Console.WriteLine("Hello from client");
        }
    }
}