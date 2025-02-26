using ConstructionPMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPMS.Infrastructure
{
    public class ConstructionDbContext : DbContext
    {
        public ConstructionDbContext(DbContextOptions<ConstructionDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable(nameof(Project));
            modelBuilder.Entity<ProjectTask>().ToTable(nameof(ProjectTask));
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<Notification>().ToTable(nameof(Notification));
        }
    }
}
