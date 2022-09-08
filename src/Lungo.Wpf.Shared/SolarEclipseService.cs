using System;
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
        Dictionary<string, DependencyObject> insideElements;
        VisualBrush brush;

        if (element is Panel border)
        {
            brush = AFasfasfasa.GetEllipseVisualBrush(((SolidColorBrush)border.Background)?.Color, out insideElements);
            border.Background = brush;
        }
        else if (element is Control control)
        {
            brush = AFasfasfasa.GetEllipseVisualBrush(((SolidColorBrush)control.Background)?.Color, out insideElements);
            control.Background = brush;
        }
        else if (element is System.Windows.Shapes.Shape shape)
        {
            brush = AFasfasfasa.GetEllipseVisualBrush(((SolidColorBrush)shape.Fill)?.Color, out insideElements);
            shape.Fill = brush;
        }
        else
        {
            brush = AFasfasfasa.GetEllipseVisualBrush(((SolidColorBrush)((Border)element).Background)?.Color, out insideElements);
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
        Rect changerElementRect = changerElement.GetElementRectFromParent();
        var centerX = changerElementRect.Left + changerElement.ActualWidth / 2;
        var centerY = changerElementRect.Top + changerElement.ActualHeight / 2;

        foreach (KeyValuePair<FrameworkElement, BackgroundInfo> item in backgroundInfos)
        {
            FrameworkElement frameworkElement = item.Key;
            BackgroundInfo backgroundInfo = item.Value;

            VisualBrush rootVisualBrush = (VisualBrush)backgroundInfo.InsideElements["RootVisualBrush"];
            VisualBrush visualBrush = (VisualBrush)backgroundInfo.InsideElements["ContentVisualBrush"];
            Border border = (Border)backgroundInfo.InsideElements["Border"];
            System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)backgroundInfo.InsideElements["Path"];
            System.Windows.Shapes.Rectangle rectangle = (System.Windows.Shapes.Rectangle)backgroundInfo.InsideElements["Rectangle"];
            path.Fill = new SolidColorBrush(newColor);
            rectangle.Visibility = Visibility.Visible;

            Rect elementRect = frameworkElement.GetElementRectFromParent();
            double elementCoefficientX = elementRect.Left / border.ActualWidth;
            double elementCoefficientY = elementRect.Top / border.ActualHeight;
            rootVisualBrush.Viewbox = new Rect(elementCoefficientX, elementCoefficientY, 1, 1);


            double coefficientX = (centerX - path.ActualWidth / 2) / path.ActualWidth;
            double coefficientY = (centerY - path.ActualHeight / 2) / path.ActualHeight;
            visualBrush.Viewbox = new Rect(-coefficientX, -coefficientY, 1, 1);


            //----------------------------------------------------------------------------------------
            //                                    Animation
            //----------------------------------------------------------------------------------------

            double sizedWidth = Application.Current.MainWindow.ActualWidth * 3;
            double sizedHeight = Application.Current.MainWindow.ActualHeight * 3;


            var pathWidthformDoubleAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(10_000),
                To = sizedWidth
            };

            var pathHeightformDoubleAnimation = new DoubleAnimation()
            {
                
                Duration = TimeSpan.FromMilliseconds(10_000),
                To = sizedHeight
            };

            pathWidthformDoubleAnimation.Completed += (s, e) =>
            {
                border.Background = new SolidColorBrush(newColor);
                path.BeginAnimation(FrameworkElement.WidthProperty, null);
                path.BeginAnimation(FrameworkElement.HeightProperty, null);
                visualBrush.BeginAnimation(TileBrush.ViewboxProperty, null);
                rectangle.Visibility = Visibility.Collapsed;
            };

            path.BeginAnimation(FrameworkElement.WidthProperty, pathWidthformDoubleAnimation);
            path.BeginAnimation(FrameworkElement.HeightProperty, pathHeightformDoubleAnimation);



            double windowCenterX = Application.Current.MainWindow.ActualWidth / 2;
            double windowCenterY = Application.Current.MainWindow.ActualHeight / 2;

            double testX = (windowCenterX - sizedWidth / 2) / sizedWidth;
            double textY = (windowCenterY - sizedHeight / 2) / sizedHeight;


            var contentVisualBrushRectAnimation = new RectAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(5_000),
                To = new Rect(-testX, -textY, 1, 1)
            };

            visualBrush.BeginAnimation(TileBrush.ViewboxProperty, contentVisualBrushRectAnimation);
        }
    }
}

internal static class FrameworkElementExtansions
{
    public static Rect GetElementRectFromScreen(this FrameworkElement element)
    {
        Point elementPoint = element.PointToScreen(new Point(0, 0));
        return new Rect(elementPoint, new Point(elementPoint.X + element.ActualWidth, elementPoint.Y + element.ActualHeight));
    }

    public static Rect GetElementRectFromWindow(this FrameworkElement element)
    {
        GeneralTransform generalTransform = element.TransformToVisual(Application.Current.MainWindow);
        return generalTransform.TransformBounds(new Rect(element.RenderSize));
    }

    public static Rect GetElementRectFromParent(this FrameworkElement element)
    {
        GeneralTransform generalTransform = element.TransformToVisual((Visual)element.Parent);
        return generalTransform.TransformBounds(new Rect(element.RenderSize));
    }

    public static Rect GetElementRectFrom(this FrameworkElement element, Visual visual)
    {
        GeneralTransform generalTransform = element.TransformToVisual(visual);
        return generalTransform.TransformBounds(new Rect(element.RenderSize));
    }
}

//internal static class VisualBrushExtensions
//{
//    public static Rect GetElementCenterPoint(this FrameworkElement element)
//    {
//        Rect changerElementRect = element.GetElementRectFromParent();
//        var centerX = changerElementRect.Left + element.ActualWidth / 2;
//        var centerY = changerElementRect.Top + element.ActualHeight / 2;


//    }
//}

internal static class AFasfasfasa
{
    public static VisualBrush GetEllipseVisualBrush(Color? baseColor, out Dictionary<string, DependencyObject> insideElements)
    {
        System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
        {
            Fill = new SolidColorBrush(Colors.Green),
            Width = 100,
            Height = 100,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        path.Data = GetCirclePathGeometry(out Dictionary<string, DependencyObject> circlePathGeometryInsideElements);

        VisualBrush contentVisualBrush = new VisualBrush()
        {
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
            Stretch = Stretch.None
        };
        contentVisualBrush.Visual = path;

        System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
        rectangle.Fill = contentVisualBrush;
        rectangle.Visibility = Visibility.Collapsed;


        Border border = new Border()
        {
            Background = baseColor == null ? null : new SolidColorBrush((Color)baseColor)
        };
        border.SetBinding(Window.HeightProperty, new Binding("Height") { Source = Application.Current.MainWindow });
        border.SetBinding(Window.WidthProperty, new Binding("Width") { Source = Application.Current.MainWindow });
        border.Child = rectangle;

        VisualBrush rootVisualBrush = new VisualBrush()
        {
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
            Stretch = Stretch.None,
        };
        rootVisualBrush.Visual = border;

         
        insideElements = new Dictionary<string, DependencyObject>();
        insideElements.Add("ContentVisualBrush", contentVisualBrush);
        insideElements.Add("Rectangle", rectangle);
        insideElements.Add("Border", border);
        insideElements.Add("Path", path);
        insideElements.Add("RootVisualBrush", rootVisualBrush);
        foreach (var item in circlePathGeometryInsideElements) insideElements.Add(item.Key, item.Value);


        return rootVisualBrush;
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
