using SQLite;

namespace Culinary_Guide.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(
                FileSystem.Current.AppDataDirectory, 
                "culinaryguide.db3"
            );
            Console.WriteLine($"数据库路径: {dbPath}");
            _database = new SQLiteAsyncConnection(dbPath);
            
            _database.CreateTableAsync<FavoriteItem>().Wait();
            _database.CreateTableAsync<ReviewItem>().Wait();
            _database.CreateTableAsync<RestaurantStatsItem>().Wait();
            _database.CreateTableAsync<UserProfileItem>().Wait();
        }

        #region 收藏操作

        public async Task<List<int>> GetFavoriteIdsAsync()
        {
            var items = await _database.Table<FavoriteItem>().ToListAsync();
            return items.Select(f => f.RestaurantId).ToList();
        }

        public async Task AddFavoriteAsync(int restaurantId)
        {
            await _database.InsertAsync(new FavoriteItem { RestaurantId = restaurantId });
        }

        public async Task RemoveFavoriteAsync(int restaurantId)
        {
            var item = await _database.Table<FavoriteItem>()
                .Where(f => f.RestaurantId == restaurantId)
                .FirstOrDefaultAsync();
            if (item != null)
            {
                await _database.DeleteAsync(item);
            }
        }

        public async Task<bool> IsFavoriteAsync(int restaurantId)
        {
            var count = await _database.Table<FavoriteItem>()
                .Where(f => f.RestaurantId == restaurantId)
                .CountAsync();
            return count > 0;
        }

        #endregion

        #region 评价操作

        public async Task<List<ReviewItem>> GetReviewsAsync(int restaurantId)
        {
            return await _database.Table<ReviewItem>()
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> AddReviewAsync(ReviewItem review)
        {
            review.CreatedAt = DateTime.Now;
            var id = await _database.InsertAsync(review);
            
            await UpdateRestaurantStatsAsync(review.RestaurantId);
            
            return id;
        }

        public async Task UpdateReviewLikeAsync(int reviewId, int likeCount)
        {
            var review = await _database.Table<ReviewItem>()
                .Where(r => r.Id == reviewId)
                .FirstOrDefaultAsync();
            if (review != null)
            {
                review.LikeCount = likeCount;
                await _database.UpdateAsync(review);
            }
        }

        public async Task<int> GetReviewCountAsync()
        {
            return await _database.Table<ReviewItem>().CountAsync();
        }

        public async Task<int> GetReviewCountByRestaurantAsync(int restaurantId)
        {
            return await _database.Table<ReviewItem>()
                .Where(r => r.RestaurantId == restaurantId)
                .CountAsync();
        }

        public async Task<ReviewItem?> GetReviewByIdAsync(int id)
        {
            return await _database.Table<ReviewItem>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region 餐厅统计（动态评分）

        public async Task<RestaurantStatsItem?> GetRestaurantStatsAsync(int restaurantId)
        {
            return await _database.Table<RestaurantStatsItem>()
                .Where(s => s.RestaurantId == restaurantId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateRestaurantStatsAsync(int restaurantId)
        {
            var reviews = await _database.Table<ReviewItem>()
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();

            var stats = await GetRestaurantStatsAsync(restaurantId);
            
            if (reviews.Count == 0)
            {
                if (stats != null)
                {
                    await _database.DeleteAsync(stats);
                }
                return;
            }

            var averageRating = reviews.Average(r => r.Rating);
            var reviewCount = reviews.Count;

            if (stats == null)
            {
                stats = new RestaurantStatsItem
                {
                    RestaurantId = restaurantId,
                    AverageRating = averageRating,
                    ReviewCount = reviewCount
                };
                await _database.InsertAsync(stats);
            }
            else
            {
                stats.AverageRating = averageRating;
                stats.ReviewCount = reviewCount;
                await _database.UpdateAsync(stats);
            }

            Console.WriteLine($"更新餐厅统计: RestaurantId={restaurantId}, Rating={averageRating:F1}, Reviews={reviewCount}");
        }

        public async Task<Dictionary<int, (double Rating, int Count)>> GetAllRestaurantStatsAsync()
        {
            var stats = await _database.Table<RestaurantStatsItem>().ToListAsync();
            return stats.ToDictionary(
                s => s.RestaurantId, 
                s => (s.AverageRating, s.ReviewCount)
            );
        }

        #endregion

        #region 用户资料

        public async Task<UserProfileItem?> GetUserProfileAsync()
        {
            return await _database.Table<UserProfileItem>().FirstOrDefaultAsync();
        }

        public async Task SaveUserProfileAsync(UserProfileItem profile)
        {
            profile.UpdatedAt = DateTime.Now;
            
            var existing = await GetUserProfileAsync();
            if (existing == null)
            {
                profile.CreatedAt = DateTime.Now;
                await _database.InsertAsync(profile);
            }
            else
            {
                profile.Id = existing.Id;
                profile.CreatedAt = existing.CreatedAt;
                await _database.UpdateAsync(profile);
            }
        }

        #endregion
    }

    [Table("Favorites")]
    public class FavoriteItem
    {
        [PrimaryKey]
        public int RestaurantId { get; set; }
    }

    [Table("Reviews")]
    public class ReviewItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string UserName { get; set; } = "";
        public double Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
    }

    [Table("RestaurantStats")]
    public class RestaurantStatsItem
    {
        [PrimaryKey]
        public int RestaurantId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfileItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nickname { get; set; } = "";
        public string Bio { get; set; } = "";
        public string? AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}