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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana principal
            var mainWindow = new MainWindow();

            // Mostrar la ventana principal
            mainWindow.Show();

            // Cerrar la ventana de inicio
            this.Close();
        }
    }
}
