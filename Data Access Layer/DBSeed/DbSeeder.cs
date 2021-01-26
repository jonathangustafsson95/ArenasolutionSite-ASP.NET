using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using API.Models;
using CommonLibrary;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    /// <summary>
    /// Initial database seed for when everything goes wrong. :)
    /// </summary>
    public class DbSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var _context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for users
                if (_context.Users.Any())
                {
                    return; // DB has been seeded
                }

                _context.Leagues.AddRange(
                new League()
                {
                    LeagueName = "HerpDerp"
                },
                new League()
                {
                    LeagueName = "Drinkomanana"
                });
                _context.SaveChanges();


                _context.Users.AddRange(
                    new LeagueOwner()
                    {
                        UserName = "Joel",
                        Password = "123"
                    },
                    new Player()
                    {
                        UserName = "Jonathan",
                        Password = "123"
                    },
                    new Operator()
                    {
                        UserName = "Jonte",
                        Password = "123"
                    },
                    new Advertiser()
                    {
                        UserName = "Jol",
                        Password = "123",
                        Balance = 200
                    },
                    new LeagueOwner()
                    {
                        UserName = "Linda",
                        Password = "123"
                    });

                _context.SaveChanges();
            }
        }
    }
}
