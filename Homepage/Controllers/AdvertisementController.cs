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
    /// Controller for Advertisements
    /// </summary>
    public class AdvertisementController : Controller
    {
        private readonly APIhelper _APIhelper;
        private readonly SessionHelper _SessionHelper;
        private readonly Adhelper _Adhelper;

        /// <summary>
        /// Constructor for AdvertisementController
        /// The parameters are dependency injections
        /// </summary>
        public AdvertisementController(APIhelper aPIhelper, SessionHelper SessionHelper, Adhelper Adhelper)
        {
            _APIhelper = aPIhelper;
            _SessionHelper = SessionHelper;
            _Adhelper = Adhelper;
        }
        private float balance = 0;
        List<Advert> newList = new List<Advert>();

        /// <summary>
        /// Profile page for an Advertiser
        /// Shows all active ads and current balance
        /// </summary>
        /// <returns>Returns different views depending on the situation</returns>
        public async Task<IActionResult> Index()
        {
            var user = _SessionHelper.GetSessionUser(HttpContext);
            List<Advert> newList;
            float balance;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }           
            try
            {
                //Hämtar all reklam som tillhör kunden
                string uri = "/Advert/AdFrontPage/";
                var response = await _APIhelper.PostAdvertAsync(uri, user);
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return RedirectToAction("Index", "Home");
                }
                newList = response as List<Advert>;

                //Hämtar kundens saldo
                uri = "/Advert/GetAdvertiserBalance/";
                response = await _APIhelper.PostUserAsync(uri, user);
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return RedirectToAction("Index", "Home");
                }
                balance = (response as Advertiser).Balance;
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["User"] = user;
            ViewData["ActiveAds"] = newList;
            ViewData["Balance"] = balance + " SEK";
            return View(new BaseViewModel() { Advert = await _Adhelper.ShowAd() });
        }

        /// <summary>
        /// Cancel an advertisement
        /// </summary>
        /// <param name="advert">The specific advert an advertiser wishes to cancel</param>
        /// <param name="productImage">Product image to the specific advert</param>
        /// <returns>Returns different views depending on the situation</returns>
        public async Task<IActionResult> Cancel([Bind("advertId,AdvertName,Sponsoring,BeginDateTime,DeadlineDateTime,Link,UserId")] Advert advert, string productImage)
        {
            byte[] productImageByte = Encoding.UTF8.GetBytes(productImage);
            advert.productImage = productImageByte;

            User newUser = _SessionHelper.GetSessionUser(HttpContext);
            string uri = "/Advert/CancelAdvert/";
            if (newUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var response = await _APIhelper.PostAdvertAsync(uri, advert);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }       
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Redirects user to Buy View
        /// </summary>
        /// <param name="advert">The advert that a certain advertiser wishes to buy</param>
        /// <returns>Returns different views depending on the situation</returns>
        public async Task<IActionResult> Buy(Advert advert=null)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
                return RedirectToAction("Index", "Home");
            AdvertViewModel advertViewModel = new AdvertViewModel() { User = user, Advert = await _Adhelper.ShowAd(), advert = advert };
            return View(advertViewModel);
        }

        /// <summary>
        /// Buy an advert
        /// </summary>
        /// <param name="advert">An advert</param>
        /// <param name="productImage">The advert's product image</param>
        /// <returns>Redirects to different view depending on the situation</returns>
        public async Task<IActionResult> BuyAdvert([Bind("advertId,AdvertName,Sponsoring,BeginDateTime,DeadlineDateTime,Link,UserId")] Advert advert, IFormFile productImage)
        {
            if (productImage == null)
            {
                return RedirectToAction("Buy", new { advert = advert });
            }
            //Convert picture to bytearray
            byte[] bytes;
            using (BinaryReader reader = new BinaryReader(productImage.OpenReadStream()))
            {
                bytes = reader.ReadBytes(Convert.ToInt32(productImage.Length));
            }

            advert.productImage = bytes;

            if (ModelState.IsValid)
            {
                string uri = "/Advert/BuyAdvert/";
                try
                {
                    var response = await _APIhelper.PostAdvertAsync(uri, advert);
                }
                catch (Exception)
                {
                    return RedirectToAction("Buy", new { advert = advert });
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Buy", new { advert = advert });
        }

        /// <summary>
        /// Adds more balance to advertiser's current balance
        /// </summary>
        /// <param name="errorMessage">Message showing if the action was successful or not</param>
        /// <returns>Return a different view depending on the situation</returns>
        public async Task<IActionResult> AddBalance(string errorMessage=null)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            float balance;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                //Hämtar nuvarande saldo
                string uri = "/Advert/GetAdvertiserBalance/";
                var response = await _APIhelper.PostUserAsync(uri, user);
                if (response.GetType() == typeof(Dictionary<string, string>))
                {
                    return RedirectToAction("Index", "Home");
                }
                balance = (response as Advertiser).Balance;
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }            
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }
            ViewData["currentBalance"] = balance;
            return View(new BaseViewModel() { Advert = await _Adhelper.ShowAd() });
        }

        /// <summary>
        /// Checks the input on adding balance to make sure the advertiser
        /// doesn't put in negative numbers and so on.
        /// </summary>
        /// <returns>Redirects to different view depending on the situation</returns>
        public async Task<IActionResult> AddBalanceButton()
        {
            float balance;
            string addedBalance = Request.Form["inputbox"];

            if(addedBalance.Contains("-"))
            {
                return RedirectToAction("AddBalance", new { errorMessage = "Negative inputs are not allowed" });
            }
            else if(float.TryParse(addedBalance, out balance)==false)
            {
                return RedirectToAction("AddBalance", new { errorMessage = "Only numbers can be written, try again" });
            }

            float.TryParse(addedBalance, out balance);
            User newUser = _SessionHelper.GetSessionUser(HttpContext);

            if (newUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                string uri = "/Advert/AddingBalance/?balance=";
                var response = await _APIhelper.PostAdvertAsync(string.Concat(uri, balance), newUser);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Takes in an advert and sends it to the API to draw money from the advertiser
        /// who owns the ad and then redirects the clicker to a certain website
        /// </summary>
        /// <param name="advert">The ad that has been clicked on</param>
        /// <param name="productImage">The advert's product image</param>
        /// <returns>Redirects to another website</returns>
        public async Task<IActionResult> AdClicked([Bind("advertId,AdvertName,Sponsoring,BeginDateTime,DeadlineDateTime,Link,UserId")] Advert advert, string productImage)
        {
            byte[] productImageByte = Encoding.UTF8.GetBytes(productImage);
            advert.productImage = productImageByte;
            try
            {
                string uri = "/Advert/AdClicked/";
                var response = await _APIhelper.PostAdvertAsync(uri, advert);
            }
            catch (Exception)
            {

                throw;
            }
            return Redirect(advert.Link);
        }
    }
}