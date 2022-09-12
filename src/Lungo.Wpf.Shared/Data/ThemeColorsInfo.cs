using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

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
