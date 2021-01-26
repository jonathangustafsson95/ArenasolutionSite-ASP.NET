using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLibrary
{
    public class League
    {
        public int LeagueOwnerId { get; set; }
        public LeagueOwner LeagueOwner { get; set; }
        public string LeagueId { get; set; }
        [Required]
        public string LeagueName { get; set; }
        public virtual ICollection<LeagueMember> LeagueMembers { get; set; }
    }
}
