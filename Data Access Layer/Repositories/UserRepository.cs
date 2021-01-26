using CommonLibrary;
using DataAccessLayer.DBContext;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Concrete implementation of the User Repository interface, using Entity Framework
    /// </summary>
    public class UserRepository : IUserRepository, IDisposable
    {
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeleteUser(int userID)
        {
            User user = context.Users.Find(userID);
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

        public async Task<User> GetUserByID(int userID)
        {
            return await context.Users.FindAsync(userID);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(username));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }

        public void InsertUser(User user)
        {
            context.Users.Add(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
