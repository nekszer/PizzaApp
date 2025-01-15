using Light.Maui;

namespace Light.Themes
{
    public class BaseTheme
    {
        private ResourceDictionary Resources
        {
            get => Application.Current.Resources;
        }

        private AppTheme Theme
        {
            get => Application.Current.UserAppTheme;
            set => Application.Current.UserAppTheme = value;
        }

        private BaseTheme() { }

        private static BaseTheme instance;
        public static BaseTheme Instance
        {
            get
            {
                if (instance == null)
                    instance = new BaseTheme();
                return instance;
            }
        }

        public BaseTheme SetTheme(bool setnavpagestyle = true)
        {
            Application.Current.RequestedThemeChanged += App_RequestedThemeChanged;
            return SetTheme(Theme, setnavpagestyle);
        }

        public T OnTheme<T>(Func<ResourceDictionary, T> @default, Func<ResourceDictionary, T> light = null, Func<ResourceDictionary, T> dark = null)
        {
            var action = Theme == AppTheme.Dark ? (dark ?? @default) : (light ?? @default);
            if (action == null) return default(T);
            return action.Invoke(Resources);
        }

        public BaseTheme SetTheme(AppTheme theme, bool setnavpagestyle = true)
        {
            Theme = theme;
            if (!setnavpagestyle) return this;
            var navigation = GetNavigationPage();
            if (navigation == null) return this;
            SetNavigationPageStyle(navigation);
            SetStatusBarStyle();
            return this;
        }

        private void SetStatusBarStyle()
        {
            switch (Theme)
            {
                default:
                    CrossContainer.Instance.Create<IStatusBarPlatformSpecific>()?.SetStatusBarColor((Color)Resources["Primary"], (Color)Resources["White"]);
                    break;

                case AppTheme.Dark:
                    CrossContainer.Instance.Create<IStatusBarPlatformSpecific>()?.SetStatusBarColor((Color)Resources["PrimaryDark"], (Color)Resources["White"]);
                    break;
            }
        }

        public NavigationPage SetNavigationPageStyle(NavigationPage navigation)
        {
            navigation.BarBackgroundColor = Theme == AppTheme.Light ?
                       (Color)Resources["Primary"] :
                       (Color)Resources["PrimaryDark"];
            navigation.BarTextColor = Theme == AppTheme.Light ?
                    (Color)Resources["White"] :
                    (Color)Resources["PrimaryDarkText"];
            return navigation;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
            => SetTheme(e.RequestedTheme);

        private NavigationPage GetNavigationPage()
        {
            var main = Application.Current.MainPage;
            if (main is NavigationPage navpage)
                return navpage;
            else if (main is FlyoutPage master)
                if (master.Detail is NavigationPage navpage1)
                    return navpage1;
            return null;
        }

        public AppTheme GetTheme()
        {
            return Theme;
        }
    }
}
