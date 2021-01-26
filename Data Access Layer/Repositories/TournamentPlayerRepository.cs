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
    /// Concrete implementation of the Tournament player repository interface, using Entity Framework
    /// </summary>
    public class TournamentPlayerRepository : ITournamentPlayerRepository, IDisposable
    {
        private ApplicationDbContext context;

        public TournamentPlayerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteTournamentPlayer(int TournamentPlayerID)
        {
            TournamentPlayer tournamentPlayer = context.TournamentPlayers.Find(TournamentPlayerID);
            context.TournamentPlayers.Remove(tournamentPlayer);
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

        public async Task<TournamentPlayer> GetTournamentPlayerByID(int TournamentPlayerID)
        {
            return await context.TournamentPlayers.FindAsync(TournamentPlayerID);
        }

        public IEnumerable<TournamentPlayer> GetTournamentPlayers()
        {
            return context.TournamentPlayers.ToList();
        }

        public void InsertTournamentPlayer(TournamentPlayer tournamentPlayer)
        {
            context.TournamentPlayers.Add(tournamentPlayer);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateTournamentPlayer(TournamentPlayer tournamentPlayer)
        {
            context.Entry(tournamentPlayer).State = EntityState.Modified;
        }

        public TournamentPlayer SkipTask(int tournamentPlayer)
        {
            return context.TournamentPlayers.Skip(tournamentPlayer).First();
        }

    }
}


