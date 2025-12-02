using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GUI
{
    public partial class SidePanel : UserControl
    {
        public SidePanel()
        {
            InitializeComponent();
        }

        // ==============================================
        // CHAT EN VIVO
        // ==============================================
        private void dashboard_sidepanel_listboxitem_Selected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                MW.ShowChat();
            }
        }

        // ==============================================
        // HISTORIAL
        // ==============================================
        private void mail_sidepanel_listboxitem_Selected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                MW.ShowHistorial();
            }
        }

        private void mail_sidepanel_listboxitem_Unselected(object sender, RoutedEventArgs e)
        {
            // Vacío - la lógica de ocultar se maneja en ShowChat() o ShowHistorial()
        }

        // ==============================================
        // MI CUENTA (Abre SubmenuOne)
        // ==============================================
        private void customer_sidepanel_listboxitem_Selected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                // Ocultar Chat si está visible
                if (MW.Chat_UC.Visibility == Visibility.Visible)
                {
                    MW.HideChat();
                }

                // Ocultar Historial si está visible
                if (MW.Historial_UC.Visibility == Visibility.Visible)
                {
                    MW.HideHistorial();
                }

                // Ocultar Mapa si está visible
                if (MW.MapView_UC != null && MW.MapView_UC.Visibility == Visibility.Visible)
                {
                    MW.HideMapView();
                }

                // Cerrar SubmenuTwo si está abierto
                MW.CloseSubmenuTwo();

                // Mostrar SubmenuOne
                Storyboard sb = MW.FindResource("SubmenuOne_Enter") as Storyboard;
                if (sb != null) sb.Begin();

                if (MW.SubmenuOne_UC != null)
                    MW.SubmenuOne_UC.IsEnabled = true;
            }
        }

        private void customer_sidepanel_listboxitem_Unselected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                // Forzar la animación de salida cuando se deselecciona
                Storyboard sb = MW.FindResource("SubmenuOne_Exit") as Storyboard;
                if (sb != null) sb.Begin();

                // Deshabilitar el panel
                if (MW.SubmenuOne_UC != null)
                    MW.SubmenuOne_UC.IsEnabled = false;
            }
        }

        // ==============================================
        // ⭐ MAPA (NUEVO)
        // ==============================================
        private void map_sidepanel_listboxitem_Selected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                // Ocultar Chat si está visible
                if (MW.Chat_UC.Visibility == Visibility.Visible)
                {
                    MW.HideChat();
                }

                // Ocultar Historial si está visible
                if (MW.Historial_UC.Visibility == Visibility.Visible)
                {
                    MW.HideHistorial();
                }

                // Cerrar SubmenuOne y SubmenuTwo si están abiertos
                MW.CloseSubmenuTwo();

                Storyboard sbExit = MW.FindResource("SubmenuOne_Exit") as Storyboard;
                if (sbExit != null) sbExit.Begin();
                if (MW.SubmenuOne_UC != null)
                    MW.SubmenuOne_UC.IsEnabled = false;

                // ✅ Mostrar el Mapa
                MW.ShowMapView();
            }
        }

        private void map_sidepanel_listboxitem_Unselected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow MW)
            {
                // Ocultar el mapa cuando se deselecciona
                if (MW.MapView_UC != null && MW.MapView_UC.Visibility == Visibility.Visible)
                {
                    MW.HideMapView();
                }
            }
        }
    }
}