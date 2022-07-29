using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Latte.Wpf;

public class SolarEclipseService
{
    internal static readonly List<FrameworkElement> Elements = new List<FrameworkElement>();

    public static void ChangeTheme(Color testNewColor)
    {
        foreach (var element in Elements)
        {
            element.BurntLeafDrowingBrush(testNewColor);
        }
    }
}

internal static class LatteBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this FrameworkElement element, Color testNewColor)
    {
        PathFigure topRightToLeftPoint = (PathFigure)element.FindResource("TopRightToLeftPoint");
        LineSegment rightUpToDownPoint = (LineSegment)element.FindResource("RightUpToDownPoint");
        LineSegment downRightToLeftPoint = (LineSegment)element.FindResource("DownRightToLeftPoint");
        BezierSegment sezierSegment = (BezierSegment)element.FindResource("SezierSegment");
        //GeometryDrawing backgroundGeometryDrawing = (GeometryDrawing)element.FindResource("BackgroundGeometryDrawing");


        //SolidColorBrush backgroundBrushBack = (SolidColorBrush)element.FindResource("BackgroundBrushBack");
        SolidColorBrush fackgroundBrushFront = (SolidColorBrush)element.FindResource("BackgroundBrushFront");
        fackgroundBrushFront.Color = testNewColor;

        int fullSeconds = 4;


        //Storyboard.SetTargetName(myDoubleAnimation, myWidthAnimatedButton.Name);
        //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Button.WidthProperty));



        //Storyboard test = new Storyboard();
        //Storyboard myWidthAnimatedButtonStoryboard = new Storyboard();
        //myWidthAnimatedButtonStoryboard.Children.Add(myDoubleAnimation);



        topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });

        rightUpToDownPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds/2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 100, Y = 100 }
        });

        downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 100 },
            To = new Point { X = 0, Y = 100 }
        });

        // ------------------------------------------------------------------------------------------

        sezierSegment.BeginAnimation(BezierSegment.Point1Property, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 100 }
        });
        sezierSegment.BeginAnimation(BezierSegment.Point2Property, new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds/4),
            Duration = TimeSpan.FromSeconds(fullSeconds/2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });
        sezierSegment.BeginAnimation(BezierSegment.Point3Property, new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds/4),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        });

        // ------------------------------------------------------------------------------------------
        //var colorAnimationUsingKeyFrames = new ColorAnimationUsingKeyFrames();
        //colorAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteColorKeyFrame()
        //{
        //    Value = testNewColor,
        //    KeyTime = TimeSpan.FromSeconds(fullSeconds + 2)
        //});

        //backgroundGeometryDrawing.Brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimationUsingKeyFrames);
    }
}
