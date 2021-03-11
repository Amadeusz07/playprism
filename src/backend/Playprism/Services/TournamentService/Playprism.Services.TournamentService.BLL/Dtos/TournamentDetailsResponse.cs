using Playprism.Services.TournamentService.DAL.Enums;
using System;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class TournamentDetailsResponse: Dto
    {
        public int Id { get; set; }
        public string Platform { get; set; }
        public string Name { get; set; }
        public string DisciplineName { get; set; }
        public string OwnerName { get; set; }
        public bool AreTeams { get; set; }
        public int CurrentNumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public string Timezone { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public string Prizes { get; set; }
        public string Rules { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public TournamentFormatEnum Format { get; set; }
        public bool InviteOnly { get; set; }
        public bool RegistrationApprovalNeeded { get; set; }
        public bool Published { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Ongoing { get; set; }
        public bool Finished { get; set; }
        public bool Aborted { get; set; }
    }
}
