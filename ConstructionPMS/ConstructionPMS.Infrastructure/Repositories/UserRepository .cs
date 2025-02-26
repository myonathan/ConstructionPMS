using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConstructionDbContext _context;

        public UserRepository(ConstructionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> AddAsync(User user)
        {
            var addedEntity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // Save changes to the database
            return addedEntity.Entity; // Return the added entity
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}