using Lungo.Wpf.Services;
using System.Windows;

namespace Lungo.Wpf.Sample.Windows;

public sealed partial class MainWindow : Window
{
    public MainWindow() => this.InitializeComponent();

    private void OnDarkChecked(object sender, RoutedEventArgs e)
    {
        SolarEclipseService.ChangeTheme(themeChanger, "Dark");
    }

    private void OnLightChecked(object sender, RoutedEventArgs e)
    {
        SolarEclipseService.ChangeTheme(themeChanger, "Light");
    }
}
