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
using System.Windows.Media.Animation;

namespace GUI
{
    /// <summary>
    /// Lógica de interacción para SubmenuOne.xaml
    /// </summary>
    public partial class SubmenuOne : UserControl
    {
        public SubmenuOne()
        {
            InitializeComponent();
        }

        // --- MÉTODOS RENOMBRADOS ---
        private void seguridad_btn_Selected(object sender, RoutedEventArgs e)
        {
            // Start the SubmenuTwo Slide-In Animation 
            Storyboard sb = FindResource("SubmenuTwo_Enter") as Storyboard;
            if (sb != null) sb.Begin();

            // Access the SubmenuTwo UserControl Menu Container and Enable it. 
            if (Application.Current.MainWindow is MainWindow MW)
            {
                if (MW.SubmenuTwo_UC != null)
                    MW.SubmenuTwo_UC.IsEnabled = true;
            }

            
        }

        private void seguridad_btn_Unselected(object sender, RoutedEventArgs e)
        {
            // Start the SubmenuTwo Slide-Out Animation 
            Storyboard sb = FindResource("SubmenuTwo_Exit") as Storyboard;
            if (sb != null) sb.Begin();

            
        }

        // (Lógica para el botón "Cerrar Sesión")
        private void cerrarsesion_btn_Selected(object sender, RoutedEventArgs e)
        {
            // Lógica para cerrar sesión
            Application.Current.Shutdown();
        }


        // --- ¡¡ESTE ES EL MÉTODO QUE ARREGLAMOS!! ---
        private void Close_Submenu_Click(object sender, RoutedEventArgs e)
        {
            // Start the SubmenuOne Slide-Out Animation 
            Storyboard sb = FindResource("SubmenuOne_Exit") as Storyboard;
            if (sb != null) sb.Begin();

            // Disable Menu Panel
            if (Menu_Panel != null)
                Menu_Panel.IsEnabled = false;

            // Unselect Seguridad Btn 
            if (seguridad_btn != null) // <-- ¡¡ARREGLADO!!
                seguridad_btn.IsSelected = false; // <-- ¡¡ARREGLADO!!

            // Unselect Cerrar Sesion Btn (¡NUEVO ARREGLO!)
            if (cerrarsesion_btn != null)
                cerrarsesion_btn.IsSelected = false;

            // Access the SidePanel UserControl "Customer" (Perfil) Btn and Unselect it. 
            if (Application.Current.MainWindow is MainWindow MW)
            {
                if (MW.MainMenu_UC?.SidePanel_UC?.customer_sidepanel_listboxitem != null)
                {
                    MW.MainMenu_UC.SidePanel_UC.customer_sidepanel_listboxitem.IsSelected = false;
                }
            }
        }
    }
}