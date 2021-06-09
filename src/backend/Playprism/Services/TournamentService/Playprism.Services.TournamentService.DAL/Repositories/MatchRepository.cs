using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class MatchRepository: Repository<MatchEntity>, IMatchRepository
    {
        public MatchRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public async Task<IEnumerable<MatchEntity>> GetIncomingMatchesAsync(int participantId)
        {
            return await MainDbContext.Matches
                .Where(x => (x.Participant1Id == participantId || x.Participant2Id == participantId)
                    && x.Played == false)
                .Include(x => x.Round)
                .ThenInclude(x => x.MatchDefinition)
                .Include(x => x.Tournament)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchEntity>> GetCompletedMatchesByParticipant1Ids(List<int> participantIds)
        {
            return await MainDbContext.Matches
                .Where(x => participantIds.Contains(x.Participant1Id.Value)
                    && x.Confirmed
                    && x.Played)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchEntity>> GetCompletedMatchesByParticipant2Ids(List<int> participantIds)
        {
            return await MainDbContext.Matches
                .Where(x => participantIds.Contains(x.Participant2Id.Value)
                    && x.Confirmed
                    && x.Played)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchEntity>> AddRangeAsync(IEnumerable<MatchEntity> entities)
        {
            MainDbContext.Matches.AddRange(entities);
            await MainDbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<MatchEntity> GetFullByIdAsync(int id)
        {
            return await MainDbContext.Matches
                .Include(x => x.Round)
                .ThenInclude(x => x.MatchDefinition)
                .FirstAsync(x => x.Id == id);
        }

        public async Task ClearAsync(int tournamentId)
        {
            var toRemove= MainDbContext.Matches.Where(x => x.TournamentId == tournamentId);
            MainDbContext.RemoveRange(toRemove);
            await MainDbContext.SaveChangesAsync();
        }
    }
}