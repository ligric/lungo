﻿using Lungo.Wpf.Sample.Windows;
using System.Windows;

namespace Lungo.Wpf.Sample
{
    public partial class App : Application
    {
        private void OnAppStartup(object sender, StartupEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
