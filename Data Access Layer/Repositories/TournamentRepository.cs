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
    /// Concrete implementation of the Tournament Repository interface, using Entity Framework
    /// </summary>
    public class TournamentRepository : ITournamentRepository, IDisposable
    {
        private ApplicationDbContext context;

        public TournamentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteTournament(int TournamentID)
        {
            Tournament tournament = context.Tournaments.Find(TournamentID);
            context.Tournaments.Remove(tournament);
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

        public async Task<Tournament> GetTournamentByID(int TournamentID)
        {
            return await context.Tournaments.FindAsync(TournamentID);
        }

        public IEnumerable<Tournament> GetTournaments()
        {
            return context.Tournaments.ToList();

        }

        public void InsertTournament(Tournament tournament)
        {
            context.Tournaments.Add(tournament);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateTournament(Tournament tournament)
        {
            context.Entry(tournament).State = EntityState.Modified;
        }

        public Tournament SkipTask(int tournament)
        {
            return context.Tournaments.Skip(tournament).First();
        }

    }
}

