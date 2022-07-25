using Latte.Wpf.Sample.Windows;
using System.Windows;

namespace Latte.Wpf.Sample
{
    public partial class App
    {
        private void OnAppStartup(object sender, StartupEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
