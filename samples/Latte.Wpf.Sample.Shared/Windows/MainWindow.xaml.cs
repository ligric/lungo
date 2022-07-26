using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Latte.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //grid.Children.Add(rectangle);
            //rectangle.Visibility = Visibility.Hidden;
        }

        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {
            //EllipseGeometry ellipse = new EllipseGeometry();
            //Border0.Background = ellipse;

            //Border5.SetResourceReference(BackgroundProperty, "Dark");
        }



        private static Storyboard AddEllipsGeometryBackground(FrameworkElement element)
        {

            return null;
        }




        private static Brush FrameworkElementToImmutableBrush(FrameworkElement source)
        {
            var currentRect = LayoutInformation.GetLayoutSlot(source);

            int width = (int)Math.Round(source.ActualWidth + source.Margin.Left + source.Margin.Right, MidpointRounding.AwayFromZero);
            int height = (int)Math.Round(source.ActualHeight + source.Margin.Top + source.Margin.Bottom, MidpointRounding.AwayFromZero);


            Size size = new Size(width, height);
            Rect rect = new Rect(size);

            source.Measure(size);
            source.Arrange(rect);
            source.UpdateLayout();

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            rtb.Render(source);

            DrawingGroup drawing = new DrawingGroup();
            drawing.Children.Add(new ImageDrawing(rtb, rect));
            drawing.ClipGeometry = new RectangleGeometry(new Rect(source.Margin.Left, source.Margin.Top, source.ActualWidth, source.ActualHeight));

            ImageBrush brush = new ImageBrush(new DrawingImage(drawing));
            brush.Freeze();

            source.Measure(new Size(currentRect.Width, currentRect.Height));
            source.Arrange(currentRect);
            source.UpdateLayout();

            return brush;
        }


    }
}
