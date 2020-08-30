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
        
        [JsonIgnore]
        public virtual ICollection<GameKey> GameKeys { get; set; }
    }
}