using System;

namespace ConstructionPMS.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; } // Store the hashed password

        public User() { }

        public User(string username, string email, string role, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            Role = role;
            PasswordHash = HashPassword(password); // Hash the password before storing
        }

        // Method to hash the password
        private string HashPassword(string password)
        {
            // Use a secure hashing algorithm (e.g., BCrypt, PBKDF2, etc.)
            // For demonstration, we'll use a simple hash (not recommended for production)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Additional methods for business logic can be added here
    }
}