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

namespace GeoWall_E
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas DrawingCanvas { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            // Asignar el Canvas del XAML a la propiedad estática
            MainWindow.DrawingCanvas = this.drawingCanvas;
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                    }
                    if (item is Line line)
                    {
                        line.Draw(drawingCanvas);

                    }
                    if (item is Segment segment)
                    {
                        segment.Draw(drawingCanvas);
                    }
                    if (item is Ray ray)
                    {
                        ray.Draw(drawingCanvas);
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
    }
}
