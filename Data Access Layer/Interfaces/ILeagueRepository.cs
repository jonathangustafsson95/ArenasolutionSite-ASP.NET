using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the League repository
    /// </summary>
    public interface ILeagueRepository : IDisposable
    {
        IEnumerable<League> GetLeagues();
        Task<League> GetLeagueByID(int LeagueID);
        void InsertLeague(League League);
        void DeleteLeague(int LeagueID);
        void UpdateLeague(League League);
        League SkipTask(int League);
        void Save();
    }
}
