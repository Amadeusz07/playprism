using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class ParticipantRepository: Repository<ParticipantEntity>, IParticipantRepository
    {
        public ParticipantRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }
    }
}