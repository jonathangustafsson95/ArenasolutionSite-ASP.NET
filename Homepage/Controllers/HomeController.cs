using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Homepage.Models;
using CommonLibrary;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Homepage.Service;
using System.Net.Http;
using System.Text;
using CommonLibrary.ViewModels;

namespace Homepage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIhelper _ApiHelper;
        private readonly Adhelper _Adhelper;


        public HomeController(ILogger<HomeController> logger, APIhelper ApiHelper, Adhelper Adhelper)
        {
            _logger = logger;
            _ApiHelper = ApiHelper;
            ViewBag.UserConfirmed = false;
            _Adhelper = Adhelper;
        }

        public async Task<IActionResult> Index()
        {
            bool SessionExist = HttpContext.Session.TryGetValue("UserSession", out byte[] vs);
            try
            {
                ViewBag.Tournaments = await GetActiveTournaments();
            }
            catch (Exception)
            {
                ViewBag.msg = "Probably can't connect to the API!";
            }
            if (!SessionExist)
            {
                return View(new BaseViewModel() { Advert = await _Adhelper.ShowAd() });
            }
            else
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserSession"));
                return View(new BaseViewModel() { User = user, Advert = await _Adhelper.ShowAd()});
            }
        }

        public async Task<IActionResult> Privacy()
        {
            bool SessionExist = HttpContext.Session.TryGetValue("UserSession", out byte[] vs);

            if (!SessionExist)
            {
                return View(new BaseViewModel() { Advert = await _Adhelper.ShowAd() });
            }
            else
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserSession"));
                return View(new BaseViewModel() { User = user, Advert = await _Adhelper.ShowAd() });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<Tournament>> GetActiveTournaments(League league = null)
        {
            List<Tournament> newList = new List<Tournament>();
            try
            {
                string uri = "/Tournament/GetActiveTournaments/";
                var response = await _ApiHelper.GetTournamentAsync(uri);
                if (response.GetType() == typeof(List<Tournament>))
                {
                    newList = response as List<Tournament>;
                }
                if (newList.Count > 5)
                {
                    newList.RemoveRange(0, newList.Count - 5);
                }
                newList.Reverse();
                return newList;
            }
            catch (Exception)
            {
                return newList;
            }
        }
    }
}
