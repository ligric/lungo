## Nugets

| package | version  | downloads |
| ---------------|:-------------:|:-------------:|
| Lungo         | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.svg?style=flat)](https://www.nuget.org/packages/Lungo/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.svg)](https://www.nuget.org/packages/Lungo) |
| Lungo.Wpf   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Wpf.svg?style=flat)](https://www.nuget.org/packages/Lungo.Wpf/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Wpf.svg)](https://www.nuget.org/packages/Lungo.Wpf) |
| Lungo.Uwp   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uwp.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uwp/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uwp.svg)](https://www.nuget.org/packages/Lungo.Uwp) |
| Lungo.Uno   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uno.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uno/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uno.svg)](https://www.nuget.org/packages/Lungo.Uno) |
| Lungo.WinUI   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.WinUI.svg?style=flat)](https://www.nuget.org/packages/Lungo.WinUI/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.WinUI.svg)](https://www.nuget.org/packages/Lungo.WinUI) |
| Lungo.Uno.WinUI   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uno.WinUI.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uno.WinUI/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uno.WinUI.svg)](https://www.nuget.org/packages/Lungo.Uno.WinUI) |

## Preview
https://user-images.githubusercontent.com/69314237/189699019-de1778a5-3608-4947-b3b0-02512929a050.mp4

## Example
```XML
<Window.Resources>
    <lungo:ThemeColorsDictionary x:Key="RectangleThemeColor">
        <Color x:Key="Dark">#1e1e1e</Color>
        <Color x:Key="Light">#ffffff</Color>
    </lungo:ThemeColorsDictionary>
</Window.Resources>
```
```XML
<Rectangle Fill="{lungo:ThemeColorResource RectangleThemeColor}" Width="100" Height="200"
           HorizontalAlignment="Left" VerticalAlignment="Top"/>

<ToggleButton x:Name="themeChanger" Height="20" Width="40" Margin="0,8,20,0"
              HorizontalAlignment="Right" VerticalAlignment="Top" 
              Checked="OnLightChecked" Unchecked="OnDarkChecked"/>
```


```C#
private void OnDarkChecked(object sender, RoutedEventArgs e)
    => SolarEclipseService.ChangeTheme(themeChanger, "Dark", 5_000);

private void OnLightChecked(object sender, RoutedEventArgs e)
    => SolarEclipseService.ChangeTheme(themeChanger, "Light", 5_000);
```



