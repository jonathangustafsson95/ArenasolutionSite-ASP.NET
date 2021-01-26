using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the LeagueMember repository
    /// </summary>
    interface ILeagueMemberRepository : IDisposable
    {
        IEnumerable<LeagueMember> GetLeagueMembers();
        Task<LeagueMember> GetLeagueMemberByID(int LeagueMemberID);
        void InsertLeagueMember(LeagueMember LeagueMember);
        void DeleteLeagueMember(int LeagueMemberID);
        void UpdateLeagueMember(LeagueMember LeagueMember);
        LeagueMember SkipTask(int LeagueMember);
        void Save();
    }
}
