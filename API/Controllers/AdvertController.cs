using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
//using API.Models;
using CommonLibrary;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Controller for Advertisements
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        static int AdCost = 3;
        private UnitOfWork unitOfWork;

        /// <summary>
        /// Constructor for AdvertController
        /// </summary>
        /// <param name="unitOfWork">Repository dependency injection</param>
        public AdvertController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Makes sure that every advert showing on the homepage is not outdated
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckDeadlineDate()
        {
            foreach (var item in unitOfWork.AdvertRepository.Get())
            {
                if (item.DeadlineDateTime < DateTime.Now)
                {
                    unitOfWork.AdvertRepository.Delete(item);
                    unitOfWork.Save();
                }
            }
        }

        /// <summary>
        /// Showing adverts connected to a certain advertiser on their profile page
        /// </summary>
        /// <param name="user">An advertiser</param>
        /// <returns>Returns a list of adverts or an exception</returns>
        [HttpPost]
        [Route("AdFrontPage")]
        public ActionResult<List<Advert>> AdFrontPage(Advertiser user)
        {
            try
            {
                CheckDeadlineDate();
                List<Advert> advertList = new List<Advert>();

                foreach (var item in unitOfWork.AdvertRepository.Get())
                {
                    if (item.UserId == user.UserId)
                        advertList.Add(item);
                }

                return advertList;
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Inserts a new advert into the database
        /// </summary>
        /// <param name="advert">A new advert the advertiser wishes to buy</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("BuyAdvert")]
        public ActionResult<string> BuyAdvert(Advert advert)
        {
            try
            {
                unitOfWork.AdvertRepository.Insert(advert);
                unitOfWork.Save();
                return StatusCode(200, "Ok");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Removes a certain advert from the database
        /// </summary>
        /// <param name="advert">The advert an advertiser wishes to cancel</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("CancelAdvert")]
        public ActionResult<string> CancelAdvert(Advert advert)
        {
            try
            {
                unitOfWork.AdvertRepository.Delete(advert);
                unitOfWork.Save();
                return StatusCode(200, "Ok");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Collects balance for a specific advertiser
        /// </summary>
        /// <param name="user">An advertiser</param>
        /// <returns>Returns an advertisers balance or a statuscode</returns>
        [HttpPost]
        [Route("GetAdvertiserBalance")]
        public ActionResult<User> GetAdvertiserBalance(Advertiser user)
        {
            try
            {
                foreach (var u in unitOfWork.AdvertiserRepository.Get())
                {
                    if (u.UserId == user.UserId)
                    {                        
                        return u;
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
        /// Adds additional balance to an advertisers current balance
        /// </summary>
        /// <param name="user">An advertiser</param>
        /// <param name="balance">The amount the advertiser wishes to add to his/hers balance</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("AddingBalance")]
        public ActionResult<string> AddingBalance(Advertiser user, float balance)
        {
            try
            {
                var result = from item in unitOfWork.AdvertiserRepository.Get()
                             where user.UserId == item.UserId
                             select item;

                foreach (var item in result)
                {
                    item.Balance += balance;
                }

                unitOfWork.Save();
                return StatusCode(200, "Ok");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Removes a certain amount of money from a certain advertiser every time
        /// his/hers ad is clicked on
        /// </summary>
        /// <param name="advert">A specific advert</param>
        /// <returns>Returns a statuscode depending on the results</returns>
        [HttpPost]
        [Route("AdClicked")]
        public ActionResult<string> AdClicked(Advert advert)
        {
            try
            {
                foreach (var item in unitOfWork.AdvertiserRepository.Get())
                {
                    if (advert.UserId == item.UserId)
                    {
                        if (item.Balance < AdCost)
                        {
                            foreach (var tempAdvert in unitOfWork.AdvertRepository.Get())
                            {
                                if (item.UserId == tempAdvert.UserId)
                                {
                                    unitOfWork.AdvertRepository.Delete(tempAdvert);
                                }
                            }
                            break;
                        }
                        item.Balance -= AdCost;
                    }
                }

                unitOfWork.Save();
                return StatusCode(200, "Ok");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        /// <summary>
        /// Picks a random advert from the database to show on the Homepage
        /// </summary>
        /// <returns>Returns a random advert from the database, a statuscode or null depending on the results</returns>
        [HttpPost]
        [Route("ShowAd")]
        public ActionResult<Advert> ShowAd()
        {
            try
            {
                CheckDeadlineDate();
                List<Advert> advertList = new List<Advert>();
                foreach (var item in unitOfWork.AdvertRepository.Get())
                {
                    if (item.BeginDateTime <= DateTime.Now)
                        advertList.Add(item);
                }

                int chosenAd = 0;
                var rand = new Random();
                int adRandom = advertList.Count();

                if (adRandom == 0)
                    return null;
                chosenAd = rand.Next(adRandom);

                return unitOfWork.AdvertRepository.Skip(chosenAd);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
    }
}
