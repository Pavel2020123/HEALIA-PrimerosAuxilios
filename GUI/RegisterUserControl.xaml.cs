using System;
using System.Windows;
using System.Windows.Controls;
using PrimerosAuxilios.BLL;
using PrimerosAuxilios.Entity;

namespace GUI
{
    /// <summary>
    /// Control de usuario para el Registro
    /// SOLO maneja la interfaz - la lógica está en BLL
    /// </summary>
    public partial class RegisterUserControl : UserControl
    {
        private readonly UserService _userService;

        public event EventHandler RegisterSuccess;
        public event EventHandler ShowLoginRequested;
        public event EventHandler CloseRequested;

        public RegisterUserControl()
        {
            InitializeComponent();
            _userService = new UserService(); // Usa BLL
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Validaciones de UI (básicas)
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Por favor ingrese su nombre completo.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Por favor ingrese un nombre de usuario.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor ingrese su correo electrónico.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor ingrese una contraseña.",
                    "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ConfirmPasswordBox.Clear();
                return;
            }

            // La lógica de registro está en BLL
            try
            {
                var registrationResult = _userService.Register(username, password, email, fullName);

                switch (registrationResult)
                {
                    case RegistrationResult.Success:
                        MessageBox.Show("¡Cuenta creada exitosamente!\nAhora puedes iniciar sesión.",
                            "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                        RegisterSuccess?.Invoke(this, EventArgs.Empty);
                        break;

                    case RegistrationResult.UsernameExists:
                        MessageBox.Show("El usuario ya existe. Por favor elige otro nombre de usuario.",
                            "Error de registro", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case RegistrationResult.EmailExists:
                        MessageBox.Show("El correo electrónico ya está en uso. Por favor ingrese otro.",
                            "Error de registro", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                // Capturar validaciones de BLL
                MessageBox.Show(ex.Message, "Error de validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ClearFields()
        {
            FullNameTextBox.Clear();
            UsernameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
        }
    }
}