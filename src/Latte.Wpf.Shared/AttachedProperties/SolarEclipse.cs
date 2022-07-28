using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Latte.Wpf;

public static class SolarEclipse
{
    private static readonly List<TargetPropertyDescriptor> targets = new List<TargetPropertyDescriptor>();

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
        bool isNewValueSubscribe = (bool)e.NewValue;
        FrameworkElement element = (FrameworkElement)@object;

        int index = targets.FindIndex(0, targets.Count, x => x.Source == element);

        if (!isNewValueSubscribe && index >= 0)
        {
            targets[index].PropertyChanged -= OnPropertyChanged;
            targets.RemoveAt(index);
        }
        else if(isNewValueSubscribe && index == -1)
        {
            BackgroundSubscribe(element);
        }
        else
        {
            throw new NotImplementedException($"isNewValueSubscribe is {isNewValueSubscribe} but index is {index}.");
        }
    }

    private static void BackgroundSubscribe(FrameworkElement element)
    {
        TargetPropertyDescriptor target = null;

        if (element is Panel elementPanel)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementPanel, nameof(elementPanel.Background));
        }
        else if (element is Border elementBorder)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementBorder, nameof(elementBorder.Background));
        }
        else if (element is Shape elementShape)
        {
            target = new TargetPropertyDescriptor(Guid.NewGuid().ToString(), elementShape, nameof(elementShape.Fill));
        }
        else
        {
            throw new NotImplementedException($"The element {element} is not Panel or Border or Shape.");
        }

        target.PropertyChanged += OnPropertyChanged;
        targets.Add(target);
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
        //SolidColorBrush backgroundBrush = (SolidColorBrush)element.FindResource("BackgroundBrush");
        GeometryDrawing backgroundGeometryDrawing = (GeometryDrawing)element.FindResource("BackgroundGeometryDrawing");

        element.MouseEnter += (s, e) =>
        {
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
            colorAnimationUsingKeyFrames.Duration = TimeSpan.FromSeconds(2);    
            colorAnimationUsingKeyFrames.KeyFrames.Add(new EasingColorKeyFrame((Color)ColorConverter.ConvertFromString("#2b2731")));
            backgroundGeometryDrawing.Brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimationUsingKeyFrames);
        };
    }
}