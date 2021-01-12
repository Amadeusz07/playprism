﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Playprism.Services.TournamentService.BLL.Common;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.BLL.Services
{
    public class MatchSettingsService: IMatchSettingsService
    {
        private readonly IRepository<MatchDefinitionEntity> _matchDefinitionRepository;
        private readonly IMapper _mapper;

        public MatchSettingsService(IRepository<MatchDefinitionEntity> matchDefinitionRepository, IMapper mapper)
        {
            _matchDefinitionRepository = matchDefinitionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatchDefinitionEntity>> GetMatchSettingsByTournamentAsync(int tournamentId)
        {
            var settingsList = await _matchDefinitionRepository.GetAsync(x => x.TournamentId == tournamentId);
            return settingsList;
        }

        public async Task<MatchDefinitionEntity> AddMatchSettingsAsync(MatchDefinitionEntity entity)
        {
            if (await NameExistsAsync(entity.TournamentId, entity.Name))
            {
                throw new ValidationException("Name already exists"); 
            }
            return await _matchDefinitionRepository.AddAsync(entity);
        }

        public async Task<MatchDefinitionEntity> CreateDefaultSettingsAsync(int tournamentId)
        {
            var result = await _matchDefinitionRepository.AddAsync(new MatchDefinitionEntity()
            {
                TournamentId = tournamentId,
                Name = Consts.DefaultSettingsName,
                ConfirmationNeeded = false,
                NumberOfGames = 1,
                ScoreBased = false
            });
            return result;
        }

        public async Task<MatchDefinitionEntity> UpdateMatchSettingsAsync(MatchDefinitionEntity entity)
        {
            var matchDefinition = await _matchDefinitionRepository.GetByIdAsync(entity.Id);
            if (await NameExistsAsync(entity.TournamentId, entity.Name))
            {
                throw new ValidationException("Name already exists"); 
            }

            matchDefinition = _mapper.Map(entity, matchDefinition);
            await _matchDefinitionRepository.UpdateAsync(matchDefinition);
            return matchDefinition;
        }

        public async Task DeleteMatchSettingsAsync(int id)
        {
            var entity = await _matchDefinitionRepository.GetByIdAsync(id);
            await _matchDefinitionRepository.DeleteAsync(entity);
        }

        private async Task<bool> NameExistsAsync(int tournamentId, string name)
        {
            return (await _matchDefinitionRepository
                .GetAsync(x => x.Name == name && x.TournamentId == tournamentId))
                .Any();
        }
    }
}