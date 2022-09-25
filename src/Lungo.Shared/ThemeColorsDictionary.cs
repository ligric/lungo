#if WPF
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Lungo.Wpf
{
    [Localizability(LocalizationCategory.Ignore)]
    [Ambient]
    [UsableDuringInitialization(true)]
    public sealed class ThemeColorsDictionary : Dictionary<string, Color>
    {
    }
}
#endif