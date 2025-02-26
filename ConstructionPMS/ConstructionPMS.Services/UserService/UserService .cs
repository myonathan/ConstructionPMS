using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Domain.Interfaces;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Shared.Exceptions;
using Nest;

namespace ConstructionPMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new CustomException($"User  with ID {userId} not found.");
            }
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new CustomException("User  cannot be null.");
            }
            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new CustomException("User  cannot be null.");
            }
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
        }
    }
}