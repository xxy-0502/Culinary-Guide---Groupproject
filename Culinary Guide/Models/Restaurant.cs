using System.Text.Json.Serialization;

namespace Culinary_Guide.Models
{
    public class Restaurant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        
        [JsonPropertyName("address")]
        public string Address { get; set; } = "";
        
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        
        [JsonPropertyName("reviewCount")]
        public int ReviewCount { get; set; }
        
        [JsonPropertyName("imageUrls")]
        public List<string> ImageUrls { get; set; } = new();
        
        [JsonPropertyName("cuisineType")]
        public string CuisineType { get; set; } = "";
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = "";
        
        [JsonPropertyName("openingHours")]
        public string OpeningHours { get; set; } = "";
        
        public double Distance { get; set; }
    }
}
