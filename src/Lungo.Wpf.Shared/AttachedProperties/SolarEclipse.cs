using System;
using System.Windows;

namespace Lungo.Wpf;

public static class SolarEclipse
{
    private static readonly SolarEclipseService solarEclipseService = new SolarEclipseService();

    public static bool GetThemeChangingSubscribe(FrameworkElement element)
    {
        return (bool)element.GetValue(ThemeChangingSubscribeProperty);
    }

    public static void SetThemeChangingSubscribe(FrameworkElement element, bool themeChangingSubscribe)
    {
        element.SetValue(ThemeChangingSubscribeProperty, themeChangingSubscribe);
    }

    public static readonly DependencyProperty ThemeChangingSubscribeProperty =
        DependencyProperty.RegisterAttached("ThemeChangingSubscribe", typeof(bool), 
            typeof(SolarEclipse), new PropertyMetadata(false, OnThemeChangingSubscribeChanged));

    private static void OnThemeChangingSubscribeChanged(DependencyObject @object, DependencyPropertyChangedEventArgs e)
    {
        FrameworkElement element = (FrameworkElement)@object;

        bool isNewValueSubscribeNew = (bool)e.NewValue;
        bool isNewValueSubscribeOld = (bool)e.OldValue;

        if (isNewValueSubscribeNew == isNewValueSubscribeOld)
            return;

        if (!isNewValueSubscribeNew)
        {
            solarEclipseService.RemoveElement(element);
        }
        else
        {
            if(!element.IsLoaded)
            {
                RoutedEventHandler loaded = null;
                loaded += (s, e) =>
                {
                    element.Loaded -= loaded;
                    solarEclipseService.AddElement(element);
                };
                element.Loaded += loaded;
            }
            else
            {
                solarEclipseService.AddElement(element);
            }
        }
    }
}