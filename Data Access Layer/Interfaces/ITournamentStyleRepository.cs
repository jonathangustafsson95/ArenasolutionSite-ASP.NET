using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the Tournament Style repository
    /// </summary>
    public interface ITournamentStyleRepository : IDisposable
    {
        IEnumerable<TournamentStyle> GetTournamentStyles();
        Task<TournamentStyle> GetTournamentStyleByID(int TournamentStyleId);
        void InsertTournamentStyle(TournamentStyle tournamentStyle);
        void DeleteTournamentStyle(int TournamentStyleId);
        void UpdateTournamentStyle(TournamentStyle tournamentStyle);
        TournamentStyle SkipTask(int tournament);
        void Save();
    }
}
