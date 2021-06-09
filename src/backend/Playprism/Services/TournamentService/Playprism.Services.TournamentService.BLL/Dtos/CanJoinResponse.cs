using System;
using System.Collections.Generic;
using System.Text;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class CanJoinResponse: Dto
    {
        public bool Accepted { get; set; }
        public string Message { get; set; }
    }
}
