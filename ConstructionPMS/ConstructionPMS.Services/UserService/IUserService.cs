using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
    }
}