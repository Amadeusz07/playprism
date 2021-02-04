using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class TournamentRepository: Repository<TournamentEntity>, ITournamentRepository
    {
        public TournamentRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }
    }
}