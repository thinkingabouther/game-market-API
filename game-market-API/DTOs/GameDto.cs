using Microsoft.Build.Framework;

namespace game_market_API.DTOs
{
    public class GameDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Price { get; set; }
    }
}