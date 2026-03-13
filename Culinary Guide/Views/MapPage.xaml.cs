using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private readonly List<Restaurant> _allRestaurants;
        private RestaurantMapDrawable _mapDrawable;

        public MapPage(IRestaurantService restaurantService, IFavoriteService favoriteService, List<Restaurant> allRestaurants)
        {
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            _allRestaurants = allRestaurants;
            InitializeComponent();
            SetupMap();
        }

        private void SetupMap()
        {
            _mapDrawable = new RestaurantMapDrawable(_allRestaurants);
            MapView.Drawable = _mapDrawable;
            BindingContext = new MapViewModel(_allRestaurants);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MapView.Invalidate();
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
            var favoritesPage = new FavoritesPage(_restaurantService, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(favoritesPage);
        }

        private async void OnHomeTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void OnExploreTabClicked(object sender, EventArgs e)
        {
        }
    }

    public class MapViewModel
    {
        public int RestaurantCount { get; set; }
        public bool HasRestaurants => RestaurantCount > 0;

        public MapViewModel(List<Restaurant> restaurants)
        {
            RestaurantCount = restaurants.Count;
        }
    }

    public class RestaurantMapDrawable : IDrawable
    {
        private readonly List<Restaurant> _restaurants;
        private readonly Random _random = new(42);

        public RestaurantMapDrawable(List<Restaurant> restaurants)
        {
            _restaurants = restaurants;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawMapBackground(canvas, dirtyRect);
            DrawGridLines(canvas, dirtyRect);
            DrawUserLocation(canvas, dirtyRect);
            DrawRestaurants(canvas, dirtyRect);
        }

        private void DrawMapBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Color.FromRgba(232, 245, 233, 255);
            canvas.FillRectangle(dirtyRect);
        }

        private void DrawGridLines(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Color.FromRgba(200, 230, 200, 100);
            canvas.StrokeSize = 1;

            float gridSize = 40;
            for (float x = 0; x < dirtyRect.Width; x += gridSize)
            {
                canvas.DrawLine(x, 0, x, dirtyRect.Height);
            }
            for (float y = 0; y < dirtyRect.Height; y += gridSize)
            {
                canvas.DrawLine(0, y, dirtyRect.Width, y);
            }
        }

        private void DrawUserLocation(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Width / 2;
            float centerY = dirtyRect.Height / 2;

            canvas.FillColor = Color.FromRgba(66, 133, 244, 80);
            canvas.FillCircle(centerX, centerY, 30);

            canvas.FillColor = Color.FromRgba(66, 133, 244, 255);
            canvas.FillCircle(centerX, centerY, 12);

            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 3;
            canvas.DrawCircle(centerX, centerY, 12);
        }

        private void DrawRestaurants(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Width / 2;
            float centerY = dirtyRect.Height / 2;

            for (int i = 0; i < _restaurants.Count; i++)
            {
                var restaurant = _restaurants[i];
                
                float angle = (float)(i * 2 * Math.PI / _restaurants.Count);
                float radius = 80 + (float)_random.NextDouble() * 100;
                
                float x = centerX + (float)Math.Cos(angle) * radius;
                float y = centerY + (float)Math.Sin(angle) * radius;

                x = Math.Max(30, Math.Min(dirtyRect.Width - 30, x));
                y = Math.Max(30, Math.Min(dirtyRect.Height - 30, y));

                canvas.FillColor = Color.FromRgba(255, 183, 197, 200);
                canvas.FillCircle(x, y, 18);

                canvas.FillColor = Color.FromRgba(255, 183, 197, 255);
                canvas.FillCircle(x, y, 14);

                canvas.FontColor = Colors.White;
                canvas.FontSize = 10;
                canvas.DrawString("🍴", x - 7, y + 4, HorizontalAlignment.Left);
            }
        }
    }
}