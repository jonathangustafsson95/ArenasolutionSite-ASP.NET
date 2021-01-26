using CommonLibrary;
using DataAccessLayer.DBContext;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Concrete implementation of the League member Repository interface, using Entity Framework
    /// </summary>
    public class LeagueMemberRepository : ILeagueMemberRepository, IDisposable
    {
        private ApplicationDbContext context;

        public LeagueMemberRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteLeagueMember(int LeagueMemberID)
        {
            LeagueMember leagueMember = context.LeagueMembers.Find(LeagueMemberID);
            context.LeagueMembers.Remove(leagueMember);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<LeagueMember> GetLeagueMemberByID(int LeagueMemberID)
        {
            return await context.LeagueMembers.FindAsync(LeagueMemberID);
        }

        public IEnumerable<LeagueMember> GetLeagueMembers()
        {
            return context.LeagueMembers.ToList();
        }

        public void InsertLeagueMember(LeagueMember leagueMember)
        {
            context.LeagueMembers.Add(leagueMember);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateLeagueMember(LeagueMember leagueMember)
        {
            context.Entry(leagueMember).State = EntityState.Modified;
        }

        public LeagueMember SkipTask(int leagueMember)
        {
            return context.LeagueMembers.Skip(leagueMember).First();
        }

    }
}
