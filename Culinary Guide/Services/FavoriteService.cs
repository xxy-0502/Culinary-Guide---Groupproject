namespace Culinary_Guide.Services
{
    public interface IFavoriteService
    {
        bool IsFavorite(int restaurantId);
        void ToggleFavorite(int restaurantId);
        List<int> GetFavoriteIds();
    }

    public class InMemoryFavoriteService : IFavoriteService
    {
        private readonly HashSet<int> _favorites = new();

        public bool IsFavorite(int restaurantId) => _favorites.Contains(restaurantId);

        public void ToggleFavorite(int restaurantId)
        {
            if (_favorites.Contains(restaurantId))
                _favorites.Remove(restaurantId);
            else
                _favorites.Add(restaurantId);
        }

        public List<int> GetFavoriteIds() => _favorites.ToList();
    }
}
