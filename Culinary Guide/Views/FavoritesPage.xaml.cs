using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private readonly List<Restaurant> _allRestaurants;
        private List<Restaurant> _favoriteRestaurants = new();

        public FavoritesPage(IRestaurantService restaurantService, IFavoriteService favoriteService, List<Restaurant> allRestaurants)
        {
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            _allRestaurants = allRestaurants;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            var favoriteIds = _favoriteService.GetFavoriteIds();
            _favoriteRestaurants = _allRestaurants
                .Where(r => favoriteIds.Contains(r.Id))
                .ToList();
            FavoritesList.ItemsSource = _favoriteRestaurants;
        }

        private void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Border border && border.BindingContext is Restaurant restaurant)
            {
                _favoriteService.ToggleFavorite(restaurant.Id);
                LoadFavorites();
            }
        }

        private async void OnRestaurantSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Restaurant restaurant)
            {
                var detailPage = new RestaurantDetailPage(_restaurantService, _favoriteService, restaurant);
                await Navigation.PushAsync(detailPage);
                FavoritesList.SelectedItem = null;
            }
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
        }

        private async void OnHomeTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnExploreTabClicked(object sender, EventArgs e)
        {
            var mapPage = new MapPage(_restaurantService, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(mapPage);
        }
    }
}