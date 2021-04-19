using Playprism.Services.TournamentService.DAL.Entities;
using System;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class MatchDto : Dto
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public MatchDefinitionEntity MatchDefinition { get; set; }
        public DateTime? MatchDate { get; set; }
        public string Participant1Name { get; set; }
        public string Participant2Name { get; set; }
        public int? Participant1Id { get; set; }
        public int? Participant2Id { get; set; }
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
        public int? Result { get; set; }

    }
}
