using Culinary_Guide.Models;
using Culinary_Guide.Services;
using Culinary_Guide.Views;

namespace Culinary_Guide
{
    public partial class MainPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private List<Restaurant> _allRestaurants = new();
        private SortOption _currentSort = SortOption.Distance;
        private TabType _currentTab = TabType.Home;

        public MainPage(IRestaurantService restaurantService, IFavoriteService favoriteService)
        {
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadRestaurants();
            UpdateTabStyles();
        }

        private async Task LoadRestaurants()
        {
            try
            {
                _allRestaurants = await _restaurantService.GetRestaurantsAsync();
                ApplySort();
            }
            catch (Exception ex)
            {
                await DisplayAlert("错误", $"加载失败：{ex.Message}", "确定");
            }
        }

        private void ApplySort()
        {
            var sorted = _currentSort switch
            {
                SortOption.Distance => _allRestaurants.OrderBy(r => r.Distance).ToList(),
                SortOption.Rating => _allRestaurants.OrderByDescending(r => r.Rating).ToList(),
                SortOption.Reviews => _allRestaurants.OrderByDescending(r => r.ReviewCount).ToList(),
                _ => _allRestaurants
            };
            RestaurantList.ItemsSource = sorted;
        }

        private void OnSearch(object sender, EventArgs e)
        {
            var query = SearchBar.Text?.ToLower() ?? "";
            if (string.IsNullOrWhiteSpace(query))
            {
                ApplySort();
                return;
            }

            var filtered = _allRestaurants
                .Where(r => r.Name.ToLower().Contains(query) || 
                           r.CuisineType.ToLower().Contains(query))
                .ToList();

            RestaurantList.ItemsSource = filtered;
        }

        private void OnDistanceSort(object sender, EventArgs e)
        {
            _currentSort = SortOption.Distance;
            UpdateSortButtons();
            ApplySort();
        }

        private void OnRatingSort(object sender, EventArgs e)
        {
            _currentSort = SortOption.Rating;
            UpdateSortButtons();
            ApplySort();
        }

        private void OnReviewsSort(object sender, EventArgs e)
        {
            _currentSort = SortOption.Reviews;
            UpdateSortButtons();
            ApplySort();
        }

        private void UpdateSortButtons()
        {
            var primaryColor = Color.FromArgb("#FFB7C5");

            DistanceBtn.BackgroundColor = _currentSort == SortOption.Distance ? primaryColor : Colors.White;
            RatingBtn.BackgroundColor = _currentSort == SortOption.Rating ? primaryColor : Colors.White;
            ReviewsBtn.BackgroundColor = _currentSort == SortOption.Reviews ? primaryColor : Colors.White;
        }

        private void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Border border && border.BindingContext is Restaurant restaurant)
            {
                _favoriteService.ToggleFavorite(restaurant.Id);
                
                var iconLabel = border.Content as Label;
                if (iconLabel != null)
                {
                    iconLabel.Text = _favoriteService.IsFavorite(restaurant.Id) ? "❤️" : "🤍";
                }
            }
        }

        private async void OnRestaurantSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Restaurant restaurant)
            {
                var detailPage = new RestaurantDetailPage(_restaurantService, _favoriteService, restaurant);
                await Navigation.PushAsync(detailPage);
                RestaurantList.SelectedItem = null;
            }
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
            _currentTab = TabType.Favorites;
            UpdateTabStyles();
            var favoritesPage = new FavoritesPage(_restaurantService, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(favoritesPage);
        }

        private void OnHomeTabClicked(object sender, EventArgs e)
        {
            _currentTab = TabType.Home;
            UpdateTabStyles();
        }

        private async void OnExploreTabClicked(object sender, EventArgs e)
        {
            _currentTab = TabType.Explore;
            UpdateTabStyles();
            var mapPage = new MapPage(_restaurantService, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(mapPage);
        }

        private void UpdateTabStyles()
        {
            var selectedColor = Color.FromRgba(0, 0, 0, 255);
            var unselectedColor = Color.FromRgba(0, 0, 0, 128);

            void UpdateTab(Grid tab, bool isSelected)
            {
                if (tab.Children.FirstOrDefault() is VerticalStackLayout stack)
                {
                    foreach (var child in stack.Children)
                    {
                        if (child is Label label)
                        {
                            label.TextColor = isSelected ? selectedColor : unselectedColor;
                        }
                    }
                }
            }

            UpdateTab(FavoritesTab, _currentTab == TabType.Favorites);
            UpdateTab(HomeTab, _currentTab == TabType.Home);
            UpdateTab(ExploreTab, _currentTab == TabType.Explore);
        }
    }

    public enum TabType
    {
        Favorites,
        Home,
        Explore
    }
}
