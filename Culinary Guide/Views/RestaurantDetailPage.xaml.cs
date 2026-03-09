using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class RestaurantDetailPage : ContentPage
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteService _favoriteService;
        private readonly Restaurant _restaurant;

        public RestaurantDetailPage(IRestaurantService restaurantService, 
                                    IFavoriteService favoriteService,
                                    Restaurant restaurant)
        {
            InitializeComponent();
            _restaurantService = restaurantService;
            _favoriteService = favoriteService;
            _restaurant = restaurant;
            BindingContext = restaurant;
            LoadImages();
            LoadReviews();
            UpdateFavoriteIcon();
        }

        private async void LoadImages()
        {
            ImageCarousel.ItemsSource = _restaurant.ImageUrls;
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
            var comment = await DisplayPromptAsync("写评价", "请输入您的评价：");
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var rating = await DisplayActionSheet("评分", "取消", null, "⭐⭐⭐⭐⭐", "⭐⭐⭐⭐", "⭐⭐⭐", "⭐⭐", "⭐");
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
                        UserName = "我",
                        Comment = comment,
                        Rating = ratingValue
                    };

                    await _restaurantService.AddReviewAsync(newReview);
                    _restaurant.ReviewCount++;
                    LoadReviews();
                    
                    await DisplayAlert("成功", "评价已提交！", "好的");
                }
            }
        }
    }
}
