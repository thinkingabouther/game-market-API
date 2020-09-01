namespace game_market_API.ViewModels
{
    public class GameKeyViewModel
    {
        public int ID { get; set; }
        
        public string Key { get; set; }
        
        public bool IsActivated { get; set; }
        
        public string Game { get; set; }
    }
}