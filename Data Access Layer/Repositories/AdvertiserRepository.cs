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
    /// Concrete implementation of the Advertiser Repository interface, using Entity Framework
    /// </summary>
    public class AdvertiserRepository : IAdvertiserRepository, IDisposable
    {
        private ApplicationDbContext context;

        public AdvertiserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteAdvertiser(int advertiserID)
        {
            Advertiser advertiser = context.Advertisers.Find(advertiserID);
            context.Advertisers.Remove(advertiser);
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

        public async Task<Advertiser> GetAdvertiserByID(int advertiserID)
        {
            return await context.Advertisers.FindAsync(advertiserID);
        }

        public IEnumerable<Advertiser> GetAdvertisers()
        {
            return context.Advertisers.ToList();
        }

        public void InsertAdvertiser(Advertiser advertiser)
        {
            context.Advertisers.Add(advertiser);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateAdvertiser(Advertiser advertiser)
        {
            context.Entry(advertiser).State = EntityState.Modified;
        }
    }
}
