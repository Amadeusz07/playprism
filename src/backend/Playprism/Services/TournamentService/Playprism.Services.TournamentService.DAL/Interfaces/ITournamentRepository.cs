using System.Collections.Generic;
using System.Threading.Tasks;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.DAL.Interfaces
{
    public interface ITournamentRepository: IRepository<TournamentEntity>
    {
    }
}