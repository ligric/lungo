using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lungo.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        private bool isDarkTheme = false;

        public MainWindow()
        {
            this.InitializeComponent();
        }


        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        public static IReadOnlyList<Point> GetPolygonVertices(int n, double r, Vector center)
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

        private Geometry GetCircleGeometry()
        {
            PathSegmentCollection pathSegments = new PathSegmentCollection();
            var points = GetPolygonVertices(8, 200, new Vector(200, 200));

            foreach (var point in points)
                pathSegments.Add(new ArcSegment(point, new Size(180, 180), 0, false, SweepDirection.Clockwise, true));

            pathSegments.Add(new ArcSegment(points[0], new Size(180, 180), 0, false, SweepDirection.Clockwise, true));

            PathFigure pathFigure = new PathFigure(points[0], pathSegments, true);
            PathFigureCollection pathFigures = new PathFigureCollection();
            pathFigures.Add(pathFigure);

            return new PathGeometry(pathFigures);
        }
    }
}
