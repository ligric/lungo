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

    public static void ChangeTheme(FrameworkElement changerElement, Color newColor, double milliseconds = 1_000)
    {
        double GetR(Size size) =>
            Math.Sqrt(2 * (size.Width * size.Width) + 2 * (size.Height * size.Height));

        Rect changerElementRect = changerElement.GetElementRectFromParent();
        Point changerElementCenter = new Point(changerElementRect.Left + changerElement.ActualWidth / 2, changerElementRect.Top + changerElement.ActualHeight / 2);

        foreach (KeyValuePair<FrameworkElement, BackgroundInfo> item in backgroundInfos)
        {
            FrameworkElement frameworkElement = item.Key;
            BackgroundInfo backgroundInfo = item.Value;

            VisualBrush rootVisualBrush = (VisualBrush)backgroundInfo.InsideElements["RootVisualBrush"];
            Border border = (Border)backgroundInfo.InsideElements["Border"];
            ScaleTransform contentVisualBrushScaleTransform = (ScaleTransform)backgroundInfo.InsideElements["ContentVisualBrushScaleTransform"];
            TranslateTransform contentVisualBrushTranslateTransform = (TranslateTransform)backgroundInfo.InsideElements["ContentVisualBrushTranslateTransform"];
            System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)backgroundInfo.InsideElements["Path"];
            System.Windows.Shapes.Rectangle rectangle = (System.Windows.Shapes.Rectangle)backgroundInfo.InsideElements["Rectangle"];

            path.Fill = new SolidColorBrush(newColor);
            rectangle.Visibility = Visibility.Visible;

            Rect elementRect = frameworkElement.GetElementRectFromParent();
            double elementCoefficientX = elementRect.Left / border.ActualWidth;
            double elementCoefficientY = elementRect.Top / border.ActualHeight;
            rootVisualBrush.Viewbox = new Rect(elementCoefficientX, elementCoefficientY, 1, 1);

            //----------------------------------------------------------------------------------------
            //                                    Animation
            //----------------------------------------------------------------------------------------
            Size windowSize = Application.Current.MainWindow.RenderSize;
            double newContentVisualBrushScale = GetR(windowSize);

            var contentVisualBrushScaleTransformX = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(milliseconds),
                From = GetR(new Size(1, 1)),
                To = newContentVisualBrushScale,
            };

            var contentVisualBrushScaleTransformY = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(milliseconds),
                From = GetR(new Size(1, 1)),
                To = newContentVisualBrushScale,
            };


            var contentVisualBrushTranslateTransformX = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(milliseconds),
                From = changerElementCenter.X - GetR(new Size(1, 1)) / 2,
                To = changerElementCenter.X - newContentVisualBrushScale / 2,
            };

            var contentVisualBrushTranslateTransformY = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(milliseconds),
                From = changerElementCenter.Y - GetR(new Size(1, 1)) / 2,
                To = changerElementCenter.Y - newContentVisualBrushScale / 2,
            };

            contentVisualBrushScaleTransformX.Completed += (s, e) =>
            {
                border.Background = new SolidColorBrush(newColor);
                contentVisualBrushScaleTransformX.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                contentVisualBrushScaleTransformY.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                contentVisualBrushTranslateTransformX.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                contentVisualBrushTranslateTransformY.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                rectangle.Visibility = Visibility.Collapsed;
            };
            contentVisualBrushScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, contentVisualBrushScaleTransformX);
            contentVisualBrushScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, contentVisualBrushScaleTransformY);

            contentVisualBrushTranslateTransform.BeginAnimation(TranslateTransform.XProperty, contentVisualBrushTranslateTransformX);
            contentVisualBrushTranslateTransform.BeginAnimation(TranslateTransform.YProperty, contentVisualBrushTranslateTransformY);
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

internal static class AFasfasfasa
{
    public static VisualBrush GetEllipseVisualBrush(Color? baseColor, out Dictionary<string, DependencyObject> insideElements)
    {
        System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
        {
            Fill = new SolidColorBrush(Colors.Green),
            Width = 1,
            Height = 1,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Data = GetCirclePathGeometry(out Dictionary<string, DependencyObject> circlePathGeometryInsideElements)
        };

        ScaleTransform contentVisualBrushScaleTransform = new ScaleTransform(1, 1);
        TranslateTransform contentVisualBrushTranslateTransform = new TranslateTransform();
        TransformGroup contentVisualBrushTransformGroup = new TransformGroup();
        contentVisualBrushTransformGroup.Children.Add(contentVisualBrushScaleTransform);
        contentVisualBrushTransformGroup.Children.Add(contentVisualBrushTranslateTransform);
        VisualBrush contentVisualBrush = new VisualBrush()
        {
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
            Stretch = Stretch.None,
            Visual = path,
            Transform = contentVisualBrushTransformGroup,
        };

        System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle()
        {
            Fill = contentVisualBrush,
            Visibility = Visibility.Collapsed
        };
        
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
            Visual = border
        };
         
        insideElements = new Dictionary<string, DependencyObject>();
        insideElements.Add("ContentVisualBrushScaleTransform", contentVisualBrushScaleTransform);
        insideElements.Add("ContentVisualBrushTranslateTransform", contentVisualBrushTranslateTransform);
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
