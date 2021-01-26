using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessLayer;

namespace API.Tournaments
{
    public abstract class TournamentAlgorithm
    {
        protected UnitOfWork unitOfWork;
        public TournamentAlgorithm(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public abstract void StartTournament(int tournamentId);
        public abstract void RecordWinLoss(int tournamentId, int userId1);
        public abstract List<Knockout> GetMatches(int tournamentId);
    }
}
