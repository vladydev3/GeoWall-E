using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace GeoWall_E
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            // Establecer el estilo de la ventana a None para ocultar la barra de título
            this.WindowStyle = WindowStyle.None;

            // Establecer el estado de la ventana a Maximized para hacer que se abra en pantalla completa
            this.WindowState = WindowState.Maximized;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana principal
            var mainWindow = new MainWindow();

            // Mostrar la ventana principal
            mainWindow.Show();

            // Cerrar la ventana de inicio
            this.Close();
        }

        private void Commands(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("| Comando | Descripción |\r\n|---------|-------------|\r\n| point <id> | Declara que se recibe un argumento de tipo punto con nombre <id> |\r\n| line <id> | Declara que se recibe un argumento de tipo recta con nombre <id> |\r\n| segment <id> | Declara que se recibe un argumento de tipo segmento con nombre <id> |\r\n| ray <id> | Declara que se recibe un argumento de tipo semirecta con nombre <id> |\r\n| circle <id> | Declara que se recibe un argumento de tipo circunferencia con nombre <id> |\r\n| point sequence <id> | Declara que se recibe un argumento de tipo secuencia de puntos con nombre <id> |\r\n| line sequence <id> | … |\r\n| color | Establece el color a ser utilizado |\r\n| restore | Restablece el color anterior |\r\n| import <string> | Incluye en el programa actual las definiciones del fichero indicado. |\r\n| draw <exp> <string> | Dibuja el o los objetos definidos en <exp> |\r\n| line(p1,p2) | Devuelve una recta que pasa por los puntos p1 y p2. |\r\n| segment(p1,p2) | Devuelve un segmento con extremos en los puntos p1 y p2. |\r\n| ray(p1,p2) | Devuelve una semirecta que comienza en p1 y pasa por p2. |\r\n| arc(p1,p2,p3,m) | Devuelve un arco que tiene centro en p1, se extiende desde una semirecta que pasa por p2 hasta una semirecta que pasa por p3 y tiene medida m. |\r\n| circle(p,m) | Devuelve una circunferencia con centro en p y medida m. |\r\n| measure(p1,p2) | Devuelve una medida entre los puntos p1 y p2. |\r\n| intersect(f1,f2) | Intersecta dos figuras (puntos, rectas, etc.) y devuelve la secuencia de puntos de intersección. Si la intersección coincide en infinitos puntos devuelve undefined. |\r\n| count(s) | Devuelve la cantidad de elementos de una secuencia. Si la secuencia es infinita devuelve undefined. |\r\n| randoms() | Devuelve una secuencia de valores aleatorios numéricos entre 0 y 1. |\r\n| points(f) | Devuelve una secuencia de puntos aleatorios en una figura. |\r\n| samples() | Devuelve una secuencia de puntos aleatorios en el lienzo. |\r\n");

        }

        private void Import(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string contenido = System.IO.File.ReadAllText(openFileDialog.FileName);

                // Crear una nueva instancia de la ventana principal
                MainWindow mainWindow = new MainWindow();

                // Establecer el texto del TextBox Entrada en la ventana principal
                mainWindow.Entrada.Text = contenido;
                

                // Mostrar la ventana principal
                mainWindow.Show();

                // Cerrar la ventana actual
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = @"D:\Escuela\Proyecto 3\GeoWall-E\cositas.docx",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
