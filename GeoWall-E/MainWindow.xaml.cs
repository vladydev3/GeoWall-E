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
            this.WindowStyle = WindowStyle.None; // Añade esta línea
            this.ResizeMode = ResizeMode.NoResize; // Añade esta línea
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
            var evaluator = new Evaluator(ast.Root);
            var toDraw = evaluator.Evaluate();  // aqui se devolveria una List<Types> con el tipo de dato que hay que imprimir (point solo por ahora) revisa en la carpeta Types la clase Point
            // despues de esto se puede hacer un foreach y dibujar cada uno de los elementos de la lista toDraw


            foreach (var item in toDraw)
            {

                if (item is Point point)
                {
                    string name = point.Name;
                    Color color = point.Color;
                    Random random = new Random();
                    Ellipse point1 = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Blue,
                        ToolTip = name// Asigna el nombre del punto a ToolTip
                    };
                    // Crear una etiqueta con el nombre del punto
                    Label label = new Label
                    {
                        Content = name,
                        Foreground = Brushes.Black
                    };

                    // Asegurarse de que el punto y la etiqueta se dibujan dentro del Canvas
                    int drawingCanvasWidth = (int)drawingCanvas.Width - (int)point1.Width;
                    int drawingCanvasHeight = (int)drawingCanvas.Height - (int)point1.Height;

                    // Posicionar el punto y la etiqueta
                    double pointCenterX = random.Next((int)point1.Width / 2, drawingCanvasWidth);
                    double pointCenterY = random.Next((int)point1.Height / 2, drawingCanvasHeight);
                    point.X = pointCenterX;
                    point.Y = pointCenterY;
                    point.Draw(name, color, drawingCanvas, point1, label, pointCenterX, pointCenterY);
                }
                if (item is Line line)
                {
                    line.Draw(drawingCanvas);

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
