using System;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class UpdateTournamentRequest: Dto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string Description { get; set; }
        public string Prizes { get; set; }
        public string Rules { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public bool RegistrationApprovalNeeded { get; set; }
        public bool Published { get; set; }
    }
}
