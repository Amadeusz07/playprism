using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Models
{
    public class AuthConfig
    {
        public string Token { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
