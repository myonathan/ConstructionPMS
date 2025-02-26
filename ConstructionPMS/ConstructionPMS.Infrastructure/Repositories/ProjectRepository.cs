using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ConstructionDbContext _context;

        public ProjectRepository(ConstructionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        public async Task<Project> AddAsync(Project project)
        {
            var addedEntity = await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(); // Save changes to the database
            return addedEntity.Entity; // Return the added entity
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid projectId)
        {
            var project = await GetByIdAsync(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}