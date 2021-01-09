using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.DAL
{
    internal class TournamentDbContext : DbContext
    {
        public TournamentDbContext(DbContextOptions<TournamentDbContext> options) : base(options)
        {
        }

        public DbSet<DisciplineEntity> Disciplines { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<MatchDefinitionEntity> MatchDefinitions { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<ParticipantEntity> Participants { get; set; }
        public DbSet<RoundEntity> Rounds { get; set; }
        public DbSet<TournamentEntity> Tournaments { get; set; }
    }
}
