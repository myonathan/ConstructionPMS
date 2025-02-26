using ConstructionPMS.Application.Commands;
using ConstructionPMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserCommand command);
        Task<UserDto> UpdateUserAsync(UpdateUserCommand command);
        Task DeleteUserAsync(DeleteUserCommand command);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}