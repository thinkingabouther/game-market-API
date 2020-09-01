using System.ComponentModel.DataAnnotations;

namespace game_market_API.DTOs
{
    public class UserCredentialsDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool isVendor { get; set; }
    }
}