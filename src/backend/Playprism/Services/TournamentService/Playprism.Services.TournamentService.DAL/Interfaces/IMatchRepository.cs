﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.DAL.Interfaces
{
    public interface IMatchRepository: IRepository<MatchEntity>
    {
        Task<IEnumerable<MatchEntity>> AddRangeAsync(IEnumerable<MatchEntity> entities);
        Task<MatchEntity> GetFullByIdAsync(int id);
        Task ClearAsync(int tournamentId);
    }
}