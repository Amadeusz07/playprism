namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class UpdateMatchDefinitionRequest: Dto
    {
        public bool ConfirmationNeeded { get; set; }
        public int NumberOfGames { get; set; }
        public bool ScoreBased { get; set; }
    }
}
