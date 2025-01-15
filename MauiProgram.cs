using Light.Maui;
using Light.Themes;
using Microsoft.Extensions.Logging;

namespace PizzaApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp(IPlatformInitializer platformInitializer = null)
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                //.UseUraniumUI()
                //.UseUraniumUIMaterial()
                // .UseFFImageLoading()
                // .UseSkiaSharp()
                .UseLight((routeService, container) =>
                {
                    platformInitializer?.RegisterTypes(container);
                    new AppRoutes(routeService);
                    new AppServices(container);
                }, (root) =>
                {
                    return BaseTheme
                        .Instance
                        .SetTheme(AppTheme.Light, true)
                        .SetNavigationPageStyle(new NavigationPage(root));
                })
                // .UseExtendedControls()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Raleway-Regular.ttf", "Raleway");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FontAwesome6Solid.otf", "FontAwesomeSolid");
                    fonts.AddFont("LineIcons.ttf", "LineIcons");
                });

#if DEBUG
			builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
