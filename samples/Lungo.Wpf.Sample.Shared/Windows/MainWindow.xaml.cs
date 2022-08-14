using System.Windows;
using System.Windows.Media;

namespace Lungo.Wpf.Sample.Windows
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private bool isDarkTheme = false;
        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {
            GeneralTransform generalTransform = button.TransformToVisual((Visual)button.Parent);
            Rect rect = generalTransform.TransformBounds(new Rect(new Point(button.Margin.Left, button.Margin.Top), button.RenderSize));

            if (isDarkTheme)
            {
                SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Light"));
            }
            else
            {
                SolarEclipseService.ChangeTheme(rect, (Color)FindResource("Dark"));
            }

            isDarkTheme = !isDarkTheme;
        }



        //private static Brush FrameworkElementToImmutableBrush(FrameworkElement source)
        //{
        //    var currentRect = LayoutInformation.GetLayoutSlot(source);

        //    int width = (int)Math.Round(source.ActualWidth + source.Margin.Left + source.Margin.Right, MidpointRounding.AwayFromZero);
        //    int height = (int)Math.Round(source.ActualHeight + source.Margin.Top + source.Margin.Bottom, MidpointRounding.AwayFromZero);


        //    Size size = new Size(width, height);
        //    Rect rect = new Rect(size);

        //    source.Measure(size);
        //    source.Arrange(rect);
        //    source.UpdateLayout();

        //    RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
        //    rtb.Render(source);

        //    DrawingGroup drawing = new DrawingGroup();
        //    drawing.Children.Add(new ImageDrawing(rtb, rect));
        //    drawing.ClipGeometry = new RectangleGeometry(new Rect(source.Margin.Left, source.Margin.Top, source.ActualWidth, source.ActualHeight));

        //    ImageBrush brush = new ImageBrush(new DrawingImage(drawing));
        //    brush.Freeze();

        //    source.Measure(new Size(currentRect.Width, currentRect.Height));
        //    source.Arrange(currentRect);
        //    source.UpdateLayout();

        //    return brush;
        //}


    }
}
