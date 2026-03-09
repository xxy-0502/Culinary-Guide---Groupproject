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
            
            var mainPage = new MainPage(restaurantService, favoriteService);
            var navPage = new NavigationPage(mainPage);
            
            return new Window(navPage);
        }
    }
}
