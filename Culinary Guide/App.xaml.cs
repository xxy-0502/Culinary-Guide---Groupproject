using Culinary_Guide.Services;

namespace Culinary_Guide
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var services = MauiProgram.Services;
            if (services == null)
                return new Window(new AppShell());

            var restaurantService = services.GetRequiredService<IRestaurantService>();
            var favoriteService = services.GetRequiredService<IFavoriteService>();
            var locationService = services.GetRequiredService<ILocationService>();
            var languageService = services.GetRequiredService<ILanguageService>();
            var userService = services.GetRequiredService<IUserService>();
            
            _ = InitializeDataAsync(restaurantService, favoriteService);
            
            var mainPage = new MainPage(restaurantService, favoriteService, locationService, languageService, userService);
            var navPage = new NavigationPage(mainPage);
            
            return new Window(navPage);
        }

        private async Task InitializeDataAsync(IRestaurantService restaurantService, IFavoriteService favoriteService)
        {
            await favoriteService.InitializeAsync();
            await restaurantService.InitializeSampleDataAsync();
        }
    }
}
