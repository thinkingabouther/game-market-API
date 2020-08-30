using System.ComponentModel.DataAnnotations;

namespace game_market_API.DTOs
{
    public class GameKeyRequest
    {
        
        [Required]
        public string Key { get; set; }
        
        [Required]
        public int GameID { get; set; }
        
    }
}