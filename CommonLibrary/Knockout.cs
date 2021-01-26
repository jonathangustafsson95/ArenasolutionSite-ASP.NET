using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;

namespace CommonLibrary
{
    public class Knockout
    {
        public int KnockoutId { get; set; }
        public Knockout LeftNode { get; set; }
        public Knockout RightNode { get; set; }
        public TournamentPlayer player1 { get; set; }
        public TournamentPlayer player2 { get; set; }
    }
}
