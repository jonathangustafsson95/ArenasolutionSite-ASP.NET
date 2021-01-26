using System;
using System.Collections.Generic;
using System.Text;

namespace API.Library.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public  Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public bool PlayerOneTurn { get; set; }
        public string[] Board { get; set; }
        public Match(string[] board, Player playerOne, Player playerTwo, bool playerOneTurn = true)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneTurn = playerOneTurn;
            Board = board;
        }

    }
}
