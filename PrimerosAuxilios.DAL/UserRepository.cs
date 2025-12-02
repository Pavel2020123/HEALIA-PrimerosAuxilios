using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using PrimerosAuxilios.Entity;

namespace PrimerosAuxilios.DAL
{
    /// <summary>
    /// Repositorio para acceso a datos de usuarios
    /// Maneja la persistencia en archivo JSON
    /// </summary>
    public class UserRepository
    {
        private readonly string _usersFilePath;

        public UserRepository()
        {
            _usersFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PrimerosAuxilios",
                "users.json"
            );

            string directory = Path.GetDirectoryName(_usersFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios del archivo JSON
        /// </summary>
        public List<User> GetAll()
        {
            if (!File.Exists(_usersFilePath))
            {
                return new List<User>();
            }

            try
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Busca un usuario por username
        /// </summary>
        public User GetByUsername(string username)
        {
            var users = GetAll();
            return users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Busca un usuario por email
        /// </summary>
        public User GetByEmail(string email)
        {
            var users = GetAll();
            return users.FirstOrDefault(u =>
                u.Email.Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si existe un usuario con el username dado
        /// </summary>
        public bool ExistsByUsername(string username)
        {
            return GetByUsername(username) != null;
        }

        /// <summary>
        /// Verifica si existe un usuario con el email dado
        /// </summary>
        public bool ExistsByEmail(string email)
        {
            return GetByEmail(email) != null;
        }

        /// <summary>
        /// Guarda un nuevo usuario
        /// </summary>
        public void Add(User user)
        {
            var users = GetAll();
            users.Add(user);
            SaveAll(users);
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public void Update(User user)
        {
            var users = GetAll();
            var existing = users.FirstOrDefault(u =>
                u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                users.Remove(existing);
                users.Add(user);
                SaveAll(users);
            }
        }

        /// <summary>
        /// Guarda todos los usuarios en el archivo JSON
        /// </summary>
        private void SaveAll(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(users, options);
                File.WriteAllText(_usersFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar usuarios: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un usuario por defecto (admin)
        /// </summary>
        public void CreateDefaultUser()
        {
            var users = GetAll();
            if (users.Count == 0)
            {
                Add(new User("admin", "admin123", "admin@primeros.com", "Administrador"));
            }
        }
    }
}