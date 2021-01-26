using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the advert repository
    /// </summary>
    public interface IAdvertRepository : IDisposable
    {
        IEnumerable<Advert> GetAdverts();
        Task<Advert> GetAdvertByID(int advertID);
        void InsertAdvert(Advert advert);
        void DeleteAdvert(int advertID);
        void UpdateAdvert(Advert advert);
        Advert SkipTask(int advert);
        void Save();
    }
}
