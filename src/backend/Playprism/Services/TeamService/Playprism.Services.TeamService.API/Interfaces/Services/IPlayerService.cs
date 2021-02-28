using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Models;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<PlayerEntity> GetPlayerByUserInfoAsync(UserInfo userInfo);
    }
}
