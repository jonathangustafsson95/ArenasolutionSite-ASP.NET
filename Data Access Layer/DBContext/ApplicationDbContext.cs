//using API.Models;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.DBContext
{
    /// <summary>
    /// This class creates the neccessary connections with Entity Framework.
    /// </summary>
    public class ApplicationDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<LeagueOwner> LeagueOwners { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<TournamentStyle> TournamentStyles { get; set; }
        public DbSet<LeagueMember> LeagueMembers { get; set; }
        public DbSet<Knockout> Knockouts { get; set; }
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            //Knockout config
            modelbuilder.Entity<Knockout>().HasKey(t => t.KnockoutId);

            modelbuilder.Entity<Knockout>()
                .HasOne(p => p.LeftNode);

            modelbuilder.Entity<Knockout>()
                .HasOne(p => p.RightNode);


            // LeagueMember config
            modelbuilder.Entity<LeagueMember>()
                .HasKey(a => new { a.LeagueId, a.UserId });

            modelbuilder.Entity<LeagueMember>()
                .HasOne(a => a.League)
                .WithMany(a => a.LeagueMembers)
                .HasForeignKey(a => a.LeagueId)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<LeagueMember>()
                .HasOne(a => a.Player)
                .WithMany(a => a.LeagueMemberShips)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // League/LeagueOwner config
            modelbuilder.Entity<League>()
                .HasOne(a => a.LeagueOwner)
                .WithMany(a => a.Leagues)
                .HasForeignKey(a => a.LeagueOwnerId);

            // TournamentPlayer config
            modelbuilder.Entity<TournamentPlayer>()
                .HasKey(a => new { a.TournamentId, a.UserId });

            modelbuilder.Entity<TournamentPlayer>()
                .HasOne(a => a.Tournament)
                .WithMany(a => a.TournamentPlayers)
                .HasForeignKey(a => a.TournamentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<TournamentPlayer>()
                .HasOne(a => a.Player)
                .WithMany(a => a.TournamentMemberships)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<Operator>()
                .HasData(
                new Operator
                {
                    UserId = 1,
                    UserName = "Jonte",
                    Password = "123"
                });
        }

        //public ApplicationDbContext(DbContextOptions options)
        //    : base(options)
        //{

        //}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn = "Server=sqlutb2-db.hb.se,56077;Database=oopc1911;User Id=oopc1911;Password=wp3437";
                optionsBuilder.UseSqlServer(conn);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
