using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// This class contains the functionality for Tournaments
    /// </summary>
    public class Tournament
    {
        public int TournamentId { get; set; }

        [Required]
        public string LeagueId { get; set; }
        public virtual League League { get; set; }

        [Required]
        public string TournamentName { get; set; }

        [Required]
        public string TournamentStyle { get; set; }

        [Required]
        [Range(2, 64, ErrorMessage = "Please enter a integer between 2 and 64")]
        public int MaxPlayer { get; set; }

        public int CurrentPlayers { get; set; }
        public virtual ICollection<TournamentPlayer> TournamentPlayers { get; set; }
        public virtual Knockout Knockout { get; set; }
    }
}
