using System;
using System.Windows;

namespace Latte.Wpf;

public static class SolarEclipse
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
            SolarEclipseService.Elements.Remove(element);
        }
        else
        {
            if (SolarEclipseService.Elements.Contains(element))
                throw new ArgumentException("The element already exist.");

            SolarEclipseService.Elements.Add(element);
        }
    }
}