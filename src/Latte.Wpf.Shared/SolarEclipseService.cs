using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        topRightToLeftPoint.Segments.Add(new LineSegment(new Point(100,0), true));
        var rightUpToDownPoint = new LineSegment(new Point(100, 0), true);
        topRightToLeftPoint.Segments.Add(rightUpToDownPoint);
        var downRightToLeftPoint = new LineSegment(new Point(100, 100), true);
        topRightToLeftPoint.Segments.Add(downRightToLeftPoint);
        var sezierSegment = new BezierSegment(new Point(100, 0), new Point(100, 0), new Point(100, 0), true);
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

    public static async void ChangeTheme(Rect fromElementRect, Color testNewColor)
    {
        //Point fromElemenCenter = new Point(fromElementRect.Right - ((fromElementRect.Right - fromElementRect.Left) / 2),
        //                                 fromElementRect.Bottom - ((fromElementRect.Bottom - fromElementRect.Top) / 2));

        List<FrameworkElementPoint> frameworkElementPoints = new List<FrameworkElementPoint>();
        foreach (var item in backgroundInfos.Keys)
        {
            GeneralTransform generalTransform = item.TransformToVisual((Visual)item.Parent);
            Rect rect = generalTransform.TransformBounds(new Rect(new Point(item.Margin.Left, item.Margin.Top), item.RenderSize));

            frameworkElementPoints.Add(new FrameworkElementPoint(item, rect.TopRight));
        }

        IOrderedEnumerable<FrameworkElementPoint> sorted = frameworkElementPoints.OrderByDescending(obj => obj.Point.X)
                                                                                 .ThenBy(obj => obj.Point.Y);

        foreach (FrameworkElementPoint item in sorted)
        {
            var element = item.Element;
            var info = backgroundInfos[element];

            element.BurntLeafDrowingBrush(info, testNewColor);

            await Task.Delay(300);
        }
    }
}

internal static class LatteBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this FrameworkElement element, BackgroundInfo backgroundInfo, Color testNewColor)
    {
        double fullSeconds = 0.5;

        PathFigure topRightToLeftPoint = (PathFigure)backgroundInfo.InsideElements["TopRightToLeftPoint"];
        LineSegment rightUpToDownPoint = (LineSegment)backgroundInfo.InsideElements["RightUpToDownPoint"];
        LineSegment downRightToLeftPoint = (LineSegment)backgroundInfo.InsideElements["DownRightToLeftPoint"];
        BezierSegment sezierSegment = (BezierSegment)backgroundInfo.InsideElements["SezierSegment"];
        SolidColorBrush backgroundBrushBack = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushBack"];
        SolidColorBrush backgroundBrushFront = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushFront"];

        //// ------------------------------------------------------------------------------------------
        backgroundBrushFront.Color = testNewColor;
        sezierSegment.Point1 = new Point(100, 0);
        sezierSegment.Point2 = new Point(100, 0);
        downRightToLeftPoint.Point = new Point(100,100);

        //// ------------------------------------------------------------------------------------------

        #region topRightToLeftPointAnimation

        var topRightToLeftPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, topRightToLeftPointAnimation);

        #endregion

        #region rightUpToDownPointAnimation

        var rightUpToDownPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds/2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 100, Y = 100 }
        };

        rightUpToDownPoint.BeginAnimation(LineSegment.PointProperty, rightUpToDownPointAnimation);

        #endregion

        #region downRightToLeftPointAnimation

        var downRightToLeftPointAnimation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = new Point { X = 100, Y = 100 },
            To = new Point { X = 0, Y = 100 }
        };

        downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, downRightToLeftPointAnimation);

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

        #endregion

        #region sezierSegment2Animation

        var sezierSegment2Animation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 4),
            Duration = TimeSpan.FromSeconds(fullSeconds / 2),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        sezierSegment.BeginAnimation(BezierSegment.Point2Property, sezierSegment2Animation);

        #endregion

        #region sezierSegment3Animation

        var sezierSegment3Animation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds / 4),
            From = new Point { X = 100, Y = 0 },
            To = new Point { X = 0, Y = 0 }
        };

        sezierSegment.BeginAnimation(BezierSegment.Point3Property, sezierSegment3Animation);

        #endregion

        sezierSegment1Animation.Completed += (s, e) =>
        {
            backgroundBrushBack.Color = testNewColor;
            sezierSegment.BeginAnimation(BezierSegment.Point1Property, null);
            sezierSegment.BeginAnimation(BezierSegment.Point2Property, null);
            downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, null);
        };

        sezierSegment.BeginAnimation(BezierSegment.Point1Property, sezierSegment1Animation);
    }
}

internal class FrameworkElementPoint
{
    public FrameworkElement Element { get; }
    public Point Point { get; }

    public FrameworkElementPoint(FrameworkElement frameworkElement, Point point)
    {
        Element = frameworkElement;
        Point = point;
    }
}