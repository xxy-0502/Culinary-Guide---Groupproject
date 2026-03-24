using Culinary_Guide.Services;
using Culinary_Guide.Views;
using Microsoft.Extensions.Logging;

namespace Culinary_Guide
{
    public static class MauiProgram
    {
        public static MauiApp? AppInstance { get; private set; }
        public static IServiceProvider? Services => AppInstance?.Services;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<ILocationService, RealLocationService>();
            builder.Services.AddSingleton<IImageCacheService, ImageCacheService>();
            builder.Services.AddSingleton<IFavoriteService, FavoriteService>();
            builder.Services.AddSingleton<ILanguageService, LanguageService>();
            builder.Services.AddSingleton<IRestaurantService, RestaurantService>();
            builder.Services.AddSingleton<IUserService, UserService>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<RestaurantDetailPage>();
            builder.Services.AddTransient<EditProfilePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            AppInstance = builder.Build();
            return AppInstance;
        }
    }
}