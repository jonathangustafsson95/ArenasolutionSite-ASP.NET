using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    public class LeagueOwner : User
    {
        public ICollection<League> Leagues { get; set; }
    }
}
