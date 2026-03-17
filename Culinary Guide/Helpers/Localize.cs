using System.ComponentModel;
using System.Runtime.CompilerServices;
using Culinary_Guide.Resources;

namespace Culinary_Guide.Helpers
{
    public class Localize : INotifyPropertyChanged
    {
        private static Localize? _instance;
        public static Localize Instance => _instance ??= new Localize();
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string AppTitle => AppResources.AppTitle;
        public string WelcomeText => AppResources.WelcomeText;
        public string Location => AppResources.Location;
        public string SearchPlaceholder => AppResources.SearchPlaceholder;
        public string SortByDistance => AppResources.SortByDistance;
        public string SortByRating => AppResources.SortByRating;
        public string SortByPopular => AppResources.SortByPopular;
        public string ReviewsCount => AppResources.ReviewsCount;
        public string TabFavorites => AppResources.TabFavorites;
        public string TabHome => AppResources.TabHome;
        public string TabExplore => AppResources.TabExplore;
        public string FavoritesTitle => AppResources.FavoritesTitle;
        public string NoFavorites => AppResources.NoFavorites;
        public string GoDiscover => AppResources.GoDiscover;
        public string ExploreTitle => AppResources.ExploreTitle;
        public string MyLocation => AppResources.MyLocation;
        public string RestaurantCount => AppResources.RestaurantCount;
        public string NoNearbyRestaurants => AppResources.NoNearbyRestaurants;
        public string ProfileTitle => AppResources.ProfileTitle;
        public string DefaultNickname => AppResources.DefaultNickname;
        public string DefaultBio => AppResources.DefaultBio;
        public string MyFavoritesCount => AppResources.MyFavoritesCount;
        public string BrowseHistoryCount => AppResources.BrowseHistoryCount;
        public string BrowseHistory => AppResources.BrowseHistory;
        public string ViewMore => AppResources.ViewMore;
        public string NoBrowseHistory => AppResources.NoBrowseHistory;
        public string Settings => AppResources.Settings;
        public string Language => AppResources.Language;
        public string Chinese => AppResources.Chinese;
        public string English => AppResources.English;
        public string NotificationSettings => AppResources.NotificationSettings;
        public string PrivacySettings => AppResources.PrivacySettings;
        public string AboutApp => AppResources.AboutApp;
        public string AboutAppContent => AppResources.AboutAppContent;
        public string Developing => AppResources.Developing;
        public string NotificationsTitle => AppResources.NotificationsTitle;
        public string NoNotifications => AppResources.NoNotifications;
        public string WelcomeNotification => AppResources.WelcomeNotification;
        public string WelcomeNotificationContent => AppResources.WelcomeNotificationContent;
        public string NewFeatureNotification => AppResources.NewFeatureNotification;
        public string NewFeatureNotificationContent => AppResources.NewFeatureNotificationContent;
        public string FavoriteNotification => AppResources.FavoriteNotification;
        public string FavoriteNotificationContent => AppResources.FavoriteNotificationContent;
        public string WriteReview => AppResources.WriteReview;
        public string EnterReview => AppResources.EnterReview;
        public string Rating => AppResources.Rating;
        public string Cancel => AppResources.Cancel;
        public string Submit => AppResources.Submit;
        public string ReviewSubmitted => AppResources.ReviewSubmitted;
        public string OK => AppResources.OK;
        public string Error => AppResources.Error;
        public string LoadFailed => AppResources.LoadFailed;
        public string Back => AppResources.Back;
        
        // GPS Location
        public string CurrentLocation => AppResources.CurrentLocation;
        public string Latitude => AppResources.Latitude;
        public string Longitude => AppResources.Longitude;
        public string Accuracy => AppResources.Accuracy;
        public string LocationPermissionRequired => AppResources.LocationPermissionRequired;
        public string CannotGetLocation => AppResources.CannotGetLocation;
        public string GetLocationFailed => AppResources.GetLocationFailed;
        public string Meters => AppResources.Meters;
        public string GettingLocation => AppResources.GettingLocation;
        
        // Restaurant Detail Labels
        public string ReviewCountFormat => AppResources.ReviewCountFormat;
        public string ReviewCountTapFormat => AppResources.ReviewCountTapFormat;
        public string AddressLabel => AppResources.AddressLabel;
        public string PhoneLabel => AppResources.PhoneLabel;
        public string OpeningHoursLabel => AppResources.OpeningHoursLabel;
        public string DescriptionLabel => AppResources.DescriptionLabel;
        public string UserReviewsLabel => AppResources.UserReviewsLabel;
        
        public void Invalidate()
        {
            OnPropertyChanged(null);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}