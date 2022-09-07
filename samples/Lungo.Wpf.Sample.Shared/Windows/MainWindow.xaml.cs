using System.Windows;
using System.Windows.Media;

namespace Lungo.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        private bool isDarkTheme = true;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnThemeChengerClicked(object sender, RoutedEventArgs e)
        {
            if (isDarkTheme)
            {
                SolarEclipseService.ChangeTheme(themeChanger, (Color)FindResource("BaseLight"));
            }
            else
            {
                SolarEclipseService.ChangeTheme(themeChanger, (Color)FindResource("BaseDark"));
            }

            isDarkTheme = !isDarkTheme;
        }
    }
}
