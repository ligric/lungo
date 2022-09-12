using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
#if __WINUI__
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using System.Windows.Media;
#endif

namespace Lungo.Wpf.Data;

internal class ThemeColorsInfo
{
    public FrameworkElement Element { get; }
    public IReadOnlyDictionary<string, Color> Themes { get; }

    public ThemeColorsInfo(FrameworkElement element, IEnumerable<KeyValuePair<string, Color>> themes)
    {
        Element = element;
        Themes = new ReadOnlyDictionary<string, Color>(themes.ToDictionary(x => x.Key, x => x.Value));
    }
}
