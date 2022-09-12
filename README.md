## Nugets

| package | version  | downloads |
| ---------------|:-------------:|:-------------:|
| Lungo         | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.svg?style=flat)](https://www.nuget.org/packages/Lungo/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.svg)](https://www.nuget.org/packages/Lungo) |
| Lungo.Wpf   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Wpf.svg?style=flat)](https://www.nuget.org/packages/Lungo.Wpf/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Wpf.svg)](https://www.nuget.org/packages/Lungo.Wpf) |
| Lungo.Uwp   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uwp.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uwp/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uwp.svg)](https://www.nuget.org/packages/Lungo.Uwp) |
| Lungo.Uno   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Uno.svg?style=flat)](https://www.nuget.org/packages/Lungo.Uno/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Uno.svg)](https://www.nuget.org/packages/Lungo.Uno) |
| Lungo.Maui   | [![NuGet Status](https://img.shields.io/nuget/v/Lungo.Maui.svg?style=flat)](https://www.nuget.org/packages/Lungo.Maui/) | [![NuGet Status](https://img.shields.io/nuget/dt/Lungo.Maui.svg)](https://www.nuget.org/packages/Lungo.Maui) |

## Examples
```XML
<Window.Resources>
    <lungo:ThemeColorsDictionary x:Key="ToggleButtonThemeColor">
        <Color x:Key="Dark">#512BD4</Color>
        <Color x:Key="Light">Red</Color>
    </lungo:ThemeColorsDictionary>

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
              Background="{lungo:ThemeColorResource ToggleButtonThemeColor}"
              HorizontalAlignment="Right" VerticalAlignment="Top" 
              Checked="OnDarkChecked" Unchecked="OnLightChecked"/>
```


```C#
private void OnDarkChecked(object sender, RoutedEventArgs e)
    => SolarEclipseService.ChangeTheme(themeChanger, "Light", 5_000);

private void OnLightChecked(object sender, RoutedEventArgs e)
    => SolarEclipseService.ChangeTheme(themeChanger, "Dark", 5_000);
```
## Preview
https://user-images.githubusercontent.com/69314237/189699019-de1778a5-3608-4947-b3b0-02512929a050.mp4



