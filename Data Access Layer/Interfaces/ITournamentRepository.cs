using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the Tournament repository
    /// </summary>
    public interface ITournamentRepository : IDisposable
    {
        IEnumerable<Tournament> GetTournaments();
        Task<Tournament> GetTournamentByID(int TournamentID);
        void InsertTournament(Tournament tournament);
        void DeleteTournament(int TournamentID);
        void UpdateTournament(Tournament tournament);
        Tournament SkipTask(int tournament);
        void Save();
    }
}
