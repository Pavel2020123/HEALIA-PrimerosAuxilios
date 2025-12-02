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
using System.Windows.Shapes; // <--- ¡¡AQUÍ ESTÁ LA LÍNEA MÁGICA QUE FALTABA!!
using System.Windows.Media.Animation;

namespace GUI
{
    public partial class SubmenuTwo : UserControl
    {
        private bool isPasswordVisible = false;

        public SubmenuTwo()
        {
            InitializeComponent();
        }

        // Alterna entre mostrar y ocultar la contraseña
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Buscar los elementos del ojo en el template
            var template = btn.Template;
            var eyeClosed = template.FindName("EyeClosed", btn) as Path; // <-- Ahora sí sabe qué es "Path"
            var eyeOpen = template.FindName("EyeOpen", btn) as Path;   // <-- Ahora sí sabe qué es "Path"

            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Mostrar contraseña en texto plano
                TextBox_Password.Text = PasswordBox_Password.Password;
                TextBox_Password.Visibility = Visibility.Visible;
                PasswordBox_Password.Visibility = Visibility.Collapsed;

                if (eyeClosed != null) eyeClosed.Visibility = Visibility.Collapsed;
                if (eyeOpen != null) eyeOpen.Visibility = Visibility.Visible;
            }
            else
            {
                // Ocultar contraseña
                PasswordBox_Password.Password = TextBox_Password.Text;
                TextBox_Password.Visibility = Visibility.Collapsed;
                PasswordBox_Password.Visibility = Visibility.Visible;

                if (eyeClosed != null) eyeClosed.Visibility = Visibility.Visible;
                if (eyeOpen != null) eyeOpen.Visibility = Visibility.Collapsed;
            }
        }

        // Actualizar contraseña
        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = isPasswordVisible ? TextBox_Password.Text : PasswordBox_Password.Password;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Por favor ingrese una nueva contraseña.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí va tu lógica para actualizar la contraseña
            try
            {
                // TODO: Implementar lógica de actualización
                // Ejemplo:
                // UserService.UpdatePassword(currentUserId, newPassword);

                MessageBox.Show("Contraseña actualizada correctamente.", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar campos
                PasswordBox_Password.Clear();
                TextBox_Password.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la contraseña: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Actualizar usuario
        private void UpdateUsername_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = TextBox_Username.Text;

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Por favor ingrese un nuevo nombre de usuario.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newUsername.Length < 3)
            {
                MessageBox.Show("El nombre de usuario debe tener al menos 3 caracteres.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí va tu lógica para actualizar el usuario
            try
            {
                // TODO: Implementar lógica de actualización
                // Ejemplo:
                // UserService.UpdateUsername(currentUserId, newUsername);

                MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar campo
                TextBox_Username.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el usuario: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cerrar panel
        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar campos al cerrar
            PasswordBox_Password.Clear();
            TextBox_Password.Clear();
            TextBox_Username.Clear();

            // Ocultar el panel o navegar a otra vista
            // Opción 1: Ocultar
            this.Visibility = Visibility.Collapsed;

            // Opción 2: Notificar al contenedor padre
            // Window.GetWindow(this)?.Close();
        }



    }
}