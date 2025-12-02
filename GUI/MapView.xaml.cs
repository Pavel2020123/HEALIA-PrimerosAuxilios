using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

namespace GUI
{
    public partial class MapView : UserControl
    {
        private bool _isWebViewInitialized = false;

        public MapView()
        {
            InitializeComponent();
            InitializeWebView();
        }

        /// <summary>
        /// Inicializa el WebView2
        /// </summary>
        private async void InitializeWebView()
        {
            try
            {
                await MapWebView.EnsureCoreWebView2Async(null);
                _isWebViewInitialized = true;

                // ✅ Cargar Google Maps de Colombia por defecto
                string mapsUrl = "https://www.google.com/maps/@10.4631,-73.2532,14z";
                MapWebView.CoreWebView2.Navigate(mapsUrl);

                // Ocultar loading
                MapLoadingPanel.Visibility = Visibility.Collapsed;
                MapWebView.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                // ✅ Silenciar el error y solo mostrar estado visual
                MapLoadingPanel.Visibility = Visibility.Collapsed;
                MapEmptyState.Visibility = Visibility.Visible;

                // No mostrar MessageBox para evitar molestar al usuario
                _isWebViewInitialized = false;
            }
        }

        /// <summary>
        /// ✅ Método público para cargar cualquier URL
        /// Úsalo así desde otra parte: mapView.LoadUrl("https://www.google.com/maps/...");
        /// </summary>
        public void LoadUrl(string url)
        {
            if (_isWebViewInitialized)
            {
                MapWebView.CoreWebView2.Navigate(url);
            }
        }

        /// <summary>
        /// ✅ Método público para cargar una ubicación específica
        /// Úsalo así: mapView.LoadLocation(10.4631, -73.2532);
        /// </summary>
        public void LoadLocation(double lat, double lon)
        {
            string mapsUrl = $"https://www.google.com/maps/@{lat},{lon},14z";
            LoadUrl(mapsUrl);
        }
    }
}