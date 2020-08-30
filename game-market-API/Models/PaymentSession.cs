using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace game_market_API.Models
{
    public class PaymentSession
    {
        [Key]
        public int ID { get; set; }
        
        public DateTime Date { get; set; }
        
        public virtual ICollection<GameKey> GameKeys { get; set; }

    }
}