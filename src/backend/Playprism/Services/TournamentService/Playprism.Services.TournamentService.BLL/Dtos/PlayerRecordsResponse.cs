using System;
using System.Collections.Generic;
using System.Text;

namespace Playprism.Services.TournamentService.BLL.Dtos
{
    public class PlayerRecordsResponse : Dto
    {
        public string Name { get; set; }
        public IEnumerable<WinsDefeatsResponse> Series { get; set; }
    }

    public class WinsDefeatsResponse: Dto
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
