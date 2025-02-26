using System;

namespace ConstructionPMS.Application.Commands
{
    public class CreateUserCommand
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}