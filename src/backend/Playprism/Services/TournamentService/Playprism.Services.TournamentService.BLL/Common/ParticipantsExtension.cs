using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Playprism.Services.TournamentService.BLL.Common
{
    public static class ParticipantsExtension
    {
        public static string GetName(this IEnumerable<ParticipantEntity> participants, int? participantId)
        {
            if (participantId.HasValue)
            {
                return participants.FirstOrDefault(x => x.Id == participantId)?.Name;
            }
            return null;
        }
    }
}
