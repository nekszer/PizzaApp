namespace Light.Themes
{
    public static class RequestTheme
    {
        public static readonly BindableProperty SetProperty =
            BindableProperty.CreateAttached(
                "Set",
                typeof(AppTheme),
                typeof(RequestTheme),
                default(AppTheme),
                propertyChanged: OnSetChanged);

        public static AppTheme GetSet(BindableObject bindable)
            => (AppTheme)bindable.GetValue(SetProperty);

        public static void SetSet(BindableObject bindable, AppTheme value)
            => bindable.SetValue(SetProperty, value);

        private static void OnSetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            var viewType = view?.GetType();
            if (viewType?.FullName == null)
                return;
            Enum.TryParse(newValue.ToString(), out AppTheme theme);
            BaseTheme.Instance.SetTheme(theme);
        }
    }
}