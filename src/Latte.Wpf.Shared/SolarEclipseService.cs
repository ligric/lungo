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
        ((SolidColorBrush)backgroundInfo.InsideElements["BackgroundBrushBack"]).Color = testNewColor;

        //var testBackground = test.Drawing.FindChild<DependencyObject>("BackgroundGeometryDrawing");


        //PathFigure topRightToLeftPoint = (PathFigure)element.FindResource("TopRightToLeftPoint");
        //LineSegment rightUpToDownPoint = (LineSegment)element.FindResource("RightUpToDownPoint");
        //LineSegment downRightToLeftPoint = (LineSegment)element.FindResource("DownRightToLeftPoint");
        //BezierSegment sezierSegment = (BezierSegment)element.FindResource("SezierSegment");


        //GeometryDrawing backgroundGeometryDrawing = (GeometryDrawing)element.FindResource("BackgroundGeometryDrawing");
        //((SolidColorBrush)(backgroundGeometryDrawing.Brush)).Color = testNewColor;        


        //var backgroundGeometryDrawing = element.FindResource("BackgroundBrushBack");
        //((SolidColorBrush)(backgroundGeometryDrawing.Brush)).Color = testNewColor;

        //SolidColorBrush backgroundBrushBack = (SolidColorBrush)element.FindResource("BackgroundBrushBack");
        //SolidColorBrush fackgroundBrushFront = (SolidColorBrush)element.FindResource("BackgroundBrushFront");
        //fackgroundBrushFront.Color = testNewColor;


        //var topRightToLeftPointAnimation =  new PointAnimation()
        //{
        //    Duration = TimeSpan.FromSeconds(fullSeconds),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 0, Y = 0 }
        //};

        //element.RegisterName("TopRightToLeftPoint", topRightToLeftPoint);
        //topRightToLeftPoint.StartPoint = new Point(100,0);

        //Storyboard.SetTarget(topRightToLeftPointAnimation, topRightToLeftPoint);
        //Storyboard.SetTargetProperty(topRightToLeftPointAnimation, new PropertyPath(PathFigure.StartPointProperty));


        //Storyboard test = new Storyboard();
        //test.Children.Add(topRightToLeftPointAnimation);
        //test.Begin();


        //topRightToLeftPoint.BeginAnimation(PathFigure.StartPointProperty, new PointAnimation()
        //{
        //    Duration = TimeSpan.FromSeconds(fullSeconds),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 0, Y = 0 }
        //});

        //rightUpToDownPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        //{
        //    Duration = TimeSpan.FromSeconds(fullSeconds/2),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 100, Y = 100 }
        //});

        //downRightToLeftPoint.BeginAnimation(LineSegment.PointProperty, new PointAnimation()
        //{
        //    BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
        //    Duration = TimeSpan.FromSeconds(fullSeconds),
        //    From = new Point { X = 100, Y = 100 },
        //    To = new Point { X = 0, Y = 100 }
        //});

        //// ------------------------------------------------------------------------------------------

        //sezierSegment.BeginAnimation(BezierSegment.Point1Property, new PointAnimation()
        //{
        //    BeginTime = TimeSpan.FromSeconds(fullSeconds/2),
        //    Duration = TimeSpan.FromSeconds(fullSeconds),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 0, Y = 100 }
        //});
        //sezierSegment.BeginAnimation(BezierSegment.Point2Property, new PointAnimation()
        //{
        //    BeginTime = TimeSpan.FromSeconds(fullSeconds/4),
        //    Duration = TimeSpan.FromSeconds(fullSeconds/2),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 0, Y = 0 }
        //});
        //sezierSegment.BeginAnimation(BezierSegment.Point3Property, new PointAnimation()
        //{
        //    Duration = TimeSpan.FromSeconds(fullSeconds/4),
        //    From = new Point { X = 100, Y = 0 },
        //    To = new Point { X = 0, Y = 0 }
        //});

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