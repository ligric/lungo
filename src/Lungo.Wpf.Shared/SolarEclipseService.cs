﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public FrameworkElement? Element { get; }
    public double Length { get; }
    public Point Point { get; }
    public Side Side { get; }

    public static readonly LengthSide Empty = new LengthSide();

    private LengthSide() { }

    public LengthSide(FrameworkElement element, double length, Point point, Side side)
    {
        Element = element;
        Length = length;
        Point = point;
        Side = side;
    }
}

public class SolarEclipseService
{
    private static readonly Dictionary<FrameworkElement, BackgroundInfo> backgroundInfos = new Dictionary<FrameworkElement, BackgroundInfo>();

    public void AddElement(FrameworkElement element)
    {
        var rotateTransform = new RotateTransform();
        var group = new DrawingGroup();
        group.Transform = rotateTransform;
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
        BezierSegment sezierSegment = new BezierSegment(new Point(100, 0), new Point(100, 0), new Point(100, 0), true);
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
        insideElements.Add("RotateTransform", rotateTransform);

        backgroundInfos.Add(element, new BackgroundInfo(burntLeafDrowingBrush, insideElements));

        if (element is Panel border)
        {
            border.Background = burntLeafDrowingBrush;
        }
        else if (element is Control control)
        {
            control.Background = burntLeafDrowingBrush;
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
        List<LengthSide> elementLengths = new List<LengthSide>();

        foreach (var elementKeyValuePair in backgroundInfos)
        {
            FrameworkElement element = elementKeyValuePair.Key;
            BackgroundInfo backgroundInfo = elementKeyValuePair.Value;

            GeneralTransform generalTransform = element.TransformToVisual((Visual)element.Parent);
            Rect rectTo = generalTransform.TransformBounds(new Rect(element.RenderSize));

            if (fromElementRect.Top >= rectTo.Bottom)
            {
                elementLengths.Add(GetLengthFromAbove(element, fromElementRect, rectTo));
            }
            else
            {
                elementLengths.Add(GetLengthFromUnder(element, fromElementRect, rectTo));
            }
        }

        LengthSide[]? sortedTopRight = elementLengths.Where(x => x.Side == Side.TopRight).OrderByDescending(x => x.Length).ToArray();
        LengthSide[]? sortedBottomRight = elementLengths.Where(x => x.Side == Side.BottomRight).OrderByDescending(x => x.Length).ToArray();
        LengthSide[]? sortedBottomLeft = elementLengths.Where(x => x.Side == Side.BottomLeft).OrderByDescending(x => x.Length).ToArray();
        LengthSide[]? sortedTopLeft = elementLengths.Where(x => x.Side == Side.TopLeft).OrderByDescending(x => x.Length).ToArray();

        List<LengthSide[]> sortedParts = new List<LengthSide[]>();

        if (sortedTopRight.Length > 0)
            sortedParts.Add(sortedTopRight);

        if (sortedBottomRight.Length > 0)
            sortedParts.Add(sortedBottomRight);

        if (sortedBottomLeft.Length > 0)
            sortedParts.Add(sortedBottomLeft);

        if (sortedTopLeft.Length > 0)
            sortedParts.Add(sortedTopLeft);

        List<LengthSide[]> rings = new List<LengthSide[]>();
        List<LengthSide> ring = new List<LengthSide>();

        //List<LengthSide> bufferRing;
        //LengthSide startRingElement;
        //LengthSide oldLength = maxLength;

        if (sortedParts[0]?[0] is null)
            return;

        Side startSide = sortedParts[0][0].Side;
        LengthSide lastLength = LengthSide.Empty;


        foreach (LengthSide?[] sortedPart in sortedParts)
        {
            for (int i = 0; i < sortedPart.Length; i++)
            {
                var item = sortedPart[i];
                var nextItem = sortedPart[i + 1];
                if (item is null)
                    continue;

                if (lastLength == LengthSide.Empty)
                {
                    lastLength = item;
                    ring.Add(item);
                    continue;
                }

                if (item.Side == Side.TopRight)
                {
                    if (item.Point.X >= lastLength.Point.X 
                        && item.Point.Y >= lastLength.Point.Y
                        && item.Point.X <= nextItem.Point.X
                        && item.Point.Y >= nextItem.Point.Y)
                        continue;

                    if (item.Side == startSide)
                    {
                        rings.Add(ring.ToArray());
                        lastLength = LengthSide.Empty;
                    }

                    ring.Add(item);
                    sortedPart[i] = null;
                    lastLength = item;
                }

                if (item.Side == Side.BottomRight || item.Side == Side.BottomLeft)
                {
                    if (item.Point.X <= lastLength.Point.X)
                    {
                        if (item.Side == startSide)
                        {
                            rings.Add(ring.ToArray());
                            lastLength = LengthSide.Empty;
                        }
                        continue;
                    }

                    ring.Add(item);
                    sortedPart[i] = null;
                    lastLength = item;
                }
            }
        }

        // 500px - 1 sec



        //foreach (var item in sortedElementLengths)
        //{
        //    FrameworkElement element = item.Element;
        //    double length = item.Length;
        //    Side side = item.Side;
        //    backgroundInfos[element].BurntLeafDrowingBrush(testNewColor, side, (500 / 1) * length);
        //    //await Task.Delay(200); // Test
        //}
    }

    private static LengthSide GetLengthFromAbove(FrameworkElement testElement, Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(testElement, rectFrom.Top - rectTo.Bottom, rectTo.BottomRight, Side.BottomRight); 
            }
            else
            {
                return new LengthSide(testElement, Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), rectTo.BottomRight, Side.BottomRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(testElement, rectFrom.Top - rectTo.Bottom, rectTo.BottomLeft, Side.BottomLeft);
            }
            else
            {
                return new LengthSide(testElement, Math.Sqrt(Math.Pow(rectFrom.TopRight.X - rectTo.BottomLeft.X, 2) + Math.Pow(rectFrom.TopRight.Y - rectTo.BottomLeft.Y, 2)), rectTo.BottomLeft, Side.BottomLeft);
            }
        }
    }

    private static LengthSide GetLengthFromUnder(FrameworkElement testElement, Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(testElement, rectTo.Top - rectFrom.Bottom, rectTo.TopRight, Side.TopRight);
            }
            else
            {
                return new LengthSide(testElement, Math.Sqrt(Math.Pow(rectTo.TopRight.X - rectFrom.BottomLeft.X, 2) + Math.Pow(rectTo.TopRight.Y - rectFrom.BottomLeft.Y, 2)), rectTo.TopRight, Side.TopRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(testElement, rectTo.Top - rectFrom.Bottom, rectTo.TopLeft, Side.TopLeft);
            }
            else
            {
                return new LengthSide(testElement, Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), rectTo.TopLeft, Side.TopLeft);
            }
        }
    }
}

