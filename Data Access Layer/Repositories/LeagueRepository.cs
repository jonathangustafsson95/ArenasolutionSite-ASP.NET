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
    /// Concrete implementation of the League repository interface, using Entity Framework
    /// </summary>
    public class LeagueRepository : ILeagueRepository, IDisposable
    {
        private ApplicationDbContext context;

        public LeagueRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteLeague(int LeagueID)
        {
            League League = context.Leagues.Find(LeagueID);
            context.Leagues.Remove(League);
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

        public async Task<League> GetLeagueByID(int LeagueID)
        {
            return await context.Leagues.FindAsync(LeagueID);
        }

        public IEnumerable<League> GetLeagues()
        {
            return context.Leagues.Include(c => c.LeagueOwner).ToList();
        }

        public void InsertLeague(League league)
        {
            context.Leagues.Add(league);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateLeague(League league)
        {
            context.Entry(league).State = EntityState.Modified;
        }

        public League SkipTask(int league)
        {
            return context.Leagues.Skip(league).First();
        }

    }
}
