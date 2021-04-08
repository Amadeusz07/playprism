using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class ParticipantRepository : Repository<ParticipantEntity>, IParticipantRepository
    {
        public ParticipantRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public async Task<IEnumerable<ParticipantEntity>> GetParticipantsByUserIdAsync(string userId)
        {
            return await MainDbContext.Participants
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ParticipantEntity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await MainDbContext.Participants
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}