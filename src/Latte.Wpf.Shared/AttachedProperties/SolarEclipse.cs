using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace Latte.Wpf.Shared.AttachedProperties
{
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
}
