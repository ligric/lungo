using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

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

    public LengthSide(FrameworkElement element, double lengthToMainPoint, Point point, Side side)
    {
        Element = element;
        Length = lengthToMainPoint;
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
        backgroundGeometryDrawing.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
        group.Children.Add(backgroundGeometryDrawing);

        GeometryDrawing geometryDrawing = new GeometryDrawing();
        var backgroundBrushFront = new SolidColorBrush((Color)Application.Current.MainWindow.FindResource("Light"));
        geometryDrawing.Brush = backgroundBrushFront;
        var pathGeometry = new PathGeometry();
        geometryDrawing.Geometry = pathGeometry;
        var topRightToLeftPoint = new PathFigure();
        topRightToLeftPoint.StartPoint = new Point(100, 0);
        pathGeometry.Figures.Add(topRightToLeftPoint);
        topRightToLeftPoint.Segments.Add(new LineSegment(new Point(100, 0), true));
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
        else if (element is System.Windows.Shapes.Shape shape)
        {
            shape.Fill = burntLeafDrowingBrush;
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

    public static void ChangeTheme(FrameworkElement changerElement, Color newColor)
    {
        Rect changerElementRect = changerElement.GetElementRectFromScreen();
        LengthSide[] minorElementLengthsToChangerElement = new LengthSide[backgroundInfos.Count];

        int i = 0;
        foreach (FrameworkElement element in backgroundInfos.Keys)
        {
            Rect minorElementRect = element.GetElementRectFromScreen();
            minorElementLengthsToChangerElement[i] = changerElementRect.Top >= minorElementRect.Bottom ? GetLengthFromAbove(element, changerElementRect, minorElementRect) 
                : GetLengthFromUnder(element, changerElementRect, minorElementRect);
            i++;
        }

        int milliseconds = 2000;
       
        double maxRightBottomElementsHeight = 0, maxTopRightElementsHeight = 0, maxTopLeftElementsHeight = 0, maxLeftBottomElementsHeight = 0,
               maxRightBottomElementsWidth = 0, maxTopRightElementsWidth = 0, maxTopLeftElementsWidth = 0, maxLeftBottomElementsWidth = 0;

        foreach (LengthSide lengthSide in minorElementLengthsToChangerElement)
        {
            FrameworkElement element = lengthSide.Element;
            switch (lengthSide.Side)
            {
                case Side.TopLeft:
                    maxTopLeftElementsHeight += element.ActualHeight;
                    maxTopLeftElementsWidth += element.ActualWidth;
                    break;
                case Side.TopRight:
                    maxTopRightElementsHeight += element.ActualHeight;
                    maxTopRightElementsWidth += element.ActualWidth;
                    break;
                case Side.BottomLeft:
                    maxLeftBottomElementsHeight += element.ActualHeight;
                    maxLeftBottomElementsWidth += element.ActualWidth;
                    break;
                case Side.BottomRight:
                    maxRightBottomElementsHeight += element.ActualHeight;
                    maxRightBottomElementsWidth += element.ActualWidth;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        double GetDiagonal(double a, double b)
        {
            return Math.Sqrt((a * a) + (b * b));
        }

        double maxRightBottomDiagonal = GetDiagonal(maxRightBottomElementsHeight, maxRightBottomElementsWidth),
               maxTopRightDiagonal = GetDiagonal(maxTopRightElementsHeight, maxTopRightElementsWidth),
               maxTopLeftDiagonal = GetDiagonal(maxTopLeftElementsHeight, maxTopLeftElementsWidth),
               maxLeftBottomDiagonal = GetDiagonal(maxLeftBottomElementsHeight, maxLeftBottomElementsWidth);




        foreach (var minor in minorElementLengthsToChangerElement)
        {
            //Task.Run(async () =>
            //{
            //    double length = minor.Length;
            //    double parentWidth =
            //    await Task.Delay(millisecond);
            //});
        }
    }

    private static LengthSide GetLengthFromAbove(FrameworkElement minorElement, Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(minorElement, rectFrom.Top - rectTo.Bottom, rectTo.BottomRight, Side.BottomRight);
            }
            else
            {
                return new LengthSide(minorElement, Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), rectTo.BottomRight, Side.BottomRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(minorElement, rectFrom.Top - rectTo.Bottom, rectTo.BottomLeft, Side.BottomLeft);
            }
            else
            {
                return new LengthSide(minorElement, Math.Sqrt(Math.Pow(rectFrom.TopRight.X - rectTo.BottomLeft.X, 2) + Math.Pow(rectFrom.TopRight.Y - rectTo.BottomLeft.Y, 2)), rectTo.BottomLeft, Side.BottomLeft);
            }
        }
    }

    private static LengthSide GetLengthFromUnder(FrameworkElement minorElement, Rect rectFrom, Rect rectTo)
    {
        if (rectTo.Left <= rectFrom.Left)
        {
            if (rectTo.Right >= rectFrom.Left)
            {
                return new LengthSide(minorElement, rectTo.Top - rectFrom.Bottom, rectTo.TopRight, Side.TopRight);
            }
            else
            {
                return new LengthSide(minorElement, Math.Sqrt(Math.Pow(rectTo.TopRight.X - rectFrom.BottomLeft.X, 2) + Math.Pow(rectTo.TopRight.Y - rectFrom.BottomLeft.Y, 2)), rectTo.TopRight, Side.TopRight);
            }
        }
        else
        {
            if (rectTo.Left <= rectFrom.Right)
            {
                return new LengthSide(minorElement, rectTo.Top - rectFrom.Bottom, rectTo.TopLeft, Side.TopLeft);
            }
            else
            {
                return new LengthSide(minorElement, Math.Sqrt(Math.Pow(rectFrom.TopLeft.X - rectTo.BottomRight.X, 2) + Math.Pow(rectFrom.TopLeft.Y - rectTo.BottomRight.Y, 2)), rectTo.TopLeft, Side.TopLeft);
            }
        }
    }
}

internal static class LungoBackgroudAnimationsHalper
{
    public static void BurntLeafDrowingBrush(this BackgroundInfo backgroundInfo, Color testNewColor, Side side = Side.TopRight, double fullSeconds = 1)
    {
        Point[] selectedPoints = new Point[] { new Point(100, 0), new Point(100, 100), new Point(0, 0), new Point(0, 100) };

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
            BeginTime = TimeSpan.FromSeconds(fullSeconds / 2),
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

internal static class FrameworkElementExtansions
{
    public static Rect GetElementRectFromScreen(this FrameworkElement element)
    {
        Point elementPoint = element.PointToScreen(new Point(0, 0));
        return new Rect(elementPoint, new Point(elementPoint.X + element.ActualWidth, elementPoint.Y + element.ActualHeight));
    }
}