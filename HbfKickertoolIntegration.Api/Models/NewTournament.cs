using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HbfKickertoolIntegration.Api.Models
{
    public class NewTournament
    {
        public List<Team> Teams { get; set; }

        public int Groups { get; set; }

        public List<int> Tables { get; set; }
    }
}
