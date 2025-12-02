using System;

namespace PrimerosAuxilios.Entity
{
    /// <summary>
    /// Entidad que representa un usuario del sistema
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }

        public User(string username, string password, string email, string fullName)
        {
            Username = username;
            Password = password;
            Email = email;
            FullName = fullName;
            CreatedAt = DateTime.Now;
        }
    }
}