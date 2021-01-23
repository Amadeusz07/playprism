using Microsoft.EntityFrameworkCore;
using Playprism.Services.TeamService.API.Entities;

namespace Playprism.Services.TeamService.API.Repositories
{
    public class TeamDbContext: DbContext 
    {
        public TeamDbContext(DbContextOptions<TeamDbContext> options): base(options)
        {
        }

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
    }
}
