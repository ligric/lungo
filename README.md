

| package | version  | downloads |
| ---------------|:-------------:|:-------------:|
| Lungo         | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.svg?style=flat)](https://www.nuget.org/packages/Lungo/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.svg)](https://www.nuget.org/packages/Lungo) |
| Lungo.Wpf   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Wpf.svg?style=flat)](https://www.nuget.org/packages/Lungo.Wpf/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Wpf.svg)](https://www.nuget.org/packages/Lungo.Wpf) |
| Lungo.Uwp   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uwp.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uwp/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uwp.svg)](https://www.nuget.org/packages/Lungo.Uwp) |
| Lungo.Uno   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uno.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uno/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uno.svg)](https://www.nuget.org/packages/Lungo.Uno) |
| Lungo.Maui   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Maui.svg?style=flat)](https://www.nuget.org/packages/Lungo.Maui/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Maui.svg)](https://www.nuget.org/packages/Lungo.Maui) |


```XML
<Rectangle ap:SolarEclipse.ThemeChangingSubscribe="True"/>
```

```C#
private void OnTestButtonClicked(object sender, RoutedEventArgs e)
{
    GeneralTransform generalTransform = themeChangerButton.TransformToVisual((Visual)themeChangerButton.Parent);
    Rect rect = generalTransform.TransformBounds(new Rect(themeChangerButton.RenderSize));

    if (isDarkTheme)
    {
        SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Light"));
    }
    else
    {
        SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Dark"));
    }

    isDarkTheme = !isDarkTheme;
}
```

https://user-images.githubusercontent.com/69314237/187296510-b0cf2c2e-9027-4913-8c8d-aeca574de47b.mp4

