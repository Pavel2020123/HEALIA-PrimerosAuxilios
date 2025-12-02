using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GUI
{
    public partial class MainWindow : Window
    {
        private string currentUsername = ""; // ⭐ NUEVO - Guardar el usuario actual

        public MainWindow()
        {
            InitializeComponent();
            SubscribeToLoginEvents();
        }

        private void SubscribeToLoginEvents()
        {
            // ==================== EVENTOS DEL LOGIN ====================
            Login_UC.LoginSuccess += OnLoginSuccess;
            Login_UC.ShowRegisterRequested += (s, e) => ShowRegister();
            Login_UC.ShowForgotPasswordRequested += (s, e) => ShowForgotPassword();
            Login_UC.CloseRequested += (s, e) => Application.Current.Shutdown();

            // ==================== EVENTOS DEL REGISTER ====================
            Register_UC.RegisterSuccess += OnRegisterSuccess;
            Register_UC.ShowLoginRequested += (s, e) => TransitionToLogin(Register_UC);
            Register_UC.CloseRequested += (s, e) => TransitionToLogin(Register_UC);

            // ==================== EVENTOS DEL FORGOT PASSWORD ====================
            ForgotPassword_UC.BackToLoginRequested += (s, e) => TransitionToLogin(ForgotPassword_UC);
            ForgotPassword_UC.CloseRequested += (s, e) => TransitionToLogin(ForgotPassword_UC);
        }

        // ✅ MÉTODO UNIFICADO para transiciones al Login
        private void TransitionToLogin(System.Windows.Controls.UserControl currentControl)
        {
            if (currentControl == Register_UC)
            {
                HideRegister();
            }
            else if (currentControl == ForgotPassword_UC)
            {
                HideForgotPassword();
            }

            // Esperar a que termine la animación de ocultar
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(350);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                ShowLogin();
            };
            timer.Start();
        }

        private void OnLoginSuccess(object sender, LoginSuccessEventArgs e)
        {
            // ⭐ GUARDAR EL USERNAME DEL USUARIO AUTENTICADO
            currentUsername = e.Username;

            HideLogin();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(350);
            timer.Tick += (s, args) =>
            {
                timer.Stop();

                // ⭐ PASAR EL USERNAME AL CHAT
                Chat_UC.SetCurrentUser(currentUsername);

                ShowChat();

                // ✅ ABRIR EL MENÚ después de que se oculte el login
                OpenAndLockMenu();
            };
            timer.Start();
        }

        /// <summary>
        /// Abre el menú y lo deja bloqueado (siempre abierto)
        /// </summary>
        private void OpenAndLockMenu()
        {
            if (MainMenu_UC?.Menu_ToggleButton != null)
            {
                MainMenu_UC.Menu_ToggleButton.IsChecked = true;

                // ✅ DESACTIVAR el botón para que no se pueda cerrar
                MainMenu_UC.Menu_ToggleButton.IsEnabled = false;
            }
        }

        private void OnRegisterSuccess(object sender, EventArgs e)
        {
            TransitionToLogin(Register_UC);
        }

        // ==================== MÉTODOS PARA SUBMENUS ====================

        public void ShowSubmenuOne()
        {
            Storyboard sb = (Storyboard)FindResource("SubmenuOne_Enter");
            sb.Begin();
        }

        public void HideSubmenuOne()
        {
            Storyboard sb = (Storyboard)FindResource("SubmenuOne_Exit");
            sb.Begin();
        }

        /// <summary>
        /// ⭐ NUEVO - Cierra SubmenuOne si está abierto
        /// </summary>
        public void CloseSubmenuOne()
        {
            if (SubmenuOne_UC?.Visibility == Visibility.Visible)
            {
                var hideSubmenuOne = (Storyboard)this.Resources["SubmenuOne_Exit"];
                hideSubmenuOne?.Begin();

                if (SubmenuOne_UC != null)
                {
                    SubmenuOne_UC.IsEnabled = false;
                }
            }
        }

        public void CloseSubmenuTwo()
        {
            if (SubmenuTwo_UC?.Visibility == Visibility.Visible)
            {
                var hideSubmenuTwo = (Storyboard)this.Resources["SubmenuTwo_Exit"];
                hideSubmenuTwo?.Begin();
            }
        }

        // ==================== MÉTODOS PARA CHAT ====================

        public void ShowChat()
        {
            // Ocultar otros elementos
            HideHistorial();
            HideMapView();
            CloseSubmenuOne();
            CloseSubmenuTwo();

            if (Historial_UC.Visibility == Visibility.Visible)
            {
                var hideHistorial = (Storyboard)this.Resources["HideHistorialAnimation"];
                hideHistorial.Completed += (s, e) =>
                {
                    var showChat = (Storyboard)this.Resources["ShowChatAnimation"];
                    showChat.Begin();
                };
                hideHistorial.Begin();
            }
            else
            {
                var showChat = (Storyboard)this.Resources["ShowChatAnimation"];
                showChat.Begin();
            }
        }

        public void HideChat()
        {
            if (Chat_UC.Visibility == Visibility.Visible)
            {
                var hideChat = (Storyboard)this.Resources["HideChatAnimation"];
                hideChat.Begin();
            }
        }

        // ==================== MÉTODOS PARA HISTORIAL ====================

        public void ShowHistorial()
        {
            // Ocultar otros elementos
            HideChat();
            HideMapView();
            CloseSubmenuOne();
            CloseSubmenuTwo();

            if (Chat_UC.Visibility == Visibility.Visible)
            {
                var hideChat = (Storyboard)this.Resources["HideChatAnimation"];
                hideChat.Completed += (s, e) =>
                {
                    // ⭐ PASAR EL USERNAME AL HISTORIAL
                    Historial_UC.SetCurrentUser(currentUsername);
                    Historial_UC.LoadConversations();

                    var showHistorial = (Storyboard)this.Resources["ShowHistorialAnimation"];
                    showHistorial.Begin();
                };
                hideChat.Begin();
            }
            else
            {
                // ⭐ PASAR EL USERNAME AL HISTORIAL
                Historial_UC.SetCurrentUser(currentUsername);
                Historial_UC.LoadConversations();

                var showHistorial = (Storyboard)this.Resources["ShowHistorialAnimation"];
                showHistorial.Begin();
            }
        }

        public void HideHistorial()
        {
            if (Historial_UC.Visibility == Visibility.Visible)
            {
                var hideHistorial = (Storyboard)this.Resources["HideHistorialAnimation"];
                hideHistorial.Begin();
            }
        }

        // ==================== 🗺️ MÉTODOS PARA MAPA (CORREGIDOS) ====================

        public void ShowMapView()
        {
            // Ocultar otros elementos
            HideChat();
            HideHistorial();
            CloseSubmenuOne();
            CloseSubmenuTwo();

            // ✅ USAR LA ANIMACIÓN que ya tienes definida
            if (MapView_UC != null)
            {
                var showMap = (Storyboard)this.Resources["ShowMapAnimation"];
                if (showMap != null)
                {
                    showMap.Begin();
                }
                else
                {
                    // Fallback si no existe la animación
                    MapView_UC.Visibility = Visibility.Visible;
                    MapView_UC.Opacity = 1;
                }
            }
        }

        public void HideMapView()
        {
            if (MapView_UC != null && MapView_UC.Visibility == Visibility.Visible)
            {
                // ✅ USAR LA ANIMACIÓN que ya tienes definida
                var hideMap = (Storyboard)this.Resources["HideMapAnimation"];
                if (hideMap != null)
                {
                    hideMap.Begin();
                }
                else
                {
                    // Fallback si no existe la animación
                    MapView_UC.Visibility = Visibility.Collapsed;
                }
            }
        }
        // ==================== MÉTODOS PARA LOGIN ====================

        public void ShowLogin()
        {
            // Ocultar todo lo demás si está visible
            if (Chat_UC.Visibility == Visibility.Visible) HideChat();
            if (Historial_UC.Visibility == Visibility.Visible) HideHistorial();
            if (MapView_UC?.Visibility == Visibility.Visible) HideMapView();
            if (Register_UC.Visibility == Visibility.Visible) HideRegister();
            if (ForgotPassword_UC.Visibility == Visibility.Visible) HideForgotPassword();

            CloseSubmenuOne();
            CloseSubmenuTwo();
            Login_UC.ClearFields();

            var showLogin = (Storyboard)this.Resources["ShowLoginAnimation"];
            showLogin.Begin();
        }

        public void HideLogin()
        {
            if (Login_UC.Visibility == Visibility.Visible)
            {
                var hideLogin = (Storyboard)this.Resources["HideLoginAnimation"];
                hideLogin.Begin();
            }
        }

        // ==================== MÉTODOS PARA REGISTER ====================

        public void ShowRegister()
        {
            if (Login_UC.Visibility == Visibility.Visible) HideLogin();
            if (ForgotPassword_UC.Visibility == Visibility.Visible) HideForgotPassword();

            Register_UC.ClearFields();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(350);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var showRegister = (Storyboard)this.Resources["ShowRegisterAnimation"];
                showRegister.Begin();
            };
            timer.Start();
        }

        public void HideRegister()
        {
            if (Register_UC.Visibility == Visibility.Visible)
            {
                var hideRegister = (Storyboard)this.Resources["HideRegisterAnimation"];
                hideRegister.Begin();
            }
        }

        // ==================== MÉTODOS PARA FORGOT PASSWORD ====================

        public void ShowForgotPassword()
        {
            if (Login_UC.Visibility == Visibility.Visible) HideLogin();
            if (Register_UC.Visibility == Visibility.Visible) HideRegister();

            ForgotPassword_UC.ClearFields();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(350);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var showForgot = (Storyboard)this.Resources["ShowForgotPasswordAnimation"];
                showForgot.Begin();
            };
            timer.Start();
        }

        public void HideForgotPassword()
        {
            if (ForgotPassword_UC.Visibility == Visibility.Visible)
            {
                var hideForgot = (Storyboard)this.Resources["HideForgotPasswordAnimation"];
                hideForgot.Begin();
            }
        }

        // ==================== EVENTOS DE VENTANA ====================

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ChangeWindowState()
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                Window_Edge.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    Window_Edge.Visibility = Visibility.Visible;
                }
            }
        }

        private void Manin_windonw_Panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                ChangeWindowState();
            }

            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
            {
                CloseSubmenuOne();
                CloseSubmenuTwo();

                // El menú principal permanece abierto y bloqueado después del login
            }
        }

        // ==================== EVENTOS DE ANIMACIÓN ====================

        public void EnableMainWindow(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            ShowLogin();
        }

        private void MainMenu_UC_Loaded(object sender, RoutedEventArgs e)
        {
            // Inicialización si es necesaria
        }
    }
}