internal static class LungoBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this BackgroundInfo backgroundInfo, Color testNewColor, Side side = Side.TopRight, double fullSeconds = 1)
    {
        Point[] selectedPoints = new Point[] { new Point(100,0), new Point(100,100), new Point(0,0), new Point(0,100) };

        RotateTransform rotateTransform = (RotateTransform)backgroundInfo.InsideElements["RotateTransform"];
        PathFigure topRightToLeftPoint = (PathFigure)backgroundInfo.InsideElements["TopRightToLeftPoint"];
        LineSegment rightUpToDownPoint = (LineSegment)backgroundInfo.InsideElements["RightUpToDownPoint"];
        LineSegment downRightToLeftPoint = (LineSegment)backgroundInfo.InsideElements["DownRightToLeftPoint"];
        BezierSegment sezierSegment = (BezierSegment)backgroundInfo.InsideElements["SezierSegment"];

        SolidColorBrush backgroundBrushBack = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushBack"];
        SolidColorBrush backgroundBrushFront = (SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushFront"];

        //// ------------------------------------------------------------------------------------------
        if (side == Side.TopRight)
            rotateTransform.Angle = 0;

        if (side == Side.BottomRight)
            rotateTransform.Angle = 90;

        if (side == Side.BottomLeft)
            rotateTransform.Angle = 180;

        if (side == Side.TopLeft)
            rotateTransform.Angle = 270;

        backgroundBrushFront.Color = testNewColor;
        //topRightToLeftPoint.StartPoint = selectedPoints[0];
        sezierSegment.Point1 = selectedPoints[0];
        sezierSegment.Point2 = selectedPoints[0];
        downRightToLeftPoint.Point = selectedPoints[1];

        //// ------------------------------------------------------------------------------------------

        #region topRightToLeftPointAnimation

        var topRightToLeftPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = selectedPoints[0], // [100,0]  [100,100]
            To = selectedPoints[2] // [0,0]  [100,0]
        };

        topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, topRightToLeftPointAnimation);

        #endregion

        #region rightUpToDownPointAnimation

        var rightUpToDownPointAnimation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds / 2),
            From = selectedPoints[0], // [100,0]  [100,100]
            To = selectedPoints[1] // [100,100]  [0,100]
        };

        rightUpToDownPoint.BeginAnimation(LineSegment.PointProperty, rightUpToDownPointAnimation);

        #endregion

        #region downRightToLeftPointAnimation

        var downRightToLeftPointAnimation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = selectedPoints[1], // [100,100]  [0,100]
            To = selectedPoints[3] // [0,100]  [0,0]
        };

        downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, downRightToLeftPointAnimation);

        #endregion

        //// ------------------------------------------------------------------------------------------

        #region sezierSegment1Animation

        var sezierSegment1Animation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 2),
            Duration = TimeSpan.FromSeconds(fullSeconds),
            From = selectedPoints[0],
            To = selectedPoints[3]
        };

        #endregion

        #region sezierSegment2Animation

        var sezierSegment2Animation = new PointAnimation()
        {
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 4),
            Duration = TimeSpan.FromSeconds(fullSeconds / 2),
            From = selectedPoints[0],
            To = selectedPoints[2]
        };

        sezierSegment.BeginAnimation(BezierSegment.Point2Property, sezierSegment2Animation);

        #endregion

        #region sezierSegment3Animation

        var sezierSegment3Animation = new PointAnimation()
        {
            Duration = TimeSpan.FromSeconds(fullSeconds / 4),
            From = selectedPoints[0],
            To = selectedPoints[2]
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