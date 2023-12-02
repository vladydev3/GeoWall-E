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
using System.Collections;
using System.Drawing.Drawing2D;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GeoWall_E
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static Canvas DrawingCanvas { get; set; }
        private System.Windows.Point zoomCenter;
        private double previousZoomFactor = 1.0;
        public Handler handler { get; set; }
        public bool runner;
        public MainWindow()
        {
            InitializeComponent();
            // Establecer el estilo de la ventana a None para ocultar la barra de título
            this.WindowStyle = WindowStyle.None;

            // Establecer el estado de la ventana a Maximized para hacer que se abra en pantalla completa
            this.WindowState = WindowState.Maximized;
            // Asignar el Canvas del XAML a la propiedad estática
            MainWindow.DrawingCanvas = this.drawingCanvas;

            zoomCenter = new System.Windows.Point(drawingCanvas.Width / 2, drawingCanvas.Height / 2);
        }

        private void Compile(object sender, RoutedEventArgs e)

        {
            Consola.Clear();
            // Asi se procesaria el codigo del usuario
            string code = Entrada.Text;
            // esto es lo q te hace todo el proceso y te devuelve la lista de Type que es lo q tienes q dibujar
            handler = new Handler(code);
            if (handler.CheckErrors())
            {
                // Obtiene todos los errores 
                List<string> errors = handler.Errors.GetAllErrors.ToList();

                // Invierte la lista de errores
                errors.Reverse();


                foreach (string error in errors)
                {
                    Consola.Text += error + Environment.NewLine;
                }
            }
            else
            {
                // Habilita el botón Run si no hay errores
                Run.IsEnabled = true;
            }

        }
        private void RunClick(object sender, RoutedEventArgs e)
        {
            // Borra el Canvas
            drawingCanvas.Children.Clear();
            Consola.Text = "";
            if (runner == true)
            {
                foreach (var item in handler.ToDraw)
                {

                    if (item is Point point)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, point);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(point.X - 400);
                        scrollViewer.ScrollToVerticalOffset(point.Y - 250);

                    }
                    if (item is Line line)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, line);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(line.P1.X - 400);
                        scrollViewer.ScrollToVerticalOffset(line.P1.Y - 250);
                    }

                    if (item is Segment segment)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, segment);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(segment.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(segment.Start.Y - 250);
                    }

                    if (item is Ray ray)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, ray);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(ray.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(ray.Start.Y - 250);
                    }

                    if (item is Circle circle)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, circle);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(circle.Center.X - 400);
                        scrollViewer.ScrollToVerticalOffset(circle.Center.Y - 250);
                    }

                    if (item is Arc arc)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, arc);
                        drawer.Draw();
                        scrollViewer.ScrollToHorizontalOffset(arc.Center.X - 400);
                        scrollViewer.ScrollToVerticalOffset(arc.Center.Y - 250);

                    }


                }
            }
            else
            {
                MessageBox.Show("ERROR IN COMPILATION", "Compilation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            // Borra el TextBox
            Entrada.Text = "";
            Consola.Text = "";
            Enumerador.Text = "";
            // Borra el Canvas
            drawingCanvas.Children.Clear();
            scaleTransform.ScaleX = 1;
            scaleTransform.ScaleY = 1;
            // Restablece el valor del Slider al valor predeterminado
            zoomSlider.Value = 1;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Run.IsEnabled = false;
            // Obtener el número y el contenido de cada línea
            string[] lines = Entrada.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Crear una variable para almacenar los números de línea
            string lineNumbers = "";

            // Recorrer cada línea y agregar el número correspondiente
            for (int i = 0; i < lines.Length; i++)
            {
                lineNumbers += (i + 1) + "\n";
            }

            // Asignar los números de línea al TextBox de la numeración
            Enumerador.Text = lineNumbers;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // 'scrollViewer' se refiere al control ScrollViewer que has añadido en tu XAML
            scrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 100);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 100);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 100);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 100);
        }


        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Captura la posición del cursor cuando se hace clic en el canvas
            zoomCenter = e.GetPosition(drawingCanvas);
            MessageBox.Show($"Has hecho clic en las coordenadas: X = {zoomCenter.X}, Y = {zoomCenter.Y}");

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplyZoom((sender as Slider).Value);

        }
        private void ApplyZoom(double zoomFactor)
        {
            if (drawingCanvas != null)
            {
                // Convierte las coordenadas del punto de zoom al espacio de la pantalla
                System.Windows.Point zoomCenterScreen = drawingCanvas.PointToScreen(zoomCenter);

                // Calcula el desplazamiento necesario para mantener el punto de zoom en su lugar
                double deltaX = (zoomCenterScreen.X * (1 - zoomFactor / previousZoomFactor)) / zoomFactor;
                double deltaY = (zoomCenterScreen.Y * (1 - zoomFactor / previousZoomFactor)) / zoomFactor;

                // Aplica la transformación de escala y traslación
                scaleTransform.ScaleX = scaleTransform.ScaleY = zoomFactor;
                translateTransform.X = deltaX;
                translateTransform.Y = deltaY;

                // Actualiza el factor de zoom anterior
                previousZoomFactor = zoomFactor;
            }
        }

        private void zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (zoomSlider.Value > 0.5)
            {
                zoomSlider.Value -= 0.01; // Cambia 0.1 a 0.05
                ApplyZoom(zoomSlider.Value);
            }
        }
        private void zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (zoomSlider.Value < 4)
            {
                zoomSlider.Value += 0.01; // Cambia 0.1 a 0.05
                ApplyZoom(zoomSlider.Value);
            }
        }
        private void Import(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string contenido = System.IO.File.ReadAllText(openFileDialog.FileName);
                string entrada = Entrada.Text = contenido;
                RunClick(sender, e);

            }
        }
        private void Export(object sender, RoutedEventArgs e)
        {
            string text = Entrada.Text;
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Archivos de texto(*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                System.IO.File.WriteAllText(filePath, text);
                MessageBox.Show("Texto guardado correctamente");
            }
            else

            {
                MessageBox.Show("Ocurrio un error");
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

    

