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


//var topRightToLeftPointAnimation = new PointAnimation()
//{
//    Duration = TimeSpan.FromMilliseconds(milliseconds),
//    From = selectedPoints[0], // [100,0]  [100,100]
//    To = selectedPoints[2] // [0,0]  [100,0]
//};

//topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, topRightToLeftPointAnimation);