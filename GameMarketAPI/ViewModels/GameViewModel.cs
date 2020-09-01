namespace game_market_API.ViewModels
{
    public class GameViewModel
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public double Price { get; set; }
        
        public string Vendor { get; set; }
        
        public int AvailableKeysCount { get; set; }
    }
}