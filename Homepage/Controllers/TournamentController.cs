using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Homepage.Models;
using CommonLibrary;
using Homepage.Service;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using CommonLibrary.ViewModels;

namespace Homepage.Controllers
{
    /// <summary>
    /// Controller for Tournaments
    /// </summary>
    public class TournamentController : Controller
    {
        private readonly APIhelper _APIhelper;
        private readonly SessionHelper _SessionHelper;
        private readonly Adhelper _Adhelper;

        /// <summary>
        /// Constructor for AdvertisementController
        /// The parameters are dependency injections
        /// </summary>
        public TournamentController(APIhelper aPIhelper, SessionHelper SessionHelper, Adhelper Adhelper)
        {
            _APIhelper = aPIhelper;
            _SessionHelper = SessionHelper;
            _Adhelper = Adhelper;
        }

        /// <summary>
        /// Shows all active tournaments on the front page
        /// </summary>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> Index()
        {
            List<Tournament> TournamentList = new List<Tournament>();
            string errorMessage = "No tournaments available";

            TournamentList = await GetActiveTournaments();
            ViewData["ActiveTournaments"] = TournamentList;

            if (TournamentList.Count == 0)
            {
                ViewData["ActiveTournaments"] = null;
                ViewBag.Error = errorMessage;
            }

            return View(new TournamentViewModel() {Advert = await _Adhelper.ShowAd()});
        }

        /// <summary>
        /// Shows all active tournaments for specific Leagues' that the
        /// player belongs to
        /// </summary>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> Play()
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
                return RedirectToAction("Index", "Home");

            List<Tournament> TournamentList = new List<Tournament>();
            string errorMessage = "No tournaments available";

            TournamentList = await GetActiveTournaments();
            ViewData["ActiveTournaments"] = TournamentList;
            ViewBag.UserTournaments = await GetUserTournaments(user);

            if (TournamentList.Count == 0)
            {
                ViewData["ActiveTournaments"] = null;
                ViewBag.Error = errorMessage;
            }
            return View(new TournamentViewModel() {Advert = await _Adhelper.ShowAd(), User = user});
        }

        /// <summary>
        /// Shows all tournaments that belongs to the 
        /// LeagueOwners League/Leagues'
        /// </summary>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> Admin()
        {
            List<Tournament> TournamentList = new List<Tournament>();
            string errorMessage = "No tournaments available";

            LeagueOwner user = (LeagueOwner)_SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
                return RedirectToAction("Index", "Home");

            TournamentList = await GetActiveTournaments(user);
            ViewData["ActiveTournaments"] = TournamentList;

            if (TournamentList.Count == 0)
            {
                ViewData["ActiveTournaments"] = null;
                ViewBag.Error = errorMessage;
            }

            return View(new TournamentViewModel() {Advert = await _Adhelper.ShowAd()});
        }

