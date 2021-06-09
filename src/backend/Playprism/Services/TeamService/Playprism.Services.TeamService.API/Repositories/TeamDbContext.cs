using Microsoft.EntityFrameworkCore;
using Playprism.Services.TeamService.API.Entities;

namespace Playprism.Services.TeamService.API.Repositories
{
    public class TeamDbContext: DbContext 
    {
        public TeamDbContext(DbContextOptions<TeamDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamPlayerAssignmentEntity>()
                .HasKey(t => new { t.TeamId, t.PlayerId});
        }

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<TeamPlayerAssignmentEntity> TeamPlayerAssignments { get; set; }
    }
}
