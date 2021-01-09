using Microsoft.Extensions.DependencyInjection;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.BLL.Services;
using Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer;

namespace Playprism.Services.TournamentService.BLL.Extensions
{
    public static class AddTournamentServiceBLL
    {
        public static void AddTournamentService(this IServiceCollection services)
        {
            services.AddTransient<ICrudService, CrudService>();
            services.AddTransient<IMatchSettingsService, MatchSettingsService>();
            services.AddTransient<IMatchService, MatchService>();
                
            services.AddTransient<ICompetitionProcess, SingleEliminationProcess>();
        }
    }
}
