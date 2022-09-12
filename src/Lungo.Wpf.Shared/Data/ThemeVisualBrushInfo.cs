﻿using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace Lungo.Wpf.Data;

internal class ThemeVisualBrushInfo
{
    public VisualBrush CurrentDrawingBrush { get; }

    public IReadOnlyDictionary<string, DependencyObject> InsideElements { get; }

    public ThemeVisualBrushInfo(VisualBrush currentDrawingBrush, Dictionary<string, DependencyObject> insideElements)
    {
        CurrentDrawingBrush = currentDrawingBrush;
        InsideElements = insideElements;
    }
}
