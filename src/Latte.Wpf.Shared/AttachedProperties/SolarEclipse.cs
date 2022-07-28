using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Latte.Wpf;

public static class SolarEclipse
{
    private static readonly List<TargetPropertyDescriptor> targets = new List<TargetPropertyDescriptor>();

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
        bool isNewValueSubscribe = (bool)e.NewValue;
        FrameworkElement element = (FrameworkElement)@object;

        int index = targets.FindIndex(0, targets.Count, x => x.Source == element);

        if (!isNewValueSubscribe && index >= 0)
        {
            targets[index].PropertyChanged -= OnPropertyChanged;
            targets.RemoveAt(index);
        }
        else if(isNewValueSubscribe && index == -1)
        {
            BackgroundSubscribe(element);
        }
        else
        {
            throw new NotImplementedException($"isNewValueSubscribe is {isNewValueSubscribe} but index is {index}.");
        }
    }

    private static void BackgroundSubscribe(FrameworkElement element)
    {
        TargetPropertyDescriptor target = null;

        if (element is Panel elementPanel)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementPanel, nameof(elementPanel.Background));
        }
        else if (element is Border elementBorder)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementBorder, nameof(elementBorder.Background));
        }
        else if (element is Shape elementShape)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementShape, nameof(elementShape.Fill));
        }
        else
        {
            throw new NotImplementedException($"The element {element} is not Panel or Border or Shape.");
        }

        target.PropertyChanged += OnPropertyChanged;
        targets.Add(target);
    }

    private static void OnPropertyChanged(object sender, EventArgs e)
    {

    }
}