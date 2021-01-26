using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the TournamentPlayer repository
    /// </summary>
    public interface ITournamentPlayerRepository : IDisposable
    {
        IEnumerable<TournamentPlayer> GetTournamentPlayers();
        Task<TournamentPlayer> GetTournamentPlayerByID(int TournamentPlayerID);
        void InsertTournamentPlayer(TournamentPlayer tournamentPlayer);
        void DeleteTournamentPlayer(int TournamentPlayerID);
        void UpdateTournamentPlayer(TournamentPlayer tournamentPlayer);
        TournamentPlayer SkipTask(int tournamentPlayer);
        void Save();
    }
}
