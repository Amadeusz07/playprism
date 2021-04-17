using System;
using System.Collections.Generic;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class BracketResponse : Dto
    {
        public List<RoundResponse> Rounds { get; set; } = new List<RoundResponse>();
    }
    
    public class RoundResponse: Dto
    {
        public int Id { get; set; }
        public List<MatchResponse> Matches { get; set; } = new List<MatchResponse>();
        public DateTime? RoundDate { get; set; }
    }

    public class MatchResponse: Dto
    {
        public int Id { get; set; }
        public string Participant1 { get; set; }
        public string Participant2 { get; set; }
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
        public int? Result { get; set; }
        public DateTime? MatchDate { get; set; }
        public int? PreviousMatch1Id { get; set; }
        public int? PreviousMatch2Id { get; set; }
    }
}
