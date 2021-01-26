using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    public class Player : User
    {
        public int Rating { get; set; }
        public string ConnectionId { get; set; }
        public string CurrentGameType { get; set; }
        public virtual ICollection<LeagueMember> LeagueMemberShips { get; set; }
        public virtual ICollection<TournamentPlayer> TournamentMemberships { get; set; }

    }
}
