using System;
using System.Collections.Generic;
using System.Text;

namespace HbfKickertoolIntegration.Standalone.Datalayer
{
    public interface ITournamentRepository
    {
        Tournament LoadCurrentTournement();

        void SaveTournementAsCurrent(Tournament tournament);

        void DeleteCurrentTournament();
    }
}
