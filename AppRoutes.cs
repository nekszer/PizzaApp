using Light.Maui.Services.Navigation;
using PizzaApp.MVVM.ViewModels;
using PizzaApp.MVVM.Views;

namespace PizzaApp
{
    public class AppRoutes
    {

        // [Route()]
        public static string SplashScreen { get; set; } = "/splashscreen";
		public static string PizzaPage { get; set; } = "/pizzapage";


        public AppRoutes(IRoutingService routeService)
        {
            routeService.Route<MainPage, MainViewModel>(SplashScreen);
            routeService.Route<PizzaPage, PizzaViewModel>(PizzaPage, true);
        }
    }
}