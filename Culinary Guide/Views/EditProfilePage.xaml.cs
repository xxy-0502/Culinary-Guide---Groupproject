using Culinary_Guide.Helpers;
using Culinary_Guide.Models;
using Culinary_Guide.Services;

namespace Culinary_Guide.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;
        private UserProfile? _currentProfile;
        private string? _newAvatarPath;

        public EditProfilePage(IUserService userService)
        {
            _userService = userService;
            _languageService = MauiProgram.Services?.GetRequiredService<ILanguageService>()!;
            InitializeComponent();
            BindingContext = Localize.Instance;
            LoadProfile();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_languageService != null)
            {
                _languageService.LanguageChanged += OnLanguageChanged;
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
            Localize.Instance.Invalidate();
        }

        private async void LoadProfile()
        {
            _currentProfile = await _userService.GetUserProfileAsync();
            
            NicknameEntry.Text = _currentProfile.Nickname;
            BioEditor.Text = _currentProfile.Bio;
            
            UpdateAvatarDisplay(_currentProfile.AvatarPath);
        }

        private void UpdateAvatarDisplay(string? avatarPath)
        {
            if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
            {
                AvatarImage.Source = ImageSource.FromFile(avatarPath);
                AvatarImage.IsVisible = true;
                AvatarPlaceholder.IsVisible = false;
            }
            else
            {
                AvatarImage.IsVisible = false;
                AvatarPlaceholder.IsVisible = true;
            }
        }

        private async void OnAvatarClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            
            var action = await DisplayActionSheet(
                loc.ChangeAvatar,
                loc.Cancel,
                null,
                loc.TakePhoto,
                loc.ChooseFromGallery
            );

            string? newPath = null;
            
            if (action == loc.TakePhoto)
            {
                newPath = await _userService.TakePhotoAsync();
            }
            else if (action == loc.ChooseFromGallery)
            {
                newPath = await _userService.PickAvatarAsync();
            }

            if (!string.IsNullOrEmpty(newPath))
            {
                _newAvatarPath = newPath;
                UpdateAvatarDisplay(newPath);
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var loc = Localize.Instance;
            
            var profile = new UserProfile
            {
                Nickname = NicknameEntry.Text?.Trim() ?? "",
                Bio = BioEditor.Text?.Trim() ?? "",
                AvatarPath = _newAvatarPath ?? _currentProfile?.AvatarPath
            };

            await _userService.SaveUserProfileAsync(profile);
            
            await DisplayAlert(loc.OK, loc.ProfileUpdated, loc.OK);
            
            await Navigation.PopAsync();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}