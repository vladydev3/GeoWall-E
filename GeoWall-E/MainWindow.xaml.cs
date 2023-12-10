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
using System.Drawing;
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
        public bool saveChecker;
        public bool saveChecker1;
        public bool saveChecker3;
        public MainWindow()
        {
            InitializeComponent(); // Este método se genera automáticamente y se utiliza para inicializar los componentes de la ventana.

            this.WindowStyle = WindowStyle.None; // Establece el estilo de la ventana a None para ocultar la barra de título.

            this.WindowState = WindowState.Maximized; // Establece el estado de la ventana a Maximized para hacer que se abra en pantalla completa.

            MainWindow.DrawingCanvas = this.drawingCanvas; // Asigna el Canvas del XAML a la propiedad estática DrawingCanvas.

            zoomCenter = new System.Windows.Point(drawingCanvas.Width / 2, drawingCanvas.Height / 2); // Inicializa el centro del zoom en el centro del Canvas.
        }

        private void Compile(object sender, RoutedEventArgs e)// Este método se ejecuta cuando se hace clic en el botón para compilar el código.

        {
            saveChecker3 = true; 

            Consola.Clear(); // Limpia la consola.

            string code = Entrada.Text; // Obtiene el código ingresado por el usuario.

            handler = new Handler(code); // Crea una nueva instancia de la clase Handler con el código del usuario.

            if (handler.CheckErrors()) 
            {
                saveChecker1 = true; 

                List<string> errors = handler.Errors.GetAllErrors.ToList(); // Obtiene todos los errores.

                foreach (string error in errors)
                {
                    Consola.Text += error + Environment.NewLine; // Agrega el error a la consola.
                }
            }
            else 
            {
                saveChecker1 = false;

                Run.IsEnabled = true; // Habilita el botón Run.
            }

        }
        private void RunClick(object sender, RoutedEventArgs e)// Este método se ejecuta cuando se hace clic en un botón "Run".
        {
            handler.HandleEvaluate();// Evalúa el código ingresado por el usuario.
            // Borra el Canvas
            drawingCanvas.Children.Clear();// Borra todos los elementos del Canvas.

            if (handler.CheckErrors())
            {
                List<string> errors = handler.Errors.GetAllErrors.ToList();// Obtiene todos los errores.

                foreach (string error in errors)
                {
                    Consola.Text += error + Environment.NewLine;// Agrega el error a la consola.
                }
            }
            else
            {
                foreach (var item in handler.ToDraw)
                {

                    if (item.Item1 is IAdjustable adjustable)
                    {
                        Picasso drawer = new Picasso(drawingCanvas, (Type)adjustable, item.Item2);
                        drawer.Draw();
                        Adjust(adjustable.SignificativePoint);
                    }
                    else if (item.Item1 is Sequence sequence)
                    {
                        foreach (var element in sequence.Elements)
                        {
                            if (element is IAdjustable adjustableElement)
                            {
                                Picasso drawer = new Picasso(drawingCanvas, (Type)adjustableElement, item.Item2);
                                drawer.Draw();
                                Adjust(adjustableElement.SignificativePoint);
                            }
                        }
                    }
                }
            }
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            if (saveChecker == false)
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres reiniciar sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);// Muestra un cuadro de mensaje preguntando al usuario si quiere reiniciar sin guardar.
                if (result == MessageBoxResult.Yes)
                {
                    saveChecker3 = false;
                    saveChecker1 = false;
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
                saveChecker3 = false;
                saveChecker1 = false;
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)//Se activa cuando el usuario modifica algo en la entrada
        {
            saveChecker = false;
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
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 100);// Desplaza el ScrollViewer verticalmente 100 unidades hacia abajo.
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 100); // Desplaza el ScrollViewer verticalmente 100 unidades hacia arriba.
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 100);// Desplaza el ScrollViewer horizontalmente 100 unidades hacia la izquierda.
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 100);// Desplaza el ScrollViewer horizontalmente 100 unidades hacia la derecha.
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
        private void Export(object sender, RoutedEventArgs e)
        {
            if (Entrada.Text == "") 
            {
                MessageBox.Show("Error!! La entrada esta vacía", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Muestra un cuadro de mensaje de error.
            }
            else if (saveChecker1 == true) 
            {
                System.Windows.Forms.MessageBox.Show("Error!! corrija los errores antes de guardar", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); // Muestra un cuadro de mensaje de error.
            }
            else if (saveChecker3 == false) 
            {
                MessageBox.Show("Error!!Compile antes de guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Muestra un cuadro de mensaje de error.
            }
            else 
            {
                saveChecker = true; 
                string text = Entrada.Text; // Obtiene el texto del TextBox de entrada.
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog(); // Crea un nuevo cuadro de diálogo para guardar archivos.
                saveFileDialog.Filter = "Archivos GS(*.gs)|*.gs"; // Establece el filtro del cuadro de diálogo para mostrar solo los archivos .gs.

                string currentDirectory = System.IO.Directory.GetCurrentDirectory(); // Obtiene el directorio actual.
                string projectDirectory = System.IO.Directory.GetParent(currentDirectory).Parent.Parent.FullName; // Obtiene el directorio del proyecto.
                string subfolder = "SavedFiles"; // Establece el nombre de la subcarpeta.
                string folderPath = System.IO.Path.Combine(projectDirectory, subfolder); // Combina el directorio del proyecto y la subcarpeta para obtener la ruta de la carpeta.
                saveFileDialog.InitialDirectory = folderPath; // Establece el directorio inicial del cuadro de diálogo para guardar.

                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    string filePath = saveFileDialog.FileName; // Obtiene la ruta del archivo seleccionado.
                    System.IO.File.WriteAllText(filePath, text); // Escribe el texto en el archivo seleccionado.
                    MessageBox.Show("Texto guardado correctamente"); // Muestra un cuadro de mensaje indicando que el texto se guardó correctamente.
                }
            }

        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            if (saveChecker == false) 
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres volver sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question); // Muestra un cuadro de mensaje preguntando al usuario si quiere salir sin guardar.
                if (result == MessageBoxResult.Yes) 
                {
                    this.Close(); // Cierra la ventana.
                }
            }
            else 
            {
                this.Close(); // Cierra la ventana.
            }

        }

        private void ReturnMenu(object sender, RoutedEventArgs e)
        {
            if (saveChecker == false) 
            {
                MessageBoxResult result = MessageBox.Show("¿Seguro que quieres salir sin guardar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question); // Muestra un cuadro de mensaje preguntando al usuario si quiere salir sin guardar.
                if (result == MessageBoxResult.Yes) 
                {
                    var Menu = new Window1(); // Crea una nueva instancia de la clase Window1.
                    Menu.Show(); // Muestra la ventana.
                    this.Close(); // Cierra la ventana actual.
                }
            }
            else 
            {
                var Menu = new Window1(); // Crea una nueva instancia de la clase Window1.
                Menu.Show(); // Muestra la ventana.
                this.Close(); // Cierra la ventana actual.
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
            scrollViewer.ScrollToVerticalOffset((scrollViewer.ScrollableHeight / 2) - 350);
            scrollViewer.ScrollToHorizontalOffset((scrollViewer.ScrollableWidth / 2) - 250);
        }
        private void Adjust(Point point)
        {
            scaleTransform.ScaleX = 1;// Restablece la escala X del transform.
            scaleTransform.ScaleY = 1; // Restablece la escala Y del transform.
            zoomSlider.Value = 1; // Restablece el valor del Slider al valor predeterminado.
            scrollViewer.ScrollToHorizontalOffset(point.X - 400);// Desplaza el ScrollViewer horizontalmente al punto X menos 400.
            scrollViewer.ScrollToVerticalOffset(point.Y - 250); // Desplaza el ScrollViewer verticalmente al punto Y menos 250.
        }
        private void TextBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {                      
                Enumerador.ScrollToVerticalOffset(e.VerticalOffset);                                   
        }
    }
}



