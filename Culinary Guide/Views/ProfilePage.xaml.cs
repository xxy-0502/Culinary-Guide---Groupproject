using Culinary_Guide.Helpers;
using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILanguageService _languageService;
        private readonly List<Restaurant> _allRestaurants;
        private List<BrowseHistory> _browseHistory = new();

        public ProfilePage(IFavoriteService favoriteService, List<Restaurant> allRestaurants)
        {
            _favoriteService = favoriteService;
            _languageService = MauiProgram.Services?.GetRequiredService<ILanguageService>()!;
            _allRestaurants = allRestaurants;
            InitializeComponent();
            BindingContext = Localize.Instance;
            LoadData();
            UpdateLanguageLabel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged += OnLanguageChanged;
            }
            LoadData();
            UpdateLanguageLabel();
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
            UpdateLanguageLabel();
        }

        private void UpdateLanguageLabel()
        {
            if (_languageService != null)
            {
                CurrentLanguageLabel.Text = _languageService.CurrentLanguage == AppLanguage.Chinese 
                    ? "中文" 
                    : "English";
            }
        }

        private void LoadData()
        {
            var favoriteCount = _favoriteService.GetFavoriteIds().Count;
            var browseCount = _browseHistory.Count;
            
            FavoriteCountLabel.Text = favoriteCount.ToString();
            BrowseCountLabel.Text = browseCount.ToString();
            HistoryList.ItemsSource = _browseHistory;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
            var favoritesPage = new FavoritesPage(null!, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(favoritesPage);
        }

        private async void OnHomeTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnExploreTabClicked(object sender, EventArgs e)
        {
            var mapPage = new MapPage(null!, _favoriteService, _allRestaurants);
            await Navigation.PushAsync(mapPage);
        }

        private async void OnLanguageClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            var currentLang = _languageService?.CurrentLanguage ?? AppLanguage.English;
            
            var action = await DisplayActionSheet(
                loc.Language,
                loc.Cancel,
                null,
                "中文",
                "English");
            
            if (action == "中文")
            {
                _languageService?.SetLanguage(AppLanguage.Chinese);
            }
            else if (action == "English")
            {
                _languageService?.SetLanguage(AppLanguage.English);
            }
        }

        private async void OnNotificationSettingClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            await DisplayAlert(loc.NotificationSettings, loc.Developing, loc.OK);
        }

        private async void OnPrivacySettingClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            await DisplayAlert(loc.PrivacySettings, loc.Developing, loc.OK);
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            await DisplayAlert(loc.AboutApp, loc.AboutAppContent, loc.OK);
        }
    }

    public class ProfileViewModel
    {
        public int FavoriteCount { get; set; }
        public int BrowseCount { get; set; }
    }

    public class BrowseHistory
    {
        public string Name { get; set; } = "";
        public string BrowseTime { get; set; } = "";
    }
}