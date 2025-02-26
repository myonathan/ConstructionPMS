using ConstructionPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Domain.Services
{
    public interface IUserDomainService
    {
        Task<User> CreateUserAsync(string username, string email, string role);
        Task<User> UpdateUserAsync(Guid userId, string username, string email, string role);
        Task DeleteUserAsync(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}