using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ConstructionDbContext _context; // Assuming you have an ApplicationDbContext for EF Core

        public ProjectRepository(ConstructionDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int projectId)
        {
            return await _context.Projects
                .AsNoTracking() // This will ensure that the entity is not tracked by the context
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int projectId)
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