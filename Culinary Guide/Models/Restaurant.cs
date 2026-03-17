using System.Text.Json.Serialization;

namespace Culinary_Guide.Models
{
    public class Restaurant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public Dictionary<string, string> NameTranslations { get; set; } = new();
        
        [JsonPropertyName("address")]
        public Dictionary<string, string> AddressTranslations { get; set; } = new();
        
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
        public string CuisineTypeKey { get; set; } = "";
        
        [JsonPropertyName("description")]
        public Dictionary<string, string> DescriptionTranslations { get; set; } = new();
        
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = "";
        
        [JsonPropertyName("openingHours")]
        public string OpeningHours { get; set; } = "";
        
        public double Distance { get; set; }
        
        [JsonIgnore]
        public string Name { get; set; } = "";
        
        [JsonIgnore]
        public string Address { get; set; } = "";
        
        [JsonIgnore]
        public string Description { get; set; } = "";
        
        [JsonIgnore]
        public string CuisineType { get; set; } = "";
        
        [JsonIgnore]
        public string ReviewCountFormatted { get; set; } = "";
        
        public void ApplyLanguage(string languageCode, string reviewCountFormat)
        {
            Name = GetTranslation(NameTranslations, languageCode);
            Address = GetTranslation(AddressTranslations, languageCode);
            Description = GetTranslation(DescriptionTranslations, languageCode);
            ReviewCountFormatted = string.Format(reviewCountFormat, ReviewCount);
        }
        
        public void ApplyLanguage(string languageCode)
        {
            Name = GetTranslation(NameTranslations, languageCode);
            Address = GetTranslation(AddressTranslations, languageCode);
            Description = GetTranslation(DescriptionTranslations, languageCode);
        }
        
        private static string GetTranslation(Dictionary<string, string> translations, string languageCode)
        {
            if (translations.TryGetValue(languageCode, out var translation) && !string.IsNullOrEmpty(translation))
            {
                return translation;
            }
            
            if (translations.TryGetValue("zh-CN", out var fallback))
            {
                return fallback;
            }
            
            return translations.Values.FirstOrDefault() ?? "";
        }
    }
}