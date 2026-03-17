using Culinary_Guide.Helpers;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class NotificationsPage : ContentPage
    {
        private readonly ILanguageService _languageService;

        public NotificationsPage()
        {
            _languageService = MauiProgram.Services?.GetRequiredService<ILanguageService>()!;
            InitializeComponent();
            BindingContext = Localize.Instance;
            LoadNotifications();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged += OnLanguageChanged;
            }
            Localize.Instance.Invalidate();
            LoadNotifications();
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
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            var loc = Localize.Instance;
            var notifications = new List<NotificationItem>
            {
                new NotificationItem
                {
                    Icon = "🎉",
                    Title = loc.WelcomeNotification,
                    Content = loc.WelcomeNotificationContent
                },
                new NotificationItem
                {
                    Icon = "🔔",
                    Title = loc.NewFeatureNotification,
                    Content = loc.NewFeatureNotificationContent
                },
                new NotificationItem
                {
                    Icon = "⭐",
                    Title = loc.FavoriteNotification,
                    Content = loc.FavoriteNotificationContent
                }
            };

            NotificationsList.ItemsSource = notifications;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnFavoritesTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnHomeTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnExploreTabClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }

    public class NotificationItem
    {
        public string Icon { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
    }
}