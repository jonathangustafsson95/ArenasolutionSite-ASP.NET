using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for the User repository
    /// </summary>
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByID(int userID);
        Task<User> GetUserByName(string username);
        void InsertUser(User user);
        void DeleteUser(int userID);
        void UpdateUser(User user);
        void Save();
    }
}
