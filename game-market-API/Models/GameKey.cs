using System.ComponentModel.DataAnnotations;

namespace game_market_API.Models
{
    public class GameKey
    {
        [Key]
        public int ID { get; set; }
        
        public string Key { get; set; }
        
        public bool IsActivated { get; set; }
        
        public virtual Game Game { get; set; }
        
        public virtual User Client { get; set; }
        
    }
}