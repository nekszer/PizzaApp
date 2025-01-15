using System.Globalization;
using Light.Maui;

namespace PizzaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LightApplication.Instance.InitializeComponent();
            LightApplication.Instance.Culture = new CultureInfo("es");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            LightApplication.Instance.OnSleep();
        }

        protected override void OnResume()
        {
            LightApplication.Instance.OnResume();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
#if WINDOWS

			window.Width = 480;
			window.Height = 900;
#endif
            return window;
        }
    }
}
