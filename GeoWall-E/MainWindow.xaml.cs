global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Data;
global using System.Windows.Documents;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;
global using System.Windows.Navigation;
global using System.Windows.Shapes;
global using System.Windows.Threading;
using System.Windows.Media.Media3D;

namespace GeoWall_E
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     
    public partial class MainWindow : Window
    {
        public static Canvas DrawingCanvas { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            // Asignar el Canvas del XAML a la propiedad estática
            MainWindow.DrawingCanvas = this.drawingCanvas;
            this.WindowState = WindowState.Maximized;
            timer.Interval = TimeSpan.FromMilliseconds(100); // Ajusta este valor según sea necesario
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        
        {         
            // Borra el Canvas
            drawingCanvas.Children.Clear();
            // Obtén las líneas del TextBox
            Dictionary<string, Point> pointCenters = new Dictionary<string, Point>();

            // Asi se procesaria el codigo del usuario
            string code = Entrada.Text;
            var lexer = new Lexer(code);
            var parser = new Parser(lexer.Tokenize(), lexer.errors);
            var ast = parser.Parse_();
            if (ast.Errors.AnyError())
            {
                MessageBox.Show(ast.Errors.diagnostics[0], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                var evaluator = new Evaluator(ast.Root);
                if (Evaluator.Errors.AnyError())
                {
                    MessageBox.Show(Evaluator.Errors.diagnostics[0], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                var toDraw = evaluator.Evaluate();              // aqui se devolveria una List<Types> con el tipo de dato que hay que imprimir (point solo por ahora) revisa en la carpeta Types la clase Point
                                                    // despues de esto se puede hacer un foreach y dibujar cada uno de los elementos de la lista toDraw
                foreach (var item in toDraw)
                {

                    if (item is Point point)
                    {
                        point.Draw(drawingCanvas);
                        scrollViewer.ScrollToHorizontalOffset(point.X-400);
                        scrollViewer.ScrollToVerticalOffset(point.Y-250);
                    }
                    if (item is Line line)
                    {
                        line.Draw(drawingCanvas);
                        scrollViewer.ScrollToHorizontalOffset(line.P1.X-400);
                        scrollViewer.ScrollToVerticalOffset(line.P1.Y-250);
                    }
                    if (item is Segment segment)
                    {
                        segment.Draw(drawingCanvas);
                        scrollViewer.ScrollToHorizontalOffset(segment.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(segment.Start.Y - 250);
                    }
                    if (item is Ray ray)
                    {
                        ray.Draw(drawingCanvas);
                        scrollViewer.ScrollToHorizontalOffset(ray.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(ray.Start.Y - 250);
                    }
                    if (item is  Circle circle)
                    {
                        circle.Draw(drawingCanvas);
                        scrollViewer.ScrollToHorizontalOffset(circle.Center.X - 400);
                        scrollViewer.ScrollToVerticalOffset(circle.Center.Y - 250);
                    }
                }         
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Borra el TextBox
            Entrada.Text = "";

            // Borra el Canvas
            drawingCanvas.Children.Clear();


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // 'scrollViewer' se refiere al control ScrollViewer que has añadido en tu XAML
            scrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 40);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 40);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 40);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 40);
        }
        private void Button_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            timer.Tick += (s, e) => { scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 100); };
            timer.Start();
        }

        private void Button_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            timer.Tick -= (s, e) => { scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 100); };
        }

        private void Button_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            timer.Tick += (s, e) => { scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 100); };
            timer.Start();
        }

        private void Button_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            timer.Tick -= (s, e) => { scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 100); };
        }

        private void Button_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            timer.Tick += (s, e) => { scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 100); };
            timer.Start();
        }

        private void Button_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            timer.Tick -= (s, e) => { scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 100); };
        }

        private void Button_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            timer.Tick += (s, e) => { scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 100); };
            timer.Start();
        }

        private void Button_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            timer.Tick -= (s, e) => { scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 100); };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double zoomFactor = slider.Value;

            ScaleTransform scale = new ScaleTransform(zoomFactor, zoomFactor);
            DrawingCanvas.LayoutTransform = scale;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double zoomFactor = slider.Value;

            ScaleTransform scale = new ScaleTransform(zoomFactor, zoomFactor);
            drawingCanvas.LayoutTransform = scale;
        }

        private void zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (scaleTransform.ScaleX > 0.5)
            {
                scaleTransform.ScaleX -= 0.1;
                scaleTransform.ScaleY -= 0.1;
            }
        }

        private void zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (scaleTransform.ScaleX < 4)
            {
                scaleTransform.ScaleX += 0.1;
                scaleTransform.ScaleY += 0.1;
            }
        }
    }
}
