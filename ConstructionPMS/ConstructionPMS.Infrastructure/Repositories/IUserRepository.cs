using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid userId);
        Task<User> AddAsync(User user); // Ensure this returns User
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
    }
}