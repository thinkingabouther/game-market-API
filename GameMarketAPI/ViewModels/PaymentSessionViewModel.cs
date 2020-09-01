using System;

namespace game_market_API.ViewModels
{
    public class PaymentSessionViewModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Client { get; set; }
        public string Game { get; set; }
        public int KeysCount { get; set; }
        public double Total { get; set; }
    }
}