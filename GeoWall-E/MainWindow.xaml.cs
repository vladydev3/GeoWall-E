using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeoWall_E
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Obtén las líneas del TextBox
            string[] lines = Entrada.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            Dictionary<string, Point> pointCenters = new Dictionary<string, Point>();


            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("point "))
                {
                    // Obtén el nombre del punto
                    string pointName = line.Substring(6); // Omite "point "

                    // Crear un círculo rojo
                    Ellipse point = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Red,
                        ToolTip = pointName // Asigna el nombre del punto a ToolTip
                    };

                    // Crear una etiqueta con el nombre del punto
                    Label label = new Label
                    {
                        Content = pointName,
                        Foreground = Brushes.Black
                    };

                    // Añadir el punto y la etiqueta al Canvas
                    drawingCanvas.Children.Add(point);
                    drawingCanvas.Children.Add(label);

                    // Posicionar el punto y la etiqueta
                    double pointCenterX = 100 + i * 20; // Cambia estos valores a la posición deseada
                    double pointCenterY = 100;
                    pointCenters[pointName] = new Point(pointCenterX, pointCenterY);
                    Canvas.SetLeft(point, pointCenterX - point.Width / 2);
                    Canvas.SetTop(point, pointCenterY - point.Height / 2);
                    double labelCenterX = pointCenterX; // La misma X que el punto
                    double labelCenterY = pointCenterY - 20; // Un poco por encima del punto
                    Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
                    Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
                }
                else if (line.StartsWith("circle "))
                {
                    // Obtén el nombre del centro
                    string centerName = line.Substring(7); // Omite "circle "
                    if (pointCenters.TryGetValue(centerName, out Point center))
                    {
                        // Busca el punto que será el centro de la circunferencia
                        Ellipse centerPoint = drawingCanvas.Children.OfType<Ellipse>().FirstOrDefault(e => e.ToolTip.ToString() == centerName);

                        if (centerPoint != null)
                        {
                            // Crear una circunferencia
                            Ellipse circle = new Ellipse
                            {
                                Width = 100, // Cambia esto al tamaño deseado
                                Height = 100, // Cambia esto al tamaño deseado
                                Stroke = Brushes.Red,
                                StrokeThickness = 2
                            };

                            // Añadir la circunferencia al Canvas
                            drawingCanvas.Children.Add(circle);

                            // Posicionar la circunferencia en el mismo lugar que el punto central
                            Canvas.SetLeft(circle, center.X - circle.Width / 2);
                            Canvas.SetTop(circle, center.Y - circle.Height / 2);
                        }

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
    }
}
