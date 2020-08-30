using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace game_market_API.Models
{
    public class User
    {
        public const string VendorRole = "Vendor";
        public const string ClientRole = "Client";
        public const string AdminRole = "Admin";
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }
    }
}