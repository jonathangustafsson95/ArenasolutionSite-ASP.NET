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
    /// Concrete implementation of the Advert Repository interface, using Entity Framework
    /// </summary>
    public class AdvertRepository : IAdvertRepository, IDisposable
    {
        private ApplicationDbContext context;

        public AdvertRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteAdvert(int advertID)
        {
            Advert advert = context.Adverts.Find(advertID);
            context.Adverts.Remove(advert);
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

        public async Task<Advert> GetAdvertByID(int advertID)
        {
            return await context.Adverts.FindAsync(advertID);
        }

        public IEnumerable<Advert> GetAdverts()
        {
            return context.Adverts.ToList();
        }

        public void InsertAdvert(Advert advert)
        {
            context.Adverts.Add(advert);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateAdvert(Advert advert)
        {
            context.Entry(advert).State = EntityState.Modified;
        }

        public Advert SkipTask(int advert)
        {
            return context.Adverts.Skip(advert).First();
        }

    }
}
