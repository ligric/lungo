[![NuGet Status](https://img.shields.io/nuget/v/Lungo.svg?style=flat)](https://www.nuget.org/packages/Lungo/) [![NuGet](https://img.shields.io/nuget/dt/Lungo.svg)](https://www.nuget.org/packages/Lungo)

```C#
GeneralTransform generalTransform = button.TransformToVisual((Visual)button.Parent);
Rect rect = generalTransform.TransformBounds(new Rect(new Point(button.Margin.Left, button.Margin.Top), button.RenderSize));

if (isDarkTheme)
{
    SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Light"));
}
else
{
    SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Dark"));
}
```

https://user-images.githubusercontent.com/69314237/182225523-9734cfa2-2719-469e-ae7c-5bd167c50324.mp4

