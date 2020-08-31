using System.Threading.Tasks;
using game_market_API.Models;

namespace game_market_API.Services.NotifyingService
{
    public interface INotifyingService
    {
        public Task Notify(PaymentSession session);
    }
}