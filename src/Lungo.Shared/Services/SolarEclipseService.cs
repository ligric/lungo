using System;

#if WPF
using FrameworkElement = System.Windows.FrameworkElement;
#else
using FrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
#endif

namespace Lungo.Wpf.Services;

public partial class SolarEclipseService
{
    public void AddElement(FrameworkElement element)
        => AddElementPrivate(element);

    public void RemoveElement(FrameworkElement element)
        => throw new NotImplementedException();

    public static void ChangeTheme(FrameworkElement changerElement, string themeKey, double milliseconds = 1_000) =>
        ChangeThemePrivate(changerElement, themeKey, milliseconds = 1_000);
}