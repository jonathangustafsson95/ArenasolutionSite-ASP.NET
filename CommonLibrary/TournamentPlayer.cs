using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// This class contains the functionality for TournamentPlayers
    /// </summary>
    public class TournamentPlayer
    {
        public int TournamentPlayerId { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public int UserId { get; set; }
        public virtual Player Player { get; set; }
    }
}
