using Culinary_Guide.Helpers;
using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private readonly ILocationService _locationService;
        private readonly ILanguageService _languageService;
        private readonly List<Restaurant> _allRestaurants;
        private RestaurantMapDrawable _mapDrawable;
        private double _currentLatitude = 39.9087;
        private double _currentLongitude = 116.3975;
        private bool _isRealLocation = false;

        public MapPage(
            IRestaurantService restaurantService, 
            IFavoriteService favoriteService,
            ILocationService locationService,
            List<Restaurant> allRestaurants)
        {
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            _locationService = locationService;
            _languageService = MauiProgram.Services?.GetRequiredService<ILanguageService>()!;
            _allRestaurants = allRestaurants;
            InitializeComponent();
            BindingContext = Localize.Instance;
            SetupMap();
        }

        private void SetupMap()
        {
            _mapDrawable = new RestaurantMapDrawable(_allRestaurants, _currentLatitude, _currentLongitude, _isRealLocation);
            MapView.Drawable = _mapDrawable;
            UpdateLabels();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged += OnLanguageChanged;
            }
            Localize.Instance.Invalidate();
            
            await GetRealLocationAsync();
            
            MapView.Invalidate();
            UpdateLabels();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged -= OnLanguageChanged;
            }
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            Localize.Instance.Invalidate();
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            var loc = Localize.Instance;
            RestaurantCountLabel.Text = $"{_allRestaurants.Count} {loc.RestaurantCount}";
            NoRestaurantsLayout.IsVisible = _allRestaurants.Count == 0;
            
            if (_isRealLocation)
            {
                LocationInfoLabel.Text = $"{loc.Latitude}: {_currentLatitude:F6}\n{loc.Longitude}: {_currentLongitude:F6}";
                LocationInfoLabel.IsVisible = true;
            }
        }

        private async Task GetRealLocationAsync()
        {
            try
            {
                var loc = Localize.Instance;
                LocationInfoLabel.Text = loc.GettingLocation;
                LocationInfoLabel.IsVisible = true;

                var location = await _locationService.GetUserLocationAsync();
                
                _currentLatitude = location.Latitude;
                _currentLongitude = location.Longitude;
                _isRealLocation = true;

                _mapDrawable = new RestaurantMapDrawable(_allRestaurants, _currentLatitude, _currentLongitude, _isRealLocation);
                MapView.Drawable = _mapDrawable;
                MapView.Invalidate();

                LocationInfoLabel.Text = $"{loc.Latitude}: {_currentLatitude:F6}\n{loc.Longitude}: {_currentLongitude:F6}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取位置失败: {ex.Message}");
            }
        }

        private async void OnMyLocationClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert(loc.Language, loc.LocationPermissionRequired, loc.OK);
                    return;
                }

                LocationInfoLabel.Text = loc.GettingLocation;
                LocationInfoLabel.IsVisible = true;

                var location = await _locationService.GetUserLocationAsync();
                
                _currentLatitude = location.Latitude;
                _currentLongitude = location.Longitude;
                _isRealLocation = true;

                _mapDrawable = new RestaurantMapDrawable(_allRestaurants, _currentLatitude, _currentLongitude, _isRealLocation);
                MapView.Drawable = _mapDrawable;
                MapView.Invalidate();

                LocationInfoLabel.Text = $"{loc.Latitude}: {location.Latitude:F6}\n{loc.Longitude}: {location.Longitude:F6}";

                await DisplayAlert(loc.CurrentLocation, 
                    $"{loc.Latitude}: {location.Latitude:F6}\n{loc.Longitude}: {location.Longitude:F6}", 
                    loc.OK);
            }
            catch (Exception ex)
            {
                LocationInfoLabel.IsVisible = false;
                await DisplayAlert(loc.Error, $"{loc.GetLocationFailed}: {ex.Message}", loc.OK);
            }
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
            var favoritesPage = new FavoritesPage(_restaurantService, _favoriteService, _locationService, _allRestaurants);
            await Navigation.PushAsync(favoritesPage);
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnHomeTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void OnExploreTabClicked(object sender, EventArgs e)
        {
        }
    }

    public class RestaurantMapDrawable : IDrawable
    {
        private readonly List<Restaurant> _restaurants;
        private readonly Random _random = new(42);
        private readonly double _userLat;
        private readonly double _userLon;
        private readonly bool _isRealLocation;

        public RestaurantMapDrawable(List<Restaurant> restaurants, double userLat, double userLon, bool isRealLocation)
        {
            _restaurants = restaurants;
            _userLat = userLat;
            _userLon = userLon;
            _isRealLocation = isRealLocation;
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

            canvas.FillColor = _isRealLocation 
                ? Color.FromRgba(76, 175, 80, 80) 
                : Color.FromRgba(66, 133, 244, 80);
            canvas.FillCircle(centerX, centerY, 30);

            canvas.FillColor = _isRealLocation 
                ? Color.FromRgba(76, 175, 80, 255) 
                : Color.FromRgba(66, 133, 244, 255);
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