using System;

namespace ConstructionPMS.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }

        public User(string username, string email, string role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            Role = role;
        }

        // Additional methods for business logic can be added here
    }
}