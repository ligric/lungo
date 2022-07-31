using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Latte.Wpf;

internal class BackgroundInfo
{
    public DrawingBrush CurrentDrawingBrush { get; }

    public IReadOnlyDictionary<string, DependencyObject> InsideElements { get; }

    public BackgroundInfo(DrawingBrush currentDrawingBrush, Dictionary<string, DependencyObject> insideElements)
    {
        CurrentDrawingBrush = currentDrawingBrush;
        InsideElements = insideElements;
    }
}

public class SolarEclipseService
{
    private static readonly Dictionary<FrameworkElement, BackgroundInfo> backgroundInfos = new Dictionary<FrameworkElement, BackgroundInfo>();

    public void AddElement(FrameworkElement element)
    {
        var group = new DrawingGroup();
        DrawingBrush burntLeafDrowingBrush = new DrawingBrush(group);

        GeometryDrawing backgroundGeometryDrawing = new GeometryDrawing();
        var backgroundBrushBack = new SolidColorBrush((Color)Application.Current.MainWindow.FindResource("Light"));
        backgroundGeometryDrawing.Brush = backgroundBrushBack;
        backgroundGeometryDrawing.Geometry = new RectangleGeometry(new Rect(0,0,100,100));
        group.Children.Add(backgroundGeometryDrawing);

        GeometryDrawing geometryDrawing = new GeometryDrawing();
        var backgroundBrushFront = new SolidColorBrush((Color)Application.Current.MainWindow.FindResource("Light"));
        geometryDrawing.Brush = backgroundBrushFront;
        var pathGeometry = new PathGeometry();
        geometryDrawing.Geometry = pathGeometry;
        var topRightToLeftPoint = new PathFigure();
        topRightToLeftPoint.StartPoint = new Point(100, 0);
        pathGeometry.Figures.Add(topRightToLeftPoint);
        topRightToLeftPoint.Segments.Add(new LineSegment(new Point(100,0), false));
        var rightUpToDownPoint = new LineSegment(new Point(100, 0), false);
        topRightToLeftPoint.Segments.Add(rightUpToDownPoint);
        var downRightToLeftPoint = new LineSegment(new Point(100, 100), false);
        topRightToLeftPoint.Segments.Add(downRightToLeftPoint);
        var sezierSegment = new BezierSegment(new Point(100, 0), new Point(100, 0), new Point(100, 0), false);
        topRightToLeftPoint.Segments.Add(sezierSegment);
        group.Children.Add(geometryDrawing);



        Dictionary<string, DependencyObject> insideElements = new Dictionary<string, DependencyObject>();
        insideElements.Add("BackgroundGeometryDrawing", backgroundGeometryDrawing);
        insideElements.Add("BackgroundBrushBack", backgroundBrushBack);
        insideElements.Add("BackgroundBrushFront", backgroundBrushFront);
        insideElements.Add("TopRightToLeftPoint", topRightToLeftPoint);
        insideElements.Add("RightUpToDownPoint", rightUpToDownPoint);
        insideElements.Add("DownRightToLeftPoint", downRightToLeftPoint);
        insideElements.Add("SezierSegment", sezierSegment);

        backgroundInfos.Add(element, new BackgroundInfo(burntLeafDrowingBrush, insideElements));

        ((Border)element).Background = burntLeafDrowingBrush;
    }

    public void RemoveElement(FrameworkElement element)
    {
        throw new NotImplementedException();
    }


    public static void ChangeTheme(Color testNewColor)
    {
        foreach (var backgroundInfo in backgroundInfos)
        {
            var element = backgroundInfo.Key;
            var info = backgroundInfo.Value;

            element.BurntLeafDrowingBrush(info, testNewColor);
        }
    }
}

internal static class LatteBackgroudAnimationsHalper
{

    public static void BurntLeafDrowingBrush(this FrameworkElement element, BackgroundInfo backgroundInfo, Color testNewColor)
    {
        int fullSeconds = 4;

        PathFigure topRightToLeftPoint = (PathFigure)backgroundInfo.InsideElements["TopRightToLeftPoint"];
        LineSegment rightUpToDownPoint = (LineSegment)backgroundInfo.InsideElements["RightUpToDownPoint"];
        LineSegment downRightToLeftPoint = (LineSegment)backgroundInfo.InsideElements["DownRightToLeftPoint"];
        BezierSegment sezierSegment = (BezierSegment)backgroundInfo.InsideElements["SezierSegment"];
        SolidColorBrush backgroundBrushBack = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushBack"];
        SolidColorBrush backgroundBrushFront = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushFront"];
       
        //// ------------------------------------------------------------------------------------------

        backgroundBrushFront.Color = testNewColor;

        //// ------------------------------------------------------------------------------------------

        #region topRightToLeftPointAnimation

        var topRightToLeftPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        Storyboard.SetTarget(topRightToLeftPointAnimation, topRightToLeftPoint);
        Storyboard.SetTargetProperty(topRightToLeftPointAnimation, new PropertyPath(PathFigure.StartPointProperty));

        #endregion

        #region rightUpToDownPointAnimation

        var rightUpToDownPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds / 2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 100, Y = 100 }
        };

        Storyboard.SetTarget(rightUpToDownPointAnimation, rightUpToDownPoint);
        Storyboard.SetTargetProperty(rightUpToDownPointAnimation, new PropertyPath(LineSegment.PointProperty));

        #endregion

        #region downRightToLeftPointAnimation

        var downRightToLeftPointAnimation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 100 },
            To = new Point { X = 0, Y = 100 }
        };

        Storyboard.SetTarget(downRightToLeftPointAnimation, downRightToLeftPoint);
        Storyboard.SetTargetProperty(downRightToLeftPointAnimation, new PropertyPath(LineSegment.PointProperty));

        #endregion

        //// ------------------------------------------------------------------------------------------

        #region sezierSegment1Animation

        var sezierSegment1Animation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 100 }
        };

        Storyboard.SetTarget(sezierSegment1Animation, sezierSegment);
        Storyboard.SetTargetProperty(sezierSegment1Animation, new PropertyPath(BezierSegment.Point1Property));

        #endregion

        #region sezierSegment2Animation

        var sezierSegment2Animation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 4),
            Duration = TimeSpan.FromSeconds(fullSeconds / 2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        Storyboard.SetTarget(sezierSegment2Animation, sezierSegment);
        Storyboard.SetTargetProperty(sezierSegment2Animation, new PropertyPath(BezierSegment.Point2Property));

        #endregion

        #region sezierSegment3Animation

        var sezierSegment3Animation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds / 4),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        Storyboard.SetTarget(sezierSegment3Animation, sezierSegment);
        Storyboard.SetTargetProperty(sezierSegment3Animation, new PropertyPath(BezierSegment.Point3Property));

        #endregion

        // ------------------------------------------------------------------------------------------
        //var colorAnimationUsingKeyFrames = new ColorAnimationUsingKeyFrames();
        //colorAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteColorKeyFrame()
        //{
        //    Value = testNewColor,
        //    KeyTime = TimeSpan.FromSeconds(fullSeconds + 2)
        //});

        //backgroundGeometryDrawing.Brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimationUsingKeyFrames);


        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(topRightToLeftPointAnimation);
        storyboard.Children.Add(rightUpToDownPointAnimation);
        storyboard.Children.Add(downRightToLeftPointAnimation);

        storyboard.Children.Add(sezierSegment1Animation);
        storyboard.Children.Add(sezierSegment2Animation);
        storyboard.Children.Add(sezierSegment3Animation);
        storyboard.Begin();
    }
}