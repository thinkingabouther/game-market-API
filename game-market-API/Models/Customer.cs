using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace game_market_API.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        
        public string EMail { get; set; }
        
        public string Password { get; set; }
        
        public virtual ICollection<GameKey> GameKeys { get; set; }
    }
}