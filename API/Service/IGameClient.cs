using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public interface IGameClient
    {
        Task GameState(string[] board);
        Task SetConnectionId(string Id);
        Task NotifyUser(string message);
        Task AnnounceGameOver(string message, string[] board);
        Task BackToGames(string message);
    }
}
