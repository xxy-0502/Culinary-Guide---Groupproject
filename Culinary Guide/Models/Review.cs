namespace Culinary_Guide.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string UserName { get; set; } = "";
        public double Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public List<Reply> Replies { get; set; } = new();
    }

    public class Reply
    {
        public string UserName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
