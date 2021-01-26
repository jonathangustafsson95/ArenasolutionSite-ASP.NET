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
    public class KnockoutRepository : IKnockoutRepository, IDisposable
    {
        private ApplicationDbContext context;

        public KnockoutRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteKnockout(int knockoutID)
        {
            Knockout knockout = context.Knockouts.Find(knockoutID);
            context.Knockouts.Remove(knockout);
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

        public async Task<Knockout> GetKnockoutByID(int knockoutID)
        {
            return await context.Knockouts.FindAsync(knockoutID);
        }

        public IEnumerable<Knockout> GetKnockouts()
        {
            return context.Knockouts.ToList();
        }

        public void InsertKnockout(Knockout knockout)
        {
            context.Knockouts.Add(knockout);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateKnockout(Knockout knockout)
        {
            context.Entry(knockout).State = EntityState.Modified;
        }

        public Knockout SkipTask(int knockout)
        {
            return context.Knockouts.Skip(knockout).First();
        }
    }
}
