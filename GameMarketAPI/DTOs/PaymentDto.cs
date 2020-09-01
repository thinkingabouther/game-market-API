using Microsoft.Build.Framework;

namespace game_market_API.DTOs
{
    public class PaymentDto
    {
        [Required]
        public string CardNumber { get; set; }
    }
}