using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using PrimerosAuxilios.BLL;

namespace GUI
{
    /// <summary>
    /// Control de usuario para Recuperación de Contraseña
    /// SOLO maneja la interfaz - la lógica está en BLL
    /// </summary>
    public partial class ForgotPasswordUserControl : UserControl
    {
        private readonly UserService _userService;
        private bool isUserValid = false;

        public event EventHandler BackToLoginRequested;
        public event EventHandler CloseRequested;

        public ForgotPasswordUserControl()
        {
            InitializeComponent();
            _userService = new UserService(); // Usa BLL

            StatusMessage.Visibility = Visibility.Visible;
            StatusText.Text = "Ingresa tu usuario para continuar";
            StatusIcon.Text = "ℹ️";
        }

        private async void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                DisablePasswordSection();
                StatusMessage.Visibility = Visibility.Visible;
                StatusText.Text = "Ingresa tu usuario para continuar";
                StatusIcon.Text = "ℹ️";
                UpdateStatusColor("#FFF0F5", "#FFE0F0", "#C33764");
                return;
            }

            StatusMessage.Visibility = Visibility.Visible;
            StatusText.Text = "Verificando usuario...";
            StatusIcon.Text = "⏳";
            UpdateStatusColor("#FFF9E6", "#FFE8B3", "#D97706");

            try
            {
                // La lógica de validación está en BLL
                bool userExists = await _userService.ValidateUsernameAsync(username);

                if (userExists)
                {
                    EnablePasswordSection();
                    StatusMessage.Visibility = Visibility.Visible;
                    StatusText.Text = "✓ Usuario verificado. Ahora ingresa tu nueva contraseña";
                    StatusIcon.Text = "✅";
                    UpdateStatusColor("#F0FFF4", "#C6F6D5", "#22863A");
                    isUserValid = true;
                    ChangePasswordButton.IsEnabled = true;
                }
                else
                {
                    DisablePasswordSection();
                    StatusMessage.Visibility = Visibility.Visible;
                    StatusText.Text = "✗ Usuario no encontrado. Verifica e intenta nuevamente";
                    StatusIcon.Text = "❌";
                    UpdateStatusColor("#FFF5F5", "#FED7D7", "#C53030");
                    isUserValid = false;
                    ChangePasswordButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                StatusMessage.Visibility = Visibility.Visible;
                StatusText.Text = "Error al verificar usuario. Intenta nuevamente";
                StatusIcon.Text = "⚠️";
                UpdateStatusColor("#FFF5F5", "#FED7D7", "#C53030");
                DisablePasswordSection();
            }
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isUserValid)
            {
                MessageBox.Show("Por favor, ingresa un usuario válido primero.",
                    "Usuario no válido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string username = UsernameTextBox.Text.Trim();
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Validaciones de UI
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Por favor, ingresa una nueva contraseña.",
                    "Campo vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                NewPasswordBox.Focus();
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.",
                    "Contraseña débil", MessageBoxButton.OK, MessageBoxImage.Warning);
                NewPasswordBox.Focus();
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, verifícalas.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ConfirmPasswordBox.Clear();
                ConfirmPasswordBox.Focus();
                return;
            }

            ChangePasswordButton.IsEnabled = false;
            ChangePasswordButton.Content = "CAMBIANDO...";

            try
            {
                // La lógica de actualización está en BLL
                bool success = await _userService.UpdatePasswordAsync(username, newPassword);

                if (success)
                {
                    MessageBox.Show(
                        "¡Contraseña cambiada exitosamente!\n\nAhora puedes iniciar sesión con tu nueva contraseña.",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearFields();
                    BackToLoginRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show(
                        "Error al cambiar la contraseña.\n\nPor favor intenta nuevamente o contacta al soporte.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado:\n\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ChangePasswordButton.IsEnabled = true;
                ChangePasswordButton.Content = "CAMBIAR CONTRASEÑA";
            }
        }

        private void BackLink_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            BackToLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            BackToLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        // Métodos auxiliares de UI
        private void EnablePasswordSection()
        {
            var fadeIn = new DoubleAnimation(1.0, TimeSpan.FromMilliseconds(300));
            PasswordSection.IsEnabled = true;
            PasswordSection.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void DisablePasswordSection()
        {
            var fadeOut = new DoubleAnimation(0.4, TimeSpan.FromMilliseconds(300));
            PasswordSection.IsEnabled = false;
            PasswordSection.BeginAnimation(OpacityProperty, fadeOut);
            ChangePasswordButton.IsEnabled = false;
            NewPasswordBox.Clear();
            ConfirmPasswordBox.Clear();
        }

        private void UpdateStatusColor(string bgColor, string borderColor, string textColor)
        {
            StatusMessage.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom(bgColor);
            StatusMessage.BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom(borderColor);
            StatusText.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom(textColor);
        }

        public void ClearFields()
        {
            UsernameTextBox.Clear();
            NewPasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            DisablePasswordSection();
            StatusMessage.Visibility = Visibility.Visible;
            StatusText.Text = "Ingresa tu usuario para continuar";
            StatusIcon.Text = "ℹ️";
            UpdateStatusColor("#FFF0F5", "#FFE0F0", "#C33764");
            isUserValid = false;
        }
    }
}