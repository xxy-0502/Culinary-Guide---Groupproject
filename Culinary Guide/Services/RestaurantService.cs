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
        void UpdateLanguage(string languageCode);
        Task InitializeSampleDataAsync();
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly ILocationService _locationService;
        private readonly ILanguageService _languageService;
        private readonly DatabaseService _databaseService;
        private List<Restaurant> _restaurants = new();
        private Dictionary<int, (double Rating, int Count)> _restaurantStats = new();
        private string _currentLanguage = "en-US";

        public RestaurantService(ILocationService locationService, ILanguageService languageService, DatabaseService databaseService)
        {
            _locationService = locationService;
            _languageService = languageService;
            _databaseService = databaseService;
        }

        public async Task InitializeSampleDataAsync()
        {
            var count = await _databaseService.GetReviewCountAsync();
            if (count == 0)
            {
                var sampleReviews = new List<ReviewItem>
                {
                    new() { RestaurantId = 1, UserName = "小明", Rating = 5, Comment = "非常好吃，水煮鱼太赞了！", LikeCount = 12 },
                    new() { RestaurantId = 1, UserName = "小红", Rating = 4, Comment = "味道正宗，就是有点辣", LikeCount = 8 },
                    new() { RestaurantId = 1, UserName = "大厨", Rating = 5, Comment = "作为四川人表示很地道", LikeCount = 25 },
                    new() { RestaurantId = 2, UserName = "老广", Rating = 5, Comment = "早茶很正宗，虾饺必点", LikeCount = 15 },
                    new() { RestaurantId = 2, UserName = "美食家", Rating = 4, Comment = "环境好，味道也不错", LikeCount = 10 },
                    new() { RestaurantId = 3, UserName = "杭州人", Rating = 5, Comment = "西湖醋鱼做得很地道", LikeCount = 20 },
                    new() { RestaurantId = 4, UserName = "湖南伢子", Rating = 4, Comment = "够辣够味", LikeCount = 6 },
                    new() { RestaurantId = 5, UserName = "北京大爷", Rating = 5, Comment = "烤鸭皮脆肉嫩，正宗！", LikeCount = 30 },
                    new() { RestaurantId = 5, UserName = "游客", Rating = 4, Comment = "第一次吃烤鸭，很惊艳", LikeCount = 5 },
                };

                for (int i = 6; i <= 15; i++)
                {
                    sampleReviews.Add(new ReviewItem
                    {
                        RestaurantId = i,
                        UserName = "食客",
                        Rating = 4,
                        Comment = "味道不错，会再来",
                        LikeCount = 3
                    });
                }

                foreach (var review in sampleReviews)
                {
                    review.CreatedAt = DateTime.Now.AddDays(-new Random().Next(1, 7));
                    await _databaseService.AddReviewAsync(review);
                }
                
                Console.WriteLine($"已初始化 {sampleReviews.Count} 条示例评论");
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            if (_restaurants.Count == 0)
            {
                await LoadRestaurantsAsync();
            }

            _restaurantStats = await _databaseService.GetAllRestaurantStatsAsync();

            foreach (var restaurant in _restaurants)
            {
                if (_restaurantStats.TryGetValue(restaurant.Id, out var stats))
                {
                    restaurant.Rating = Math.Round(stats.Rating, 1);
                    restaurant.ReviewCount = stats.Count;
                }
            }

            var userLocation = await _locationService.GetUserLocationAsync();
            foreach (var restaurant in _restaurants)
            {
                restaurant.Distance = _locationService.CalculateDistance(
                    userLocation.Latitude, userLocation.Longitude,
                    restaurant.Latitude, restaurant.Longitude);
            }

            var languageCode = _languageService.CurrentLanguage == AppLanguage.Chinese ? "zh-CN" : "en-US";
            var reviewCountFormat = _languageService.GetString("ReviewCountTapFormat");
            foreach (var restaurant in _restaurants)
            {
                restaurant.ApplyLanguage(languageCode, reviewCountFormat);
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
                
                var languageCode = _languageService.CurrentLanguage == AppLanguage.Chinese ? "zh-CN" : "en-US";
                var reviewCountFormat = _languageService.GetString("ReviewCountTapFormat");
                foreach (var restaurant in _restaurants)
                {
                    restaurant.ApplyLanguage(languageCode, reviewCountFormat);
                    restaurant.CuisineType = GetCuisineTypeDisplay(restaurant.CuisineTypeKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading restaurants: {ex.Message}");
                _restaurants = new List<Restaurant>();
            }
        }

        public Task<Restaurant?> GetRestaurantByIdAsync(int id) =>
            Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));

        public async Task<List<Review>> GetReviewsAsync(int restaurantId)
        {
            var reviewItems = await _databaseService.GetReviewsAsync(restaurantId);
            return reviewItems.Select(r => new Review
            {
                Id = r.Id,
                RestaurantId = r.RestaurantId,
                UserName = r.UserName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                LikeCount = r.LikeCount
            }).ToList();
        }

        public async Task AddReviewAsync(Review review)
        {
            var reviewItem = new ReviewItem
            {
                RestaurantId = review.RestaurantId,
                UserName = review.UserName,
                Rating = review.Rating,
                Comment = review.Comment,
                LikeCount = 0
            };

            var id = await _databaseService.AddReviewAsync(reviewItem);
            review.Id = id;
            review.CreatedAt = reviewItem.CreatedAt;
            review.LikeCount = 0;

            var stats = await _databaseService.GetRestaurantStatsAsync(review.RestaurantId);
            if (stats != null)
            {
                var restaurant = _restaurants.FirstOrDefault(r => r.Id == review.RestaurantId);
                if (restaurant != null)
                {
                    restaurant.Rating = Math.Round(stats.AverageRating, 1);
                    restaurant.ReviewCount = stats.ReviewCount;
                    
                    var languageCode = _languageService.CurrentLanguage == AppLanguage.Chinese ? "zh-CN" : "en-US";
                    var reviewCountFormat = _languageService.GetString("ReviewCountTapFormat");
                    restaurant.ApplyLanguage(languageCode, reviewCountFormat);
                }
            }
            
            Console.WriteLine($"添加评论成功: Id={id}, RestaurantId={review.RestaurantId}, Rating={review.Rating}, 新平均评分={stats?.AverageRating:F1}");
        }

        public async Task AddLikeToReviewAsync(int reviewId)
        {
            var review = await _databaseService.GetReviewByIdAsync(reviewId);
            if (review != null)
            {
                review.LikeCount++;
                await _databaseService.UpdateReviewLikeAsync(reviewId, review.LikeCount);
                Console.WriteLine($"点赞成功: ReviewId={reviewId}, LikeCount={review.LikeCount}");
            }
        }

        public void UpdateLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
            var reviewCountFormat = _languageService.GetString("ReviewCountTapFormat");
            foreach (var restaurant in _restaurants)
            {
                restaurant.ApplyLanguage(languageCode, reviewCountFormat);
                restaurant.CuisineType = GetCuisineTypeDisplay(restaurant.CuisineTypeKey);
            }
        }

        private string GetCuisineTypeDisplay(string key)
        {
            return key?.ToLower() switch
            {
                "sichuan" => _languageService.GetString("Cuisine_Sichuan"),
                "cantonese" => _languageService.GetString("Cuisine_Cantonese"),
                "zhejiang" => _languageService.GetString("Cuisine_Zhejiang"),
                "hunan" => _languageService.GetString("Cuisine_Hunan"),
                "beijing" => _languageService.GetString("Cuisine_Beijing"),
                "shandong" => _languageService.GetString("Cuisine_Shandong"),
                "western" => _languageService.GetString("Cuisine_Western"),
                _ => key ?? ""
            };
        }
    }
}