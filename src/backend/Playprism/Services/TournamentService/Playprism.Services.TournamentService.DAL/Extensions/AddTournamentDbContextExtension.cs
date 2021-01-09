using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playprism.Services.TournamentService.DAL.Interfaces;
using Playprism.Services.TournamentService.DAL.Repositories;

namespace Playprism.Services.TournamentService.DAL.Extensions
{
    public static class AddTournamentDbContextExtension
    {
        public static void AddTournamentDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TournamentDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("TournamentDbConnection"), 
                    options => options.MigrationsAssembly("Playprism.Services.TournamentService.DAL")
                    );
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ITournamentRepository), typeof(TournamentRepository));
            services.AddScoped(typeof(IParticipantRepository), typeof(ParticipantRepository));
            services.AddScoped(typeof(IRoundRepository), typeof(RoundRepository));
            services.AddScoped(typeof(IMatchRepository), typeof(MatchRepository));
        }
    }
}
