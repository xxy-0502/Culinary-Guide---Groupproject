namespace Culinary_Guide.Models
{
    public class UserProfile
    {
        public string Nickname { get; set; } = "";
        public string Bio { get; set; } = "";
        public string? AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}