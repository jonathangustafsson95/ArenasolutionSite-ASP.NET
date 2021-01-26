using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Games
{
    public class TicTacToeEngine : IGameInterface
    {
        public string MatchId { get; set; }
        public string PlayerOneMark { get; set; }
        public string PlayerTwoMark { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public bool PlayerOneTurn { get; set; }
        public string[] Board { get; set; }

        public TicTacToeEngine(Player playerOne, Player playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            Board = new string[9];
            PlayerOneTurn = true;
            PlayerOneMark = "X";
            PlayerTwoMark = "O";
        }
        public string[] GetAvailableSpots(string[] board)
        {
            return board.Where(i => !IsPlayed(i)).ToArray();
        }

        public bool IsWon(string[] board, Player player)
        {
            string playerMark;
            if (player.UserId == PlayerOne.UserId)
            {
                playerMark = PlayerOneMark;
            }
            else 
            {
                playerMark = PlayerTwoMark;
            }
            if (
                   (board[0] == playerMark && board[1] == playerMark && board[2] == playerMark) ||
                   (board[3] == playerMark && board[4] == playerMark && board[5] == playerMark) ||
                   (board[6] == playerMark && board[7] == playerMark && board[8] == playerMark) ||
                   (board[0] == playerMark && board[3] == playerMark && board[6] == playerMark) ||
                   (board[1] == playerMark && board[4] == playerMark && board[7] == playerMark) ||
                   (board[2] == playerMark && board[5] == playerMark && board[8] == playerMark) ||
                   (board[0] == playerMark && board[4] == playerMark && board[8] == playerMark) ||
                   (board[2] == playerMark && board[4] == playerMark && board[6] == playerMark)
                   )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPlayed(string input)
        {
            return input == "X" || input == "O";
        }
    }
}

