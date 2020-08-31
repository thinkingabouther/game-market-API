using System;
using System.Threading.Tasks;
using game_market_API.Models;

namespace game_market_API.Services.NotifyingService
{
    public class VendorNotifyingService : INotifyingService
    {
        public async Task Notify(PaymentSession session)
        {
            Console.WriteLine("Hello from vendor");
        }
    }
}