using API.Games;
using CommonLibrary;
using API.Service;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;

namespace API.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        private GameHandler _GameHandler;
        private GameFactory _GameFactory;
        private UnitOfWork _UnitOfWork;

        public GameHub(GameHandler GameHandler, GameFactory GameFactory, UnitOfWork unitOfWork)
        {
            _GameHandler = GameHandler;
            _GameFactory = GameFactory;
            _UnitOfWork = unitOfWork;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task OnPlayerReceived(Player player)
        {
            try
            {
                player.ConnectionId = Context.ConnectionId;
                await Clients.Caller.SetConnectionId(Context.ConnectionId);

                IGameInterface ongoingGame = _GameHandler.GetGame(player);
                if (ongoingGame != null)
                {
                    await Groups.AddToGroupAsync(player.ConnectionId, groupName: ongoingGame.MatchId);
                    await Clients.Groups(ongoingGame.MatchId).GameState(ongoingGame.Board);                    
                }
                if (_GameHandler.Players.Count() >= 1)
                {
                    Player playerTwo = _GameHandler.GetOpponent(player);
                    if (playerTwo != null)
                    {
                        IGameInterface game = _GameHandler.CreateGame(player, playerTwo);
                        await Groups.AddToGroupAsync(player.ConnectionId, groupName: game.MatchId);
                        await Groups.AddToGroupAsync(playerTwo.ConnectionId, groupName: game.MatchId);
                        await Clients.Groups(game.MatchId).GameState(game.Board);
                    }
                    else
                    {
                        _GameHandler.Players.Add(player);
                    }
                }
                else
                {
                    _GameHandler.Players.Add(player);
                }         
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public async Task OnUserMoveReceived(int index)
        {
            var gameState = _GameHandler.UpdateBoard(index, Context.ConnectionId);
            if (gameState.GetType() == typeof(string))
            {
                await Clients.Caller.NotifyUser(gameState as string);
            }
            else
            {
                var game = _GameHandler.IsGameOver(gameState as IGameInterface, Context.ConnectionId);
                if(game.GetType() == typeof(string))
                {
                    await Clients.Groups((gameState as IGameInterface).MatchId).AnnounceGameOver(game as string, (gameState as IGameInterface).Board);
                }
                else if (game.GetType() == typeof(Dictionary<string, Player>))
                {
                    var dict = game as Dictionary<string, Player>;
                    UpdateRating(dict["Winner"], dict["Looser"]);
                    await Clients.Groups((gameState as IGameInterface).MatchId).AnnounceGameOver(dict["Winner"].UserName , (gameState as IGameInterface).Board);
                }
                else
                {
                    await Clients.Groups((game as IGameInterface).MatchId).GameState((game as IGameInterface).Board);
                }
            }
        }
        public async Task OnGameOver(string why)
        {
            IGameInterface game = _GameHandler.RemoveGame(Context.ConnectionId);
            string message;
            if (why == "forfeit")
            {
                if (game.PlayerOne.ConnectionId == Context.ConnectionId)
                {
                    UpdateRating(game.PlayerTwo, game.PlayerOne);
                }
                else
                {
                    UpdateRating(game.PlayerOne, game.PlayerTwo);
                }
                message = "A player forfeited the game, ratings have been recorded";
            }
            else
            {
                message = "A player left the game-session, ratings have been recorded";
            }
            await Clients.Groups(game.MatchId).BackToGames(message);
            await Groups.RemoveFromGroupAsync(game.PlayerOne.ConnectionId, game.MatchId);
            await Groups.RemoveFromGroupAsync(game.PlayerTwo.ConnectionId, game.MatchId);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {

            return base.OnDisconnectedAsync(exception);
        }
        public void UpdateRating(Player player1, Player player2)
        {
            Player winner = (Player)_UnitOfWork.UserRepository.GetByID(player1.UserId);
            Player looser = (Player)_UnitOfWork.UserRepository.GetByID(player2.UserId);
            winner.Rating += 25;
            looser.Rating -= 25;

            _UnitOfWork.UserRepository.Update(winner);
            _UnitOfWork.UserRepository.Update(looser);
            _UnitOfWork.Save();
        }
    }
}
