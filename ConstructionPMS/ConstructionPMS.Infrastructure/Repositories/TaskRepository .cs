using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ConstructionDbContext _context;

        public TaskRepository(ConstructionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTasks.ToListAsync();
        }

        public async Task<ProjectTask> GetByIdAsync(Guid taskId)
        {
            return await _context.ProjectTasks.FindAsync(taskId);
        }

        public async Task<ProjectTask> AddAsync(ProjectTask task)
        {
            var addedEntity = await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync(); // Save changes to the database
            return addedEntity.Entity; // Return the added entity
        }

        public async Task UpdateAsync(ProjectTask task)
        {
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid taskId)
        {
            var task = await GetByIdAsync(taskId);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}