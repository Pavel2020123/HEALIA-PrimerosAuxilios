using System;
using System.Linq;
using System.Threading.Tasks;
using PrimerosAuxilios.Entity;
using PrimerosAuxilios.DAL;

namespace PrimerosAuxilios.BLL
{
    /// <summary>
    /// Servicio de lógica de negocio para usuarios
    /// Contiene validaciones y reglas de negocio
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
            _userRepository.CreateDefaultUser();
        }

        /// <summary>
        /// Valida las credenciales de login
        /// </summary>
        public bool Login(string username, string password)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = _userRepository.GetByUsername(username);

            if (user != null && user.Password == password)
            {
                // Actualizar fecha de último login
                user.LastLogin = DateTime.Now;
                _userRepository.Update(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        public RegistrationResult Register(string username, string password, string email, string fullName)
        {
            // Validación: username no puede estar vacío
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío");
            }

            // Validación: username debe tener al menos 3 caracteres
            if (username.Length < 3)
            {
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
            }

            // Validación: email no puede estar vacío
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("El email no puede estar vacío");
            }

            // Validación: email debe tener formato válido
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new ArgumentException("El email no tiene un formato válido");
            }

            // Validación: password no puede estar vacío
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía");
            }

            // Validación: password debe tener al menos 6 caracteres
            if (password.Length < 6)
            {
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");
            }

            // Normalizar el email
            email = email.Trim().ToLowerInvariant();

            // Verificar si el username ya existe
            if (_userRepository.ExistsByUsername(username))
            {
                return RegistrationResult.UsernameExists;
            }

            // Verificar si el email ya existe
            if (_userRepository.ExistsByEmail(email))
            {
                return RegistrationResult.EmailExists;
            }

            // Crear y guardar el nuevo usuario
            var newUser = new User(username, password, email, fullName);
            _userRepository.Add(newUser);

            return RegistrationResult.Success;
        }

        /// <summary>
        /// Valida si un usuario existe
        /// </summary>
        public Task<bool> ValidateUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Task.FromResult(false);
            }

            username = username.Trim();
            bool exists = _userRepository.ExistsByUsername(username);

            return Task.FromResult(exists);
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        public Task<bool> UpdatePasswordAsync(string username, string newPassword)
        {
            try
            {
                // Validación: password debe tener al menos 6 caracteres
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return Task.FromResult(false);
                }

                username = username.Trim();
                var user = _userRepository.GetByUsername(username);

                if (user == null)
                {
                    return Task.FromResult(false);
                }

                // Actualizar la contraseña
                user.Password = newPassword;
                _userRepository.Update(user);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios (para administración)
        /// </summary>
        public System.Collections.Generic.List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }
    }
}