using System.Windows;
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

        private void OnThemeChengerClicked(object sender, RoutedEventArgs e)
        {
            if (isDarkTheme)
            {
                SolarEclipseService.ChangeTheme(themeChanger, (Color)FindResource("Light"));
            }
            else
            {
                SolarEclipseService.ChangeTheme(themeChanger, (Color)FindResource("Dark"));
            }

            isDarkTheme = !isDarkTheme;
        }
    }
}
