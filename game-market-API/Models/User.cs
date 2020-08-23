using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace game_market_API.Models
{
    public enum Role
    {
        Vendor,
        Client
    }
    public class User
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public Role Role { get; set; }
    }
}