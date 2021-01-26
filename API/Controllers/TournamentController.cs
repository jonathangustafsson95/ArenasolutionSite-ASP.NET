using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using System.Net.Http;
using Newtonsoft.Json;
using API.Tournaments;

namespace API.Controllers
{
    /// <summary>
    /// Controller for Tournaments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private UnitOfWork unitOfWork;

        /// <summary>
        /// Constructor for TournamentController
        /// </summary>
        /// <param name="unitOfWork">Repository dependency injection</param>
        public TournamentController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new tournament and adds it to the database
        /// </summary>
        /// <param name="tournament">A specific tournament</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("CreateTournament")]
        public ActionResult<string> CreateTournament(Tournament tournament)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tournament.CurrentPlayers = 0;
                    unitOfWork.TournamentRepository.Insert(tournament);
                    unitOfWork.Save();
                    return StatusCode(200, "OK");
                }
                else
                {
                    return StatusCode(500, "Something went wrong!");
                }   
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Collects all currently active tournaments connected to
        /// a specific League 
        /// </summary>
        /// <param name="Leagueid">A certain League's ID</param>
        /// <returns>Returns a specific tournament connected to a specific League or a statuscode</returns>
        [HttpGet]
        [Route("GetSpecificTournament")]
        public ActionResult<Tournament> GetSpecificTournament(int tournamentId)
        {
            try
            {
                foreach (var item in unitOfWork.TournamentRepository.Get())
                {
                    if (item.TournamentId == tournamentId)
                    {
                        return item;
                    }
                }
                return StatusCode(500, "Something went wrong!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Collects a list of tournaments depending on what kind of 
        /// user that is used(Player, LeagueOwner etc.)
        /// </summary>
        /// <param name="userId">A user's ID</param>
        /// <returns>Returns a list of tournaments or a statuscode depending on the results</returns>
        [HttpGet]
        [Route("GetActiveTournaments")]
        public ActionResult<List<Tournament>> GetActiveTournaments(string userId=null)
        {
            List<Tournament> TournamentList = new List<Tournament>();
            try
            {
                if (userId != null)
                {
                    Int32.TryParse(userId, out int UserId);
                    User user = unitOfWork.UserRepository.GetByID(UserId);
                    if (user.GetType() == typeof(Player))
                    {
                        foreach (var league in unitOfWork.TournamentRepository.Get(null, null, "League"))
                        {
                            foreach (var leagueMembership in (user as Player).LeagueMemberShips)
                            {
                                if (league.LeagueId == leagueMembership.LeagueId)
                                {
                                    TournamentList.Add(league);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var league in unitOfWork.TournamentRepository.Get(null, null, "League"))
                        {
                            foreach (var League in (user as LeagueOwner).Leagues)
                            {
                                if (league.LeagueId == League.LeagueId)
                                {
                                    TournamentList.Add(league);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var league in unitOfWork.TournamentRepository.Get(null, null, "League"))
                    {
                        TournamentList.Add(league);
                    }
                }
                return TournamentList;
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Registers a specific player to a tournament
        /// </summary>
        /// <param name="dict">A dictionary containing a player's ID and a tournament's ID</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("RegisterForTournament")]
        public ActionResult<string> RegisterForTournament([FromBody]Dictionary<string, string> dict)
        {
            try
            {
                int userId;
                Int32.TryParse(dict["UserId"], out userId);

                int tournamentId;
                Int32.TryParse(dict["TournamentId"], out tournamentId);

                Tournament tournament= unitOfWork.TournamentRepository.GetByID(tournamentId);
                TournamentPlayer newRegister = new TournamentPlayer() 
                { 
                    TournamentId = tournament.TournamentId,
                    UserId = userId
                };

                List<TournamentPlayer> tournamentPlayerList = new List<TournamentPlayer>();
                //Checks if player already registred or not
                foreach (var item in unitOfWork.TournamentPlayerRepository.Get())
                {
                    if(item.TournamentId == tournamentId && item.UserId == userId)
                    {
                        return StatusCode(500, "Something went wrong!");
                    }
                }

                if (tournament.CurrentPlayers < tournament.MaxPlayer)
                {
                    unitOfWork.TournamentPlayerRepository.Insert(newRegister);
                    tournament.CurrentPlayers += 1;
                    if (tournament.CurrentPlayers == tournament.MaxPlayer)
                        StartTournament(tournament.TournamentId);
                }
                
                
                unitOfWork.TournamentRepository.Update(tournament);
                unitOfWork.Save();
                return StatusCode(200, "OK");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Collects all tournaments connected to a certain player
        /// </summary>
        /// <param name="userId">A certain user</param>
        /// <returns>Returns a list of tournaments or a statuscode depending on the results</returns>
        [HttpGet]
        [Route("GetUserTournaments")]
        public ActionResult<List<TournamentPlayer>> GetUserTournaments(int userId)
        {
            List<TournamentPlayer> TournamentList = new List<TournamentPlayer>();

            try
            {
                foreach (var item in unitOfWork.TournamentPlayerRepository.Get())
                {
                    if(item.UserId == userId)
                        TournamentList.Add(item);
                }
                return TournamentList;
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Edits a tournament
        /// </summary>
        /// <param name="tournament">A specific tournament</param>
        /// <returns>Returns a statuscode</returns>
        [HttpPost]
        [Route("EditTournament")]
        public ActionResult<string> EditTournament(Tournament tournament)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TournamentRepository.Update(tournament);
                    unitOfWork.Save();
                    return StatusCode(200, "OK");
                }
                return StatusCode(500, "Something went wrong!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Deletes a tournament
        /// </summary>
        /// <param name="tournamentId">A specific tournament</param>
        /// <returns>Returns a statuscode</returns>
        [HttpGet]
        [Route("DeleteTournament")]
        public ActionResult<string> DeleteTournament(int tournamentId)
        {
            try
            {
                foreach (var item in unitOfWork.TournamentRepository.Get())
                {
                    if (item.TournamentId == tournamentId)
                    {
                        unitOfWork.TournamentRepository.Delete(item);
                        unitOfWork.Save();
                        return StatusCode(200, "OK");
                    }
                }
                return StatusCode(500, "Something went wrong!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        [HttpGet]
        [Route("AddWinLoss")]
        public ActionResult<string> AddWinLoss(int tournamentId, int winnerUserId, int loserUserId)
        {
            try
            {
                TournamentAlgorithm tournamentAlgorithm;
                Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);

                if (tournament.TournamentStyle == "KnockOut")
                    tournamentAlgorithm = new KnockoutAlgorithm(unitOfWork);
                else
                    return StatusCode(500, "Something went wrong!");

                tournamentAlgorithm.RecordWinLoss(tournament.TournamentId, winnerUserId);

                return StatusCode(200, "OK");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        [HttpGet]
        [Route("StartTournament")]
        public ActionResult<string> StartTournament(int tournamentId)
        {
            try
            {
                TournamentAlgorithm tournamentAlgorithm;
                Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);

                if (tournament.TournamentStyle == "KnockOut")
                    tournamentAlgorithm = new KnockoutAlgorithm(unitOfWork);
                else
                    return StatusCode(500, "Something went wrong!");

                tournamentAlgorithm.StartTournament(tournament.TournamentId);
                return StatusCode(200, "OK");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        [HttpGet]
        [Route("GetTournamentMatches")]
        public ActionResult<List<Knockout>> GetTournamentMatches(int tournamentId)
        {
            try
            {
                TournamentAlgorithm tournamentAlgorithm;
                Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);

                if (tournament.TournamentStyle == "KnockOut")
                    tournamentAlgorithm = new KnockoutAlgorithm(unitOfWork);
                else
                    return StatusCode(500, "Something went wrong!");

                return tournamentAlgorithm.GetMatches(tournament.TournamentId);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
    }
}