using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the advertiser repository
    /// </summary>
    public interface IAdvertiserRepository : IDisposable
    {
        IEnumerable<Advertiser> GetAdvertisers();
        Task<Advertiser> GetAdvertiserByID(int advertID);
        void InsertAdvertiser(Advertiser advertiser);
        void DeleteAdvertiser(int advertiserID);
        void UpdateAdvertiser(Advertiser advertiser);
        void Save();
    }
}
