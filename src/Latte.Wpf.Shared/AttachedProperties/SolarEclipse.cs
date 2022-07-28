using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Latte.Wpf;

public static class SolarEclipse
{
    private static readonly List<FrameworkElement> elements = new List<FrameworkElement>();

    public static bool GetThemeChangingSubscribe(FrameworkElement element)
    {
        return (bool)element.GetValue(ThemeChangingSubscribeProperty);
    }

    public static void SetThemeChangingSubscribe(FrameworkElement element, bool themeChangingSubscribe)
    {
        element.SetValue(ThemeChangingSubscribeProperty, themeChangingSubscribe);
    }

    public static readonly DependencyProperty ThemeChangingSubscribeProperty =
        DependencyProperty.RegisterAttached("ThemeChangingSubscribe", typeof(bool), 
            typeof(SolarEclipse), new PropertyMetadata(false, OnThemeChangingSubscribeChanged));

    private static void OnThemeChangingSubscribeChanged(DependencyObject @object, DependencyPropertyChangedEventArgs e)
    {
        FrameworkElement element = (FrameworkElement)@object;

        bool isNewValueSubscribeNew = (bool)e.NewValue;
        bool isNewValueSubscribeOld = (bool)e.OldValue;

        if (isNewValueSubscribeNew == isNewValueSubscribeOld)
            return;

        if (!isNewValueSubscribeNew)
        {
            elements.Remove(element);
        }
        else
        {
            if (elements.Contains(element))
                throw new ArgumentException("The element already exist.");

            elements.Add(element);
        }
    }

    private static void OnPropertyChanged(object sender, EventArgs e)
    {
        FrameworkElement element = (FrameworkElement)sender;
        element.BurntLeafDrowingBrush();
    }
}

internal static class LatteBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this FrameworkElement element)
    {
        PathFigure topRightToLeftPoint = (PathFigure)element.FindResource("TopRightToLeftPoint");
        LineSegment rightUpToDownPoint = (LineSegment)element.FindResource("RightUpToDownPoint");
        LineSegment downRightToLeftPoint = (LineSegment)element.FindResource("DownRightToLeftPoint");
        BezierSegment sezierSegment = (BezierSegment)element.FindResource("SezierSegment");
        GeometryDrawing backgroundGeometryDrawing = (GeometryDrawing)element.FindResource("BackgroundGeometryDrawing");

        topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(4),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });

        rightUpToDownPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 100, Y = 100 }
        });

        downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(2),
            Duration = TimeSpan.FromSeconds(4),
            From = new Point { X = 100, Y = 100 },
            To = new Point { X = 0, Y = 100 }
        });

        // ------------------------------------------------------------------------------------------

        sezierSegment.BeginAnimation(BezierSegment.Point1Property, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(2),
            Duration = TimeSpan.FromSeconds(4),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 100 }
        });
        sezierSegment.BeginAnimation(BezierSegment.Point2Property, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(1),
            Duration = TimeSpan.FromSeconds(2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });
        sezierSegment.BeginAnimation(BezierSegment.Point3Property, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(1),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });

        // ------------------------------------------------------------------------------------------
        if (backgroundGeometryDrawing.Brush.IsFrozen)
            backgroundGeometryDrawing.Brush = backgroundGeometryDrawing.Brush.CloneCurrentValue();

        var colorAnimationUsingKeyFrames = new ColorAnimationUsingKeyFrames();
        colorAnimationUsingKeyFrames.AutoReverse = true;
        colorAnimationUsingKeyFrames.Duration = TimeSpan.FromSeconds(2);
        colorAnimationUsingKeyFrames.KeyFrames.Add(new EasingColorKeyFrame((Color)ColorConverter.ConvertFromString("#2b2731")));
        backgroundGeometryDrawing.Brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimationUsingKeyFrames);
    }
}