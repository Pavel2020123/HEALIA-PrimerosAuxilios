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
using System.Diagnostics;
namespace GUI
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        // Reset / Restart the application.
        // Reset / Restart the application.
        private void Reset_Control_Click(object sender, RoutedEventArgs e)
        {
            // 1. Creamos la nueva ventana
            MainWindow nuevaVentana = new MainWindow();

            // 2. Le decimos que se muestre
            nuevaVentana.Show();

            // 3. Buscamos y cerramos la ventana "vieja" (la actual)
            Window ventanaActual = Window.GetWindow(this);
            ventanaActual.Close();
        }
    }
}
