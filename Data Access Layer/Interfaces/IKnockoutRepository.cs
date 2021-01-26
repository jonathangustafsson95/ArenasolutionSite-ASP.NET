using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IKnockoutRepository : IDisposable
    {
        IEnumerable<Knockout> GetKnockouts();
        Task<Knockout> GetKnockoutByID(int advertID);
        void InsertKnockout(Knockout knockout);
        void DeleteKnockout(int knockoutID);
        void UpdateKnockout(Knockout knockout);
        Knockout SkipTask(int knockout);
        void Save();
    }
}
