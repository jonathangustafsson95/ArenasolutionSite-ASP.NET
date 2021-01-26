using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.ViewModels;
using Homepage.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Homepage.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly APIhelper _APIhelper;
        private readonly SessionHelper _SessionHelper;
        private readonly Adhelper _Adhelper;
        public LeaguesController(APIhelper APIhelper, SessionHelper SessionHelper, Adhelper Adhelper)
        {
            _APIhelper = APIhelper;
            _SessionHelper = SessionHelper;
            _Adhelper = Adhelper;
        }
        public async Task<IActionResult> Index()
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            int id = 0;
            string uri = "/League/GetLeagues/";
            var response = await _APIhelper.GetLeagueAsync(string.Concat(uri, id.ToString()));
            if (response.GetType() == typeof(Dictionary<string,string>))
            {
                ViewBag.Error = (response as Dictionary<string, string>)["Content"];
                return View(new LeagueViewModel() { Advert = await _Adhelper.ShowAd(), User = user });
            }
            var leaguesList = response as List<League>;
            ViewBag.Leagues = leaguesList;
            if (leaguesList.Count() == 0)
            {
                ViewBag.Leagues = null;
                ViewBag.Error = "No leagues available";
            }
            return View(new LeagueViewModel() { Advert = await _Adhelper.ShowAd() , User = user});
        }

        public async Task<IActionResult> CreateLeague(League league)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (ModelState.IsValid)
            {
                league.LeagueOwnerId = user.UserId;
                string uri = "/League/CreateLeague";
                var response = await _APIhelper.PostUserAsync(uri, league);
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    // This does not work atm because of partial view
                    var dict = response as Dictionary<string, string>;
                    ModelState.AddModelError("LeagueName", dict["Content"]);
                }
                var updatedUser = response as User;
                _SessionHelper.SetSessionUser(HttpContext, updatedUser);
            }
            return RedirectToAction("Index", "Users", new { area = "" });
        }
        public async Task<IActionResult> EditLeague(League league)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (ModelState.IsValid)
            {
                string uri = "/League/EditLeague";
                var response = await _APIhelper.PostUserAsync(uri, league);
                if (response.GetType() == typeof(Dictionary<string, string>))
                {
                    // This does not work atm because of partial view
                    var dict = response as Dictionary<string, string>;
                    ModelState.AddModelError("LeagueName", dict["Content"]);
                }
                var updatedUser = response as User;
                _SessionHelper.SetSessionUser(HttpContext, updatedUser);
            }
            return RedirectToAction("Index", "Users", new { area = ""});
        }
        public async Task<IActionResult> ShowLeague(string id)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            LeagueViewModel leagueViewModel = new LeagueViewModel();

            try
            {
                // Get League
                string uri = "/League/GetLeague/";
                var response = await _APIhelper.GetLeagueAsync(string.Concat(uri, id));
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    var dict = response as Dictionary<string, string>;
                    ViewBag.Error = dict["Content"];
                }
                leagueViewModel.League = response as League;
                leagueViewModel.User = user;
                leagueViewModel.Advert = await _Adhelper.ShowAd();
                return View("ShowLeague", leagueViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ShowLeague", leagueViewModel);
            }
        }
        public async Task<IActionResult> Apply(string id)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            try
            {
                string uri = "/League/ApplyForMembership";
                var response = await _APIhelper.PostUserAsync(uri, new Dictionary<string, string>() { { "userId", user.UserId.ToString() }, { "leagueId", id } });
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return RedirectToAction("ShowLeague", new { id });
                }
                var updatedUser = response as User;
                _SessionHelper.SetSessionUser(HttpContext, updatedUser);
                return RedirectToAction("ShowLeague", new { id });                
            }
            catch (Exception)
            {
                return RedirectToAction("ShowLeague", new { id });
            }
        }
        public async Task<IActionResult> AcceptMember(int userId, string leagueId)
        {
            string id = leagueId;
            try
            {
                string uri = "/League/AcceptMember";
                var response = await _APIhelper.PostLeagueAsync(uri, new Dictionary<string, string>() { { "userId", userId.ToString() }, { "leagueId", leagueId } });
                return RedirectToAction("ShowLeague", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> DeleteMember(int userId, string leagueId)
        {
            string id = leagueId;
            User user = _SessionHelper.GetSessionUser(HttpContext);
            try
            {
                string uri = "/League/DeleteMember";
                var response = await _APIhelper.PostUserAsync(uri, new Dictionary<string, string>() { { "userId", userId.ToString() }, { "leagueId", leagueId } });
                if (response.GetType() == typeof(string))
                {
                    if (user.GetType() == typeof(Player))
                    {
                        return RedirectToAction("Index", "Users", new { area = "" });
                    }
                    return RedirectToAction("ShowLeague", new { id });
                }                
                if (user.GetType() == typeof(Player))
                {
                    _SessionHelper.SetSessionUser(HttpContext, response as Player);
                    return RedirectToAction("Index", "Users", new { area = "" });
                }
                return RedirectToAction("ShowLeague", new { id });

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> Delete(string id)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            try
            {
                string uri = string.Concat("/League/DeleteLeague/", id);
                var response = await _APIhelper.DeleteLeagueAsync(uri);
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return RedirectToAction("Index");
                }
                var updatedUser = response as User;
                _SessionHelper.SetSessionUser(HttpContext, updatedUser);
                if (user.GetType() == typeof(LeagueOwner))
                {
                    return RedirectToAction("Index", "Users", new { area = "" });
                }
                return RedirectToAction("Index");    
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}