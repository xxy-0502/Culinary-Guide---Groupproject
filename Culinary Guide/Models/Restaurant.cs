namespace Culinary_Guide.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public string CuisineType { get; set; } = "";
        public string Description { get; set; } = "";
        public string Phone { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        
        public double Distance { get; set; }
    }
}
