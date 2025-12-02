using System;
using System.Windows;
using System.Windows.Controls;
using PrimerosAuxilios.BLL;

namespace GUI
{
    /// <summary>
    /// Control de usuario para el Login
    /// SOLO maneja la interfaz - la lógica está en BLL
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        private readonly UserService _userService;

        // Eventos para comunicarse con MainWindow
        public event EventHandler<LoginSuccessEventArgs> LoginSuccess;
        public event EventHandler ShowRegisterRequested;
        public event EventHandler ShowForgotPasswordRequested;
        public event EventHandler CloseRequested;

        public LoginUserControl()
        {
            InitializeComponent();
            _userService = new UserService(); // Usa BLL
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validaciones básicas de UI
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Por favor ingrese su usuario.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor ingrese su contraseña.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // La lógica de autenticación está en BLL
            bool loginSuccess = _userService.Login(username, password);

            if (loginSuccess)
            {
                MessageBox.Show("¡Bienvenido al sistema!",
                    "Login exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearFields();
                LoginSuccess?.Invoke(this, new LoginSuccessEventArgs(username));
            }
            else
            {
                MessageBox.Show(
                    $"Usuario o contraseña incorrectos.\n\n💡 Tip: Usuario de prueba:\nUsuario: admin\nContraseña: admin123",
                    "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBox.Clear();
            }
        }

        private void ForgotPasswordLink_Click(object sender, RoutedEventArgs e)
        {
            ShowForgotPasswordRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            ShowRegisterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ClearFields()
        {
            UsernameTextBox.Clear();
            PasswordBox.Clear();
        }
    }

    // Clase para pasar datos del usuario autenticado
    public class LoginSuccessEventArgs : EventArgs
    {
        public string Username { get; set; }

        public LoginSuccessEventArgs(string username)
        {
            Username = username;
        }
    }
}