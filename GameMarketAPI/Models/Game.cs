using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace game_market_API.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }
        
        public string Name { get; set; }

        public double Price { get; set; }
        
        public int VendorID { get; set; }
        public virtual User Vendor { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<GameKey> GameKeys { get; set; }
    }
}