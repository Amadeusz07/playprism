namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class JoinTournamentRequest: Dto
    {
        public int CandidateId { get; set; }
        public string Name { get; set; }
    }
}
