namespace Culinary_Guide.Services
{
    public interface IFavoriteService
    {
        bool IsFavorite(int restaurantId);
        void ToggleFavorite(int restaurantId);
        List<int> GetFavoriteIds();
        Task InitializeAsync();
    }

    public class FavoriteService : IFavoriteService
    {
        private readonly DatabaseService _database;
        private readonly HashSet<int> _cache = new();
        private bool _isInitialized = false;

        public FavoriteService(DatabaseService database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            
            var ids = await _database.GetFavoriteIdsAsync();
            foreach (var id in ids)
            {
                _cache.Add(id);
            }
            _isInitialized = true;
        }

        public bool IsFavorite(int restaurantId) => _cache.Contains(restaurantId);

        public void ToggleFavorite(int restaurantId)
        {
            if (_cache.Contains(restaurantId))
            {
                _cache.Remove(restaurantId);
                _ = _database.RemoveFavoriteAsync(restaurantId);
            }
            else
            {
                _cache.Add(restaurantId);
                _ = _database.AddFavoriteAsync(restaurantId);
            }
        }

        public List<int> GetFavoriteIds() => _cache.ToList();
    }
}