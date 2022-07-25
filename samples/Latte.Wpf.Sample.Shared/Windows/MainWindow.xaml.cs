using System.Windows;
using System.Windows.Media;

namespace Latte.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {
            Border0.SetResourceReference(BackgroundProperty, "Light");
        }
    }
}
