using CommonLibrary;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    //En kort kommentar så att det har skett en "change"...
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context;
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        private GenericRepository<User> userRepository;
        private GenericRepository<Advert> advertRepository;
        private GenericRepository<Advertiser> advertiserRepository;
        private GenericRepository<Tournament> tournamentRepository;
        private GenericRepository<TournamentPlayer> tournamentPlayerRepository;
        private GenericRepository<TournamentStyle> tournamentStyleRepository;
        private GenericRepository<League> leagueRepository;
        private GenericRepository<Knockout> knockoutRepository;
        private GenericRepository<LeagueMember> leagueMemberRepository;

        public GenericRepository<Knockout> KnockoutRepository
        {
            get
            {
                if (this.knockoutRepository == null)
                {
                    this.knockoutRepository = new GenericRepository<Knockout>(context);
                }
                return knockoutRepository;
            }
        }
        public GenericRepository<TournamentPlayer> TournamentPlayerRepository
        {
            get
            {
                if (this.tournamentPlayerRepository == null)
                {
                    this.tournamentPlayerRepository = new GenericRepository<TournamentPlayer>(context);
                }
                return tournamentPlayerRepository;
            }
        }
        public GenericRepository<TournamentStyle> TournamentStyleRepository
        {
            get
            {
                if (this.tournamentStyleRepository == null)
                {
                    this.tournamentStyleRepository = new GenericRepository<TournamentStyle>(context);
                }
                return tournamentStyleRepository;
            }
        }
        public GenericRepository<Tournament> TournamentRepository
        {
            get
            {
                if (this.tournamentRepository == null)
                {
                    this.tournamentRepository = new GenericRepository<Tournament>(context);
                }
                return tournamentRepository;
            }
        }
        public GenericRepository<League> LeagueRepository
        {
            get
            {
                if (this.leagueRepository == null)
                {
                    this.leagueRepository = new GenericRepository<League>(context);
                }
                return leagueRepository;
            }
        }
        public GenericRepository<LeagueMember> LeagueMemberRepository
        {
            get
            {
                if (this.leagueMemberRepository == null)
                {
                    this.leagueMemberRepository = new GenericRepository<LeagueMember>(context);
                }
                return leagueMemberRepository;
            }
        }
        public GenericRepository<Advert> AdvertRepository
        {
            get 
            {
                if (this.advertRepository == null)
                {
                    this.advertRepository = new GenericRepository<Advert>(context);
                }
                return advertRepository;
            }
        }
        public GenericRepository<Advertiser> AdvertiserRepository
        {
            get
            {
                if (this.advertiserRepository == null)
                {
                    this.advertiserRepository = new GenericRepository<Advertiser>(context);
                }
                return advertiserRepository;
            }
        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
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
    }
}
