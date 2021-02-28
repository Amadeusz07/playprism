using Playprism.Services.TeamService.API.Models;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interface.Repositories
{
    public interface IAuth0Repository
    {
        Task<UserInfo> SearchUserByNameAsync(string username);
    }
}
