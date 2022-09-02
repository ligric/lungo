using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Lungo.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        private bool isDarkTheme = false;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnThemeChangerButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isDarkTheme)
            {
                SolarEclipseService.ChangeTheme(themeChangerButton, (Color)FindResource("Light"));
            }
            else
            {
                SolarEclipseService.ChangeTheme(themeChangerButton, (Color)FindResource("Dark"));
            }

            isDarkTheme = !isDarkTheme;
        }
    }
}
