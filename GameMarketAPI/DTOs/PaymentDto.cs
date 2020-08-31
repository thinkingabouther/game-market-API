using Microsoft.Build.Framework;

namespace game_market_API.DTOs
{
    public class PaymentDto
    {
        [Required]
        public int SessionID { get; set; }
        
        [Required]
        public string CardNumber { get; set; }
    }
}