        /// <summary>
        /// Creates a tournament
        /// </summary>
        /// <param name="tournament">The tournament a LeagueOwner wants to create</param>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> CreateTournament(Tournament tournament=null )
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                string uri = "/Tournament/CreateTournament/";
                try
                {
                    var response = await _APIhelper.PostTournamentAsync(uri, tournament);
                }
                catch (Exception)
                {
                    return RedirectToAction("Admin");
                }
                return RedirectToAction("Admin");
            }
            return View(new TournamentViewModel() { User = user, Advert = await _Adhelper.ShowAd(), tournament = tournament });
        }

        /// <summary>
        /// A helping method to Play() that collects all tournaments connected to a specific player
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns a list with TournamentPlayers</returns>
        private async Task<List<TournamentPlayer>> GetUserTournaments(User user)
        {
            List<TournamentPlayer> newList = new List<TournamentPlayer>();
            string uri = $"/Tournament/GetUserTournaments/?userId={user.UserId}";
            try
            {
                var response = await _APIhelper.GetTournamentPlayerAsync(uri);
                if (response.GetType() == typeof(List<TournamentPlayer>))
                {
                    newList = response as List<TournamentPlayer>;
                }
            }
            catch (Exception)
            {
                return newList;
            }
            return newList;
        }

        /// <summary>
        /// A helping method that collects all active tournaments for a 
        /// specific player or LeagueOwner
        /// </summary>
        /// <param name="user">A specific user</param>
        /// <returns>Returns a list of tournaments</returns>
        private async Task<List<Tournament>> GetActiveTournaments(User user=null)
        {
            List<Tournament> newList = new List<Tournament>();
            string uri;

            if (user == null)
            {
                uri = "/Tournament/GetActiveTournaments/";
            }
            else
            {
                uri = $"/Tournament/GetActiveTournaments/?Leagueid={user.UserId}";
            }
            try
            {
                var response = await _APIhelper.GetTournamentAsync(uri);
                if (response.GetType() == typeof(List<Tournament>))
                {
                    newList = response as List<Tournament>;
                }
            }
            catch (Exception)
            {
                return newList;
            }
            return newList;
        }

        /// <summary>
        /// Finds a specific tournament and returns it
        /// </summary>
        /// <param name="tournamentId">A tournament</param>
        /// <returns>Returns a specific tournament</returns>
        private async Task<Tournament> GetSpecificTournament(int tournamentId)
        {
            try
            {
                string uri = $"/Tournament/GetSpecificTournament/?tournamentId={tournamentId}";
                var response = await _APIhelper.GetTournamentAsync(uri);
                if (response.GetType() == typeof(string))
                {
                    return null;
                }
                return response as Tournament;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Edits a tournament
        /// </summary>
        /// <param name="tournament">A specific tournament</param>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> EditTournament(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uri = "/Tournament/EditTournament/";
                    var response = await _APIhelper.PostTournamentAsync(uri, tournament);
                }
                catch (Exception)
                {
                    return RedirectToAction("Admin");
                }
            }
            return RedirectToAction("Admin");
        }

        /// <summary>
        /// Delete a tournament
        /// </summary>
        /// <param name="TournamentId">Tournament ID for the tournament the LeagueOwner wishes to delete</param>
        /// <returns>Redirects to a view</returns>
        public async Task<IActionResult> DeleteTournament(int TournamentId)
        {
            if (ModelState.IsValid)
            {
                string uri = $"/Tournament/DeleteTournament/?tournamentId={TournamentId}";
                try
                {
                    var response = await _APIhelper.GetTournamentAsync(uri);
                }
                catch (Exception)
                {
                    RedirectToAction("Admin");
                }
            }
            return RedirectToAction("Admin");
        }

        /// <summary>
        /// Gets the specific tournament ID that LeagueOwner
        /// wishes to edit and forwards it to the edit page
        /// </summary>
        /// <param name="tournamentId">The tournament ID for the tournament the LeagueOwner wishes to edit</param>
        /// <returns>Returns a view</returns>
        public async Task<IActionResult> OnClickEdit(int tournamentId)
        {
            Tournament tournament = await GetSpecificTournament(tournamentId);
            return View(new TournamentViewModel() {Advert = await _Adhelper.ShowAd(), tournament = tournament});
        }

        /// <summary>
        /// Register for a tournament
        /// </summary>
        /// <param name="tournamentId">Tournament ID for the tournament that the player want to register to</param>
        /// <returns>Redirects to a view depending on the situation</returns>
        public async Task<IActionResult> OnClickRegister(int tournamentId)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var value = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournamentId.ToString() }
            };
            try
            {
                string uri = "/Tournament/RegisterForTournament/";
                var response = await _APIhelper.PostTournamentAsync(uri, value);
            }
            catch (Exception)
            {
                return RedirectToAction("Play");
            }
            return RedirectToAction("Play");
        }

        public async Task<IActionResult> PlayTournament(int tournamentId)
        {
            List<Knockout> matches;
            string uri = $"/Tournament/GetTournamentMatches/?tournamentId={tournamentId}";

            try
            {
                var response = await _APIhelper.GetKnockoutAsync(uri);
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return View();
                }
                matches = response as List<Knockout>;
            }
            catch (Exception)
            {
                return View();
            }

            ViewData["Tournament"] = matches;

            return View();
        }

        /// <summary>
        /// Spectate a tournament button
        /// </summary>
        /// <returns>Returns a view</returns>
        public IActionResult OnClickWatch()
        {
            return View();
        }
    }
}