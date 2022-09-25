#if __WINUI__ || HAS_UNO || HAS_UNO_WINUI
using Microsoft.UI.Xaml;
using System;

namespace Lungo.Wpf.Services;

public partial class SolarEclipseService
{
    public void AddElementPrivate(FrameworkElement element)
        => throw new NotImplementedException();

    public static void ChangeThemePrivate(FrameworkElement changerElement, string themeKey, double milliseconds = 1_000)
        => throw new NotImplementedException();
}
#endif