using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class TournamentRepository: Repository<TournamentEntity>, ITournamentRepository
    {
        public TournamentRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }
    }
}