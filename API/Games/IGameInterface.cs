using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace API.Games
{
    public interface IGameInterface
    {
        public string MatchId { get; set; }
        public string PlayerOneMark { get; set; }
        public string PlayerTwoMark { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public bool PlayerOneTurn { get; set; }
        public string[] Board { get; set; }

        string[] GetAvailableSpots(string[] board);
        bool IsWon(string[] board, Player player);
        bool IsPlayed(string input);
    }
}
