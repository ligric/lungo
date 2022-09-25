using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#if WPF
using FrameworkElement = System.Windows.FrameworkElement;
using Color = System.Windows.Media.Color;
#else
using FrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
using Color = Windows.UI.Color;
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