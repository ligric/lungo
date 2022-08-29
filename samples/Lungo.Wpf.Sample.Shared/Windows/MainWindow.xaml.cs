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
            themeChangerButton.Click += OnTestButtonClicked;
        }

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
    }
}
