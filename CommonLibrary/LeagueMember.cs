using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLibrary
{
    public class LeagueMember
    {
        public int UserId { get; set; }
        public virtual Player Player { get; set; }
        public string LeagueId { get; set; }
        [JsonIgnore]
        public virtual League League { get; set; }
        public bool Applicant { get; set; }
    }
}
