using System.Text.Json.Serialization;

namespace game_market_API.DTOs
{
    public class Message
    {
        [JsonPropertyName("vendor-url")]
        public string VendorUrl { get; set; }

        [JsonPropertyName("republish-count")]
        public int RepublishCount { get; set; } = 0;
        
        [JsonPropertyName("game-name")]
        public string GameName { get; set; }
        
        [JsonPropertyName("keys-count")]
        public int KeysCount { get; set; }
        
        [JsonPropertyName("take")]
        public double Take { get; set; }
        
        [JsonPropertyName("commission")]
        public double Commission { get; set; }
        
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}