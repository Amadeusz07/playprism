using Playprism.Services.TournamentService.DAL.Enums;
using System;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class TournamentListItemResponse: Dto
    {
        public int Id { get; set; }
        public string DisciplineName { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public DateTime StartDate { get; set; }
        public int CurrentNumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public bool Ongoing { get; set; }
        public bool Finished { get; set; }
        public bool Aborted { get; set; }
        public bool Published { get; set; }
        public TournamentFormatEnum Format { get; set; }
    }
}
