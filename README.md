[![NuGet Status](https://img.shields.io/nuget/v/Lungo.svg?style=flat)](https://www.nuget.org/packages/Lungo/) [![NuGet](https://img.shields.io/nuget/dt/Lungo.svg)](https://www.nuget.org/packages/Lungo)

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

