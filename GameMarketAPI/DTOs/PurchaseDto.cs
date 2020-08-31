using Microsoft.Build.Framework;

namespace game_market_API.DTOs
{
    public class PurchaseDto
    {
        [Required]
        public int GameID { get; set; }
        
        [Required]
        public int GameCount { get; set; }
        
    }
}