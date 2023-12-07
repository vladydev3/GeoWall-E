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
using System.Security.Cryptography.X509Certificates;
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
        public bool saver;
        public bool saver1;
        public bool saver3;
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
            saver3 = true;
            Consola.Clear();
            // Asi se procesaria el codigo del usuario
            string code = Entrada.Text;
            // esto es lo q te hace todo el proceso y te devuelve la lista de Type que es lo q tienes q dibujar
            handler = new Handler(code);
            if (handler.CheckErrors())
            {
                saver1 = true;
                // Obtiene todos los errores 
                List<string> errors = handler.Errors.GetAllErrors.ToList();

               

                foreach (string error in errors)
                {
                    Consola.Text += error + Environment.NewLine;
                }
            }
            else
            {
                saver1 = false;
                // Habilita el botón Run si no hay errores
                Run.IsEnabled = true;
            }

        }
        private void RunClick(object sender, RoutedEventArgs e)
        {
            
            // Borra el Canvas
            drawingCanvas.Children.Clear();
            Consola.Text = "";
                foreach (var item in handler.ToDraw)
                {

                    if (item.Item1 is Point point)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, point,item.Item2);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(point.X - 400);
                        scrollViewer.ScrollToVerticalOffset(point.Y - 250);

                    }
                    else if ( item.Item1 is Line line)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, line, item.Item2);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(line.P1.X - 400);
                        scrollViewer.ScrollToVerticalOffset(line.P1.Y - 250);
                    }

                    else if ( item.Item1 is Segment segment)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, segment, item.Item2);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(segment.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(segment.Start.Y - 250);
                    }

                    else if ( item.Item1 is Ray ray)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, ray, item.Item2);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(ray.Start.X - 400);
                        scrollViewer.ScrollToVerticalOffset(ray.Start.Y - 250);
                    }

                    else if ( item.Item1 is Circle circle)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, circle, item.Item2);
                        drawer.Draw();
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                        // Restablece el valor del Slider al valor predeterminado
                        zoomSlider.Value = 1;
                        scrollViewer.ScrollToHorizontalOffset(circle.Center.X - 400);
                        scrollViewer.ScrollToVerticalOffset(circle.Center.Y - 250);
                    }

                    else if ( item.Item1 is Arc arc)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, arc, item.Item2);
                        drawer.Draw();
                        scrollViewer.ScrollToHorizontalOffset(arc.Center.X - 400);
                        scrollViewer.ScrollToVerticalOffset(arc.Center.Y - 250);

                    }
                    else if (item.Item1 is Sequence sequence)
                    {
                    foreach (var element in sequence.Elements)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, element, item.Item2);
                        drawer.Draw();
                        if (element is Point pointt)
                        {
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 1;
                            // Restablece el valor del Slider al valor predeterminado
                            zoomSlider.Value = 1;
                            scrollViewer.ScrollToHorizontalOffset(pointt.X - 400);
                            scrollViewer.ScrollToVerticalOffset(pointt.Y - 250);

                        }
                        else if (element is Line linet)
                        {
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 1;
                            // Restablece el valor del Slider al valor predeterminado
                            zoomSlider.Value = 1;
                            scrollViewer.ScrollToHorizontalOffset(linet.P1.X - 400);
                            scrollViewer.ScrollToVerticalOffset(linet.P1.Y - 250);
                        }
                        else if (item.Item1 is Segment segmentt)
                        {
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 1;
                            // Restablece el valor del Slider al valor predeterminado
                            zoomSlider.Value = 1;
                            scrollViewer.ScrollToHorizontalOffset(segmentt.Start.X - 400);
                            scrollViewer.ScrollToVerticalOffset(segmentt.Start.Y - 250);

                        }
                        else if (item.Item1 is Ray rayy)
                        {
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 1;
                            // Restablece el valor del Slider al valor predeterminado
                            zoomSlider.Value = 1;
                            scrollViewer.ScrollToHorizontalOffset(rayy.Start.X - 400);
                            scrollViewer.ScrollToVerticalOffset(rayy.Start.Y - 250);

                        }
                        else if (item.Item1 is Circle circlee)
                        {
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 1;
                            // Restablece el valor del Slider al valor predeterminado
                            zoomSlider.Value = 1;
                            scrollViewer.ScrollToHorizontalOffset(circlee.Center.X - 400);
                            scrollViewer.ScrollToVerticalOffset(circlee.Center.Y - 250);

                        }
                        else if (item.Item1 is Arc arcc)
                        {
                            scrollViewer.ScrollToHorizontalOffset(arcc.Center.X - 400);
                            scrollViewer.ScrollToVerticalOffset(arcc.Center.Y - 250);
                        }

                    }
                }
                        
            }
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            if (saver == false)
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres reiniciar sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)           
                {
                    saver3 = false;
                    saver1 = false;
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
            }
            else
            {
                saver3 = false;
                saver1 = false;
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


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saver = false;
            Run.IsEnabled = false;
            // Obtener el número y el contenido de cada línea
            string[] lines = Entrada.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Crear una variable para almacenar los números de línea
            string lineNumbers = "";

            // Recorrer cada línea y agregar el número correspondiente
            for (int i = 0; i < lines.Length; i++)
            {
                lineNumbers += "<" + (i + 1) + ">" + "\n"; 
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
            if (Entrada.Text =="")
            {
                MessageBox.Show("Error!! La entrada esta vacía", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (saver1==true)
            {
                System.Windows.Forms.MessageBox.Show("Error!! corrija los errores antes de guardar", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if (saver3==false )
            {
                MessageBox.Show("Error!!Compile antes de guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                saver = true;
                string text = Entrada.Text;
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "Archivos GS(*.gs)|*.gs";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    System.IO.File.WriteAllText(filePath, text);
                    MessageBox.Show("Texto guardado correctamente");
                }
            }

        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            if (saver ==false)
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres volver sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // El usuario ha hecho clic en 'Sí', puedes cerrar la aplicación aquí
                    this.Close();
                }


            }
            else 
            {
                this.Close();
            }
            
        }

        private void ReturnMenu(object sender, RoutedEventArgs e)
        {
            if (saver == false)
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres salir sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var Menu = new Window1();
                    Menu.Show();
                    this.Close();
                }

            }
            else
            {
                var Menu = new Window1();
                Menu.Show();
                this.Close();
            }
            
        }

        private void Entrada_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Obtener el RichTextBox
            var richTextBox = sender as RichTextBox;

            // Crear una variable para almacenar los números de línea
            string lineNumbers = "";

            // Obtener el número de líneas visibles en el RichTextBox
            int visibleLineCount = (int)(richTextBox.ExtentHeight / richTextBox.FontSize);

            // Recorrer cada línea y agregar el número correspondiente
            for (int i = 0; i < visibleLineCount; i++)
            {
                lineNumbers += (i + 1) + "\n";
            }

            // Asignar los números de línea al TextBox de la numeración
            Enumerador.Text = lineNumbers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset((scrollViewer.ScrollableHeight / 2) -350);
            scrollViewer.ScrollToHorizontalOffset((scrollViewer.ScrollableWidth / 2)-250);
        }
    }
}

    

