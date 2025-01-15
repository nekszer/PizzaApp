using Microsoft.Maui.Platform;

namespace Light.Theme
{
    public partial class AccentTheme : ResourceDictionary
    {
        public AccentTheme()
        {
            InitializeComponent();
#if WINDOWS
    		var uiSettings = new Windows.UI.ViewManagement.UISettings();
    		var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
    		var mauicolor = Color.Parse(color.ToString());
    		Add("Primary", mauicolor);
    		Add("PrimaryDark", GetDarkColor(mauicolor));
    		Add("PrimaryDarkText", GetContrastingTextColor(mauicolor));
#elif ANDROID
    		var value = new Android.Util.TypedValue();
            Android.App.Application.Context.ApplicationContext.Theme.ResolveAttribute(Android.Resource.Attribute.ColorAccent, value, true);
            var contexwrapper = new AndroidX.AppCompat.View.ContextThemeWrapper(Android.App.Application.Context.ApplicationContext, Android.Resource.Style.ThemeDeviceDefault);
            contexwrapper.Theme.ResolveAttribute(Android.Resource.Attribute.ColorAccent, value, true);
            var color = value.Data;
            var mauicolor = new Android.Graphics.Color(color).ToColor();
    		Add("Primary", mauicolor);
    		Add("PrimaryDark", GetDarkColor(mauicolor));
    		Add("PrimaryDarkText", GetContrastingTextColor(mauicolor));
#else
            Add("Primary", Color.FromArgb("#512BD4"));
            Add("PrimaryDark", Color.FromArgb("#ac99ea"));
            Add("PrimaryDarkText", Color.FromArgb("#242424"));
#endif
        }

        private Color GetContrastingTextColor(Color backgroundColor)
        {
            // Calcula el brillo del color de fondo (background)
            double brightness = (0.299 * backgroundColor.Red + 0.587 * backgroundColor.Green + 0.114 * backgroundColor.Blue) / 255;
            // Determina el color de texto (foreground) en función del brillo
            return brightness < 0.5 ? Colors.White : Colors.Black;
        }

        private Color GetDarkColor(Color color)
        {
            // Ajusta la intensidad del color oscuro (0.5 para la mitad de intensidad)
            double factorIntensidad = 0.5;
            // Calcula los nuevos componentes RGB para el color oscuro
            byte r = (byte)(color.Red * factorIntensidad);
            byte g = (byte)(color.Green * factorIntensidad);
            byte b = (byte)(color.Blue * factorIntensidad);
            // Crea el nuevo color oscuro
            return new Color(r, g, b);
        }
    }
}