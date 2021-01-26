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
    /// Concrete implementation of the Tournament Style Repository interface, using Entity Framework
    /// </summary>
    public class TournamentStyleRepository : ITournamentStyleRepository, IDisposable
    {
        private ApplicationDbContext context;

        public TournamentStyleRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteTournamentStyle(int TournamentStyleID)
        {
            TournamentStyle tournamentStyle = context.TournamentStyles.Find(TournamentStyleID);
            context.TournamentStyles.Remove(tournamentStyle);
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

        public async Task<TournamentStyle> GetTournamentStyleByID(int TournamentStyleID)
        {
            return await context.TournamentStyles.FindAsync(TournamentStyleID);
        }

        public IEnumerable<TournamentStyle> GetTournamentStyles()
        {
            return context.TournamentStyles.ToList();

        }

        public void InsertTournamentStyle(TournamentStyle tournamentStyle)
        {
            context.TournamentStyles.Add(tournamentStyle);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateTournamentStyle(TournamentStyle tournamentStyle)
        {
            context.Entry(tournamentStyle).State = EntityState.Modified;
        }

        public TournamentStyle SkipTask(int tournamentStyle)
        {
            return context.TournamentStyles.Skip(tournamentStyle).First();
        }

    }
}

