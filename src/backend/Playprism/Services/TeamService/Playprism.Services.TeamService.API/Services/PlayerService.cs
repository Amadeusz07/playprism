using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Services;
using Playprism.Services.TeamService.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<PlayerEntity> GetPlayerByUserIdAsync(string userId)
        {
            var player = (await _playerRepository.GetAsync(x => x.UserId == userId)).FirstOrDefault();
            return player;
        }

        public async Task<PlayerEntity> GetPlayerByUserInfoAsync(UserInfo userInfo)
        {
            PlayerEntity player;
            var players = await _playerRepository.GetAsync(x => x.Name == userInfo.Username);
            if (players.Any())
            {
                player = players.SingleOrDefault();
            }
            else
            {
                player = await HandleUnknownUserAsync(userInfo);
            }

            return player;
        }

        public async Task<PlayerEntity> HandleUnknownUserAsync(UserInfo userInfo)
        {
            var newEntity = new PlayerEntity()
            {
                Name = userInfo.Username,
                UserId = userInfo.UserId
            };
            await _playerRepository.AddAsync(newEntity);

            return newEntity;
        }
    }
}
