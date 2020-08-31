using game_market_API.Models;

namespace game_market_API.Services.ClientNotifyingService
{
    public interface INotifyingService
    {
        public void Notify(PaymentSession session);
    }
}