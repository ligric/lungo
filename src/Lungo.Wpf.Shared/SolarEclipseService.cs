﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Lungo.Wpf;

internal class BackgroundInfo
{
    public VisualBrush CurrentDrawingBrush { get; }

    public IReadOnlyDictionary<string, DependencyObject> InsideElements { get; }

    public BackgroundInfo(VisualBrush currentDrawingBrush, Dictionary<string, DependencyObject> insideElements)
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
        VisualBrush brush = AFasfasfasa.GetEllipseVisualBrush(out Dictionary<string, DependencyObject> insideElements);

        if (element is Panel border)
        {
            border.Background = brush;
        }
        else if (element is Control control)
        {
            control.Background = brush;
        }
        else if (element is System.Windows.Shapes.Shape shape)
        {
            shape.Fill = brush;
        }
        else
        {
            ((Border)element).Background = brush;
        }

        backgroundInfos.Add(element, new BackgroundInfo(brush, insideElements));
    }

    public void RemoveElement(FrameworkElement element)
    {
        throw new NotImplementedException();
    }

    public static void ChangeTheme(FrameworkElement changerElement, Color newColor)
    {
        //Rect changerElementRect = changerElement.GetElementRectFromScreen();



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

internal static class AFasfasfasa
{
    public static VisualBrush GetEllipseVisualBrush(out Dictionary<string, DependencyObject> insideElements)
    {
        System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
        {
            Width = 400,
            Height = 400,
            Fill = new SolidColorBrush(Colors.Green),
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        path.Data = GetCirclePathGeometry(out Dictionary<string, DependencyObject> circlePathGeometryInsideElements);
         
        Border border = new Border()
        {
            Background = new SolidColorBrush(Colors.Red)
        };
        border.SetBinding(Window.ActualHeightProperty, new Binding("Height") { Source = Application.Current.MainWindow });
        border.SetBinding(Window.ActualWidthProperty, new Binding("Width") { Source = Application.Current.MainWindow });
        border.Child = path;

        VisualBrush ellipseVisualBrush = new VisualBrush()
        {
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
            Stretch = Stretch.None,
        };
        ellipseVisualBrush.Visual = border;


        insideElements = new Dictionary<string, DependencyObject>();
        insideElements.Add("Border", border);
        insideElements.Add("Path", path);
        insideElements.Add("VisualBrush", ellipseVisualBrush);
        foreach (var item in circlePathGeometryInsideElements) insideElements.Add(item.Key, item.Value);


        return ellipseVisualBrush;
    }

    private static IReadOnlyList<Point> GetPolygonVertices(int n, double r, Vector center)
    {
        Point[] points = new Point[n];
        double segmentAngle = 2 * Math.PI / n; // Угол сегмента в радианах.
        for (int i = 0; i < n; i++)
        {
            double angle = segmentAngle * i; // Угол очередной вершины.
            Point vertic = new Point(r * Math.Cos(angle), r * Math.Sin(angle)); // Точка вершины без смещения центра.
            vertic += center; // Добавляем смещение центра.
            points[i] = vertic; // Запоминание точки вершины.
        }
        return Array.AsReadOnly(points); // Возврат вершин в массиве только для чтения.
    }

    private static PathGeometry GetCirclePathGeometry(out Dictionary<string, DependencyObject> insideElements)
    {
        insideElements = new Dictionary<string, DependencyObject>();
        PathSegmentCollection pathSegments = new PathSegmentCollection();
        var points = GetPolygonVertices(8, 200, new Vector(200, 200));

        for (int i = 0; i < points.Count; i++)
        {
            var arcSegmentItem = new ArcSegment(points[i], new Size(180, 180), 0, false, SweepDirection.Clockwise, true);
            pathSegments.Add(arcSegmentItem);
            insideElements.Add($"ArcSegment{i}", arcSegmentItem);
        }

        var arcSegment = new ArcSegment(points[0], new Size(180, 180), 0, false, SweepDirection.Clockwise, true);
        pathSegments.Add(arcSegment);
        insideElements.Add($"ArcSegment{points.Count}", arcSegment);


        PathFigure pathFigure = new PathFigure(points[0], pathSegments, true);
        insideElements.Add("PathFigure", pathFigure);


        PathFigureCollection pathFigures = new PathFigureCollection();
        pathFigures.Add(pathFigure);

        return new PathGeometry(pathFigures);
    }
}
//var topRightToLeftPointAnimation = new PointAnimation()
//{
//    Duration = TimeSpan.FromMilliseconds(milliseconds),
//    From = selectedPoints[0], // [100,0]  [100,100]
//    To = selectedPoints[2] // [0,0]  [100,0]
//};

//topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, topRightToLeftPointAnimation);