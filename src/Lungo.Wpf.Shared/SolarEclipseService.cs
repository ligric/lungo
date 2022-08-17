using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Lungo.Wpf;

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

internal enum Side
{
    TopLeft,
    TopRight,

    BottomLeft,
    BottomRight
}

internal class LengthSide
{
    public double Length { get; }
    public Side Side { get; }

    public LengthSide(double length, Side side)
    {
        Length = length;
        Side = side;
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

        if (element is Panel border)
        {
            border.Background = burntLeafDrowingBrush;
        }
        else
        {
            ((Border)element).Background = burntLeafDrowingBrush;
        }
    }

    public void RemoveElement(FrameworkElement element)
    {
        throw new NotImplementedException();
    }

    public static async void ChangeTheme(Rect fromElementRect, Color testNewColor)
    {
        foreach (var element in backgroundInfos.Keys)
        {
            GeneralTransform generalTransform = element.TransformToVisual((Visual)element.Parent);
            Rect rectTo = generalTransform.TransformBounds(new Rect(element.RenderSize));

            if (fromElementRect.Top >= rectTo.Bottom)
            {
                var testRes = GetLengthFromAbove(fromElementRect, rectTo);
                System.Diagnostics.Debug.WriteLine($"{element.Name} -- {testRes.Length} -- {testRes.Side}");
            }
            else
            {
                var testRes = GetLengthFromUnder(fromElementRect, rectTo);
                System.Diagnostics.Debug.WriteLine($"{element.Name} -- {testRes.Length} -- {testRes.Side}");
            }
        }
    }

    private static LengthSide GetLengthFromAbove(Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(rectFrom.Top - rectTo.Bottom, Side.BottomRight); 
            }
            else
            {
                return new LengthSide(Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), Side.BottomRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(rectFrom.Top - rectTo.Bottom, Side.BottomLeft);
            }
            else
            {
                return new LengthSide(Math.Sqrt(Math.Pow(rectFrom.TopRight.X - rectTo.BottomLeft.X, 2) + Math.Pow(rectFrom.TopRight.Y - rectTo.BottomLeft.Y, 2)), Side.BottomLeft);
            }
        }
    }

    private static LengthSide GetLengthFromUnder(Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(rectTo.Top - rectFrom.Bottom, Side.TopRight);
            }
            else
            {
                return new LengthSide(Math.Sqrt(Math.Pow(rectTo.TopRight.X - rectFrom.BottomLeft.X, 2) + Math.Pow(rectTo.TopRight.Y - rectFrom.BottomLeft.Y, 2)), Side.TopRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(rectTo.Top - rectFrom.Bottom, Side.TopLeft);
            }
            else
            {
                return new LengthSide(Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), Side.TopLeft);
            }
        }
    }
}

internal static class LungoBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this FrameworkElement element, BackgroundInfo backgroundInfo, Color testNewColor)
    {
        double fullSeconds = 0.2;

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

internal class ElementDistance
{
    public FrameworkElement Element { get; }
    public double DistanceLength { get; }

    public ElementDistance(FrameworkElement frameworkElement, double distanceLength)
    {
        Element = frameworkElement;
        DistanceLength = distanceLength;
    }
}