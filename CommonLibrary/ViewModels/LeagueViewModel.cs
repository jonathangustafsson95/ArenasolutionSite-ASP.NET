using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.ViewModels
{
    public class LeagueViewModel : BaseViewModel
    {
        public League League { get; set; }
        public List<LeagueMember> LeagueMembers { get; set; }
    }
}
