using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Games;
using CommonLibrary;

namespace API.Service
{
    public class GameFactory
    {
        public IGameInterface CreateGame(Player playerOne, Player playerTwo)
        {
            switch (playerOne.CurrentGameType)
            {
                case "TicTacToe":
                    IGameInterface game = new TicTacToeEngine(playerOne, playerTwo);
                    game.MatchId = Guid.NewGuid().ToString();
                    return game;
                case "Othello":
                    return null;
                default:
                    return null;
            }
        }
    }
}
