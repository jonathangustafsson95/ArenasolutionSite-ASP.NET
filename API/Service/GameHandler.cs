using API.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using CommonLibrary;

namespace API.Service
{
    public class GameHandler
    {
        public List<IGameInterface> Games; 
        public List<Player> Players;
        private readonly IHubContext<GameHub> _HubContext;
        private readonly GameFactory _GameFactory;

        public GameHandler(IHubContext<GameHub> context, GameFactory GameFactory)
        {
            _HubContext = context;
            _GameFactory = GameFactory;
            Games = new List<IGameInterface>();
            Players = new List<Player>();
        }

        public Player GetOpponent(Player player)
        {          
            Player Opponent = Players.FirstOrDefault(p => p.CurrentGameType == player.CurrentGameType && p.UserId != player.UserId);
            if (Opponent != null && GetGame(Opponent) == null)
            {
                Players.Remove(Opponent);
                return Opponent;
            }
            else
            {
                if (Players.Contains(player))
                {
                    return null;
                }
                Players.Add(player);
                return null;
            }          
        }

        public IGameInterface GetGame(Player player)
        {
            foreach (var game in Games)
            {
                if (game.PlayerOne.UserId == player.UserId)
                {
                    game.PlayerOne = player;
                }
                else if (game.PlayerTwo.UserId == player.UserId)
                {
                    game.PlayerTwo = player;
                }
                return game;
            }
            return null;
        }

        public IGameInterface RemoveGame(string connectionId)
        {
            IGameInterface game = Games.FirstOrDefault(p => p.PlayerOne.ConnectionId == connectionId || p.PlayerTwo.ConnectionId == connectionId);
            try
            {   
                Games.Remove(game);
                return game;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IGameInterface CreateGame(Player player1, Player player2)
        {
            IGameInterface game = _GameFactory.CreateGame(player2, player1);
            Games.Add(game);
            return game;
        }

        public object UpdateBoard(int index, string connectionid)
        {
            IGameInterface game = Games.FirstOrDefault(g => g.PlayerOne.ConnectionId == connectionid || g.PlayerTwo.ConnectionId == connectionid);
            if (game != null)
            {
                if (game.PlayerOne.ConnectionId == connectionid && game.PlayerOneTurn)
                {
                    if (!game.IsPlayed(game.Board[index]))
                    {
                        game.Board[index] = game.PlayerOneMark;
                        game.PlayerOneTurn = false;
                        return game;
                    }
                    return "You can't place there..";
                }
                else if (game.PlayerTwo.ConnectionId == connectionid && !game.PlayerOneTurn)
                {
                    if (!game.IsPlayed(game.Board[index]))
                    {
                        game.Board[index] = game.PlayerTwoMark;
                        game.PlayerOneTurn = true;
                        return game;
                    }
                    return "You can't place there..";
                }
            }
            return "Wait for your turn.";
        }
        public object IsGameOver(IGameInterface game, string connectionid)
        {
            Player player;
            Player looser;
            if (game.PlayerOne.ConnectionId == connectionid)
            {
                player = game.PlayerOne;
            }
            else
            {
                player = game.PlayerTwo;
            }
            if (game.IsWon(game.Board, player))
            {
                looser = game.PlayerOne == player ? game.PlayerTwo : game.PlayerOne;
                var dict = new Dictionary<string, Player> { { "Winner", player }, { "Looser", looser} };
                return dict;
            }
            else if (game.GetAvailableSpots(game.Board).Count() == 0)
            {
                return "Draw";
            }
            else
                return game;
        }
    }
}
