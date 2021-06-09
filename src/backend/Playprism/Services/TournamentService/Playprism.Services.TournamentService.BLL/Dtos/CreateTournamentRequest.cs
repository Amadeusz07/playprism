using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class CreateTournamentRequest: Dto
    {
        public string Name { get; set; }
        [Required]
        public int DisciplineId { get; set; }
        [Required]
        public bool AreTeams { get; set; }
        public string Platform { get; set; }
        [Required]
        public int MaxNumberOfPlayers { get; set; }
        [Required]
        public bool RegistrationApprovalNeeded { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; } 
    }
}
