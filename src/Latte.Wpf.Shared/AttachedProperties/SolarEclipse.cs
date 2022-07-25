using System.Windows;

namespace Latte.Wpf;

public class SolarEclipse
{
    public static bool GetThemeChangingSubscribe(FrameworkElement element)
    {
        return (bool)element.GetValue(ThemeChangingSubscribeProperty);
    }

    public static void SetThemeChangingSubscribe(FrameworkElement element, bool themeChangingSubscribe)
    {
        element.SetValue(ThemeChangingSubscribeProperty, themeChangingSubscribe);
    }

    public static readonly DependencyProperty ThemeChangingSubscribeProperty =
        DependencyProperty.RegisterAttached("ThemeChangingSubscribe", typeof(bool), typeof(SolarEclipse), new PropertyMetadata(false));
}
