using Culinary_Guide.Helpers;
using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class RestaurantDetailPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private readonly ILanguageService _languageService;
        private readonly Restaurant _restaurant;

        public RestaurantDetailPage(IRestaurantService restaurantService, 
                                    IFavoriteService favoriteService,
                                    Restaurant restaurant)
        {
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            _languageService = MauiProgram.Services?.GetRequiredService<ILanguageService>()!;
            _restaurant = restaurant;
            InitializeComponent();
            BindingContext = restaurant;
            UpdateLabels();
            UpdateFavoriteIcon();
            LoadReviews();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged += OnLanguageChanged;
            }
            if (ImageCarousel.ItemsSource == null)
            {
                ImageCarousel.ItemsSource = _restaurant.ImageUrls;
            }
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
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            var loc = Localize.Instance;
            ReviewCountLabel.Text = string.Format(loc.ReviewCountFormat, _restaurant.ReviewCount);
            AddressTitleLabel.Text = loc.AddressLabel;
            PhoneTitleLabel.Text = loc.PhoneLabel;
            OpeningHoursTitleLabel.Text = loc.OpeningHoursLabel;
            DescriptionTitleLabel.Text = $"📝 {loc.DescriptionLabel}";
            UserReviewsTitleLabel.Text = $"💬 {loc.UserReviewsLabel}";
            WriteReviewLabel.Text = $"✍️ {loc.WriteReview}";
        }

        private async void LoadReviews()
        {
            var reviews = await _restaurantService.GetReviewsAsync(_restaurant.Id);
            ReviewsList.ItemsSource = reviews;
        }

        private void UpdateFavoriteIcon()
        {
            FavoriteIcon.Text = _favoriteService.IsFavorite(_restaurant.Id) ? "❤️" : "🤍";
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnFavoriteClicked(object sender, EventArgs e)
        {
            _favoriteService.ToggleFavorite(_restaurant.Id);
            UpdateFavoriteIcon();
        }

        private async void OnLikeReview(object sender, EventArgs e)
        {
            if (sender is Label label && label.BindingContext is Review review)
            {
                await _restaurantService.AddLikeToReviewAsync(review.Id);
                review.LikeCount++;
                
                if (ReviewsList.ItemsSource is List<Review> reviews)
                {
                    var index = reviews.IndexOf(review);
                    ReviewsList.ItemsSource = null;
                    ReviewsList.ItemsSource = reviews;
                }
            }
        }

        private async void OnWriteReviewClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            var comment = await DisplayPromptAsync(loc.WriteReview, loc.EnterReview);
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var rating = await DisplayActionSheet(loc.Rating, loc.Cancel, null, "⭐⭐⭐⭐⭐", "⭐⭐⭐⭐", "⭐⭐⭐", "⭐⭐", "⭐");
                var ratingValue = rating switch
                {
                    "⭐⭐⭐⭐⭐" => 5.0,
                    "⭐⭐⭐⭐" => 4.0,
                    "⭐⭐⭐" => 3.0,
                    "⭐⭐" => 2.0,
                    "⭐" => 1.0,
                    _ => 0
                };

                if (ratingValue > 0)
                {
                    var newReview = new Review
                    {
                        RestaurantId = _restaurant.Id,
                        UserName = _languageService.CurrentLanguage == AppLanguage.Chinese ? "我" : "Me",
                        Comment = comment,
                        Rating = ratingValue
                    };

                    await _restaurantService.AddReviewAsync(newReview);
                    _restaurant.ReviewCount++;
                    UpdateLabels();
                    LoadReviews();
                    
                    await DisplayAlert(loc.OK, loc.ReviewSubmitted, loc.OK);
                }
            }
        }
    }
}