using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace game_market_API.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }
        
        [Microsoft.Build.Framework.Required]
        public string Name { get; set; }

        [Microsoft.Build.Framework.Required]
        public double Price { get; set; }
        
        public virtual ICollection<GameKey> GameKeys { get; set; }
    }
}