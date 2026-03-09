using Culinary_Guide.Models;
using System.Text.Json;

namespace Culinary_Guide.Services
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetRestaurantsAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<List<Review>> GetReviewsAsync(int restaurantId);
        Task AddReviewAsync(Review review);
        Task AddLikeToReviewAsync(int reviewId);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly ILocationService _locationService;
        private List<Restaurant> _restaurants = new();
        private Dictionary<int, List<Review>> _reviews = new();

        public RestaurantService(ILocationService locationService)
        {
            _locationService = locationService;
            InitializeReviews();
        }

        private void InitializeReviews()
        {
            _reviews = new Dictionary<int, List<Review>>
            {
                [1] = new List<Review>
                {
                    new Review { Id = 1, RestaurantId = 1, UserName = "小明", Rating = 5, Comment = "非常好吃，水煮鱼太赞了！", CreatedAt = DateTime.Now.AddDays(-1), LikeCount = 12 },
                    new Review { Id = 2, RestaurantId = 1, UserName = "小红", Rating = 4, Comment = "味道正宗，就是有点辣", CreatedAt = DateTime.Now.AddDays(-3), LikeCount = 8 },
                    new Review { Id = 3, RestaurantId = 1, UserName = "大厨", Rating = 5, Comment = "作为四川人表示很地道", CreatedAt = DateTime.Now.AddDays(-5), LikeCount = 25 }
                },
                [2] = new List<Review>
                {
                    new Review { Id = 4, RestaurantId = 2, UserName = "老广", Rating = 5, Comment = "早茶很正宗，虾饺必点", CreatedAt = DateTime.Now.AddDays(-2), LikeCount = 15 },
                    new Review { Id = 5, RestaurantId = 2, UserName = "美食家", Rating = 4, Comment = "环境好，味道也不错", CreatedAt = DateTime.Now.AddDays(-4), LikeCount = 10 }
                },
                [3] = new List<Review>
                {
                    new Review { Id = 6, RestaurantId = 3, UserName = "杭州人", Rating = 5, Comment = "西湖醋鱼做得很地道", CreatedAt = DateTime.Now.AddDays(-1), LikeCount = 20 }
                },
                [4] = new List<Review>
                {
                    new Review { Id = 7, RestaurantId = 4, UserName = "湖南伢子", Rating = 4, Comment = "够辣够味", CreatedAt = DateTime.Now.AddDays(-2), LikeCount = 6 }
                },
                [5] = new List<Review>
                {
                    new Review { Id = 8, RestaurantId = 5, UserName = "北京大爷", Rating = 5, Comment = "烤鸭皮脆肉嫩，正宗！", CreatedAt = DateTime.Now.AddDays(-1), LikeCount = 30 },
                    new Review { Id = 9, RestaurantId = 5, UserName = "游客", Rating = 4, Comment = "第一次吃烤鸭，很惊艳", CreatedAt = DateTime.Now.AddDays(-3), LikeCount = 5 }
                }
            };

            for (int i = 6; i <= 15; i++)
            {
                _reviews[i] = new List<Review>
                {
                    new Review { Id = i * 10 + 1, RestaurantId = i, UserName = "食客", Rating = 4, Comment = "味道不错，会再来", CreatedAt = DateTime.Now.AddDays(-2), LikeCount = 3 }
                };
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            if (_restaurants.Count == 0)
            {
                await LoadRestaurantsAsync();
            }

            var userLocation = await _locationService.GetUserLocationAsync();
            foreach (var restaurant in _restaurants)
            {
                restaurant.Distance = _locationService.CalculateDistance(
                    userLocation.Latitude, userLocation.Longitude,
                    restaurant.Latitude, restaurant.Longitude);
            }

            return _restaurants;
        }

        private async Task LoadRestaurantsAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("restaurants.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                _restaurants = JsonSerializer.Deserialize<List<Restaurant>>(json) ?? new List<Restaurant>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading restaurants: {ex.Message}");
                _restaurants = new List<Restaurant>();
            }
        }

        public Task<Restaurant?> GetRestaurantByIdAsync(int id) =>
            Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));

        public Task<List<Review>> GetReviewsAsync(int restaurantId) =>
            Task.FromResult(_reviews.GetValueOrDefault(restaurantId, new List<Review>()));

        public Task AddReviewAsync(Review review)
        {
            if (!_reviews.ContainsKey(review.RestaurantId))
                _reviews[review.RestaurantId] = new List<Review>();

            review.Id = _reviews[review.RestaurantId].Max(r => r.Id) + 1;
            review.CreatedAt = DateTime.Now;
            _reviews[review.RestaurantId].Add(review);
            return Task.CompletedTask;
        }

        public Task AddLikeToReviewAsync(int reviewId)
        {
            foreach (var reviews in _reviews.Values)
            {
                var review = reviews.FirstOrDefault(r => r.Id == reviewId);
                if (review != null)
                {
                    review.LikeCount++;
                    break;
                }
            }
            return Task.CompletedTask;
        }
    }
}
