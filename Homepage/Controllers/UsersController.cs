using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using Homepage.Service;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using CommonLibrary.ViewModels;
using CommonLibrary.Enums;

namespace Homepage.Controllers
{
    public class UsersController : Controller
    {
        private readonly APIhelper _APIhelper;
        private readonly SessionHelper _SessionHelper;
        private readonly Adhelper _Adhelper;

        /// <summary>
        /// Controller for Users. takes APIhelper, SessioinHelper and Adhelper as DependencyInjection
        /// </summary>
        /// <param name="APIhelper">Communicates with API</param>
        /// <param name="SessionHelper">Deserialises users from session to correct subtype</param>
        /// <param name="Adhelper">Helps to show ads</param>
        public UsersController(APIhelper APIhelper, SessionHelper SessionHelper, Adhelper Adhelper)
        {
            _APIhelper = APIhelper;
            _SessionHelper = SessionHelper;
            _Adhelper = Adhelper;
        }
        /// <summary>
        /// This method shows the loginView if the user is not loged in and the profilepage if
        /// the user is loged in.
        /// </summary>
        /// <returns>Either login view or profilepage</returns>
        public async Task<IActionResult> Index()
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            if (user == null)
            {
                return View();
            }
            return RedirectToAction("ProfilePage", new { id = user.UserId });
        }
        /// <summary>
        /// This method is called by Users/Index view (loginPage) and takes a user and sends it to
        /// the API for authentication. If user is authenticated it's redirected to profilepage. Else 
        /// an errormessage is displayed.
        /// </summary>
        /// <param name="user">A user (baseclass)</param>
        /// <returns></returns>
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get user
                    string uri = "/User/Authenticate";
                    var response = await _APIhelper.PostUserAsync(uri, user);
                    if (response.GetType() == typeof(Dictionary<string, string>))
                    {
                        Dictionary<string, string> dict = response as Dictionary<string, string>;
                        ModelState.AddModelError("User.UserName", dict["Content"]);
                        return View("Index");
                    }
                    var newUser = response as User;
                    _SessionHelper.SetSessionUser(HttpContext, newUser);
                    return RedirectToAction("ProfilePage", new { id = newUser.UserId });
                }
                catch (Exception ex)
                {
                    ViewBag.msg = "Woops what happend?" + " " + ex.Message;
                    return View("Index");
                }
            }
            else
            {
                return View("Index", new BaseViewModel() { User = user, Advert = await _Adhelper.ShowAd() });
            }
        }
        /// <summary>
        /// This method is called when the user logs out. It "forgets" the usersession.
        /// </summary>
        /// <returns>redirect to User/Index (loginView)</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Index");
        }
        /// <summary>
        /// This method is called when the user clicks on profilepage or clicks proflilepage in searchview.
        /// if the userId matches with the userId from session user is redirected to its own profilepage. Else if
        /// usesrId does not match userId from session the matching user is retrieved from the API and that users
        /// profilepage is shown.
        /// </summary>
        /// <param name="id">userId</param>
        /// <returns></returns>
        public async Task<IActionResult> ProfilePage(int id)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            User targetUser;
            try
            {
                string uri = "/User/ById/";
                var response = await _APIhelper.GetUserAsync(string.Concat(uri, id.ToString()));
                if (response.GetType() == typeof(Dictionary<string,string>))
                {
                    return RedirectToAction("Index", "Home");
                }
                targetUser = response as User;
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            if (user != null && user.UserId == id)
            {
                ViewBag.Search = false;
            }
            else
            {
                ViewBag.Search = true;
                HttpContext.Session.SetString("BackFromProfile", "True");
            }
            return View(new ProfilePageViewModel()
            {
                User = targetUser,
                UserTypes = new UserType(),
                Advert = await _Adhelper.ShowAd()
            });
        }

        public async Task<IActionResult> ChangeUserSettings(int UserTypeNum, int userId)
        {
            string userTypeString = ((UserType)UserTypeNum).ToString("F");
            if (userTypeString != null)
            {
                try
                {
                    string uri = "/User/ChangeUserType";
                    var response = await _APIhelper.PostUserAsync(uri, new Dictionary<string, string>() { { "userTypeString", userTypeString }, { "userId", userId.ToString() } });
                    if (response.GetType() == typeof(Dictionary<string, string>))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    var changedUser = response as User;
                    return View("ProfilePage", new ProfilePageViewModel() { User = changedUser, Advert = await _Adhelper.ShowAd() });
                }
                catch (Exception)
                {
                    RedirectToAction("Index", "Home");
                }
            }           
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Tar emot en godtycklig söksträng och skickar den till api't som försöker matcha den mot user.userName i databasen.
        /// Den anropas också om man trycker på "Back" när man besöker någons profil. Då hämtar den istället userList från session.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="back"></param>
        /// <returns></returns>
        public async Task<IActionResult> SearchForUser(string searchString)
        {
            User user = _SessionHelper.GetSessionUser(HttpContext);
            string SearchString = _SessionHelper.GetSearchString(HttpContext);
            string BackFromProfile = _SessionHelper.GetBackFromProfile(HttpContext);

            if (BackFromProfile == "True")
            {
                searchString = SearchString;
                HttpContext.Session.SetString("BackFromProfile", "False");
            }
            if (searchString == null)
            {
                searchString = "getallusers";
            }
            try
            {
                string uri = "/User/SearchUser/";
                var response = await _APIhelper.GetUserAsync(string.Concat(uri, searchString));
                if (response.GetType() == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string> dict = response as Dictionary<string, string>;
                    ViewBag.UsersList = null;
                    ViewBag.msg = dict["Content"];
                    return View(new ProfilePageViewModel() { User = user, Advert = await _Adhelper.ShowAd() });
                }
                var newUserList = response as List<User>;
                HttpContext.Session.SetString("SearchString", searchString);
                ViewBag.UsersList = newUserList;
                return View(new ProfilePageViewModel() { User = user, Advert = await _Adhelper.ShowAd() });
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName, Password")] Player player)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uri = "/User/Register";
                    var response = await _APIhelper.PostUserAsync(uri, player);
                    if (response.GetType() == typeof(Dictionary<string, string>))
                    {
                        Dictionary<string, string> dict = response as Dictionary<string, string>;
                        if (dict["StatusCode"] == "400")
                        {
                            ModelState.AddModelError("UserName", dict["Content"]);
                        }
                        else
                        {
                            ViewBag.msg = dict["Content"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.msg = "Woops what happend?" + " " + ex.Message;
                }
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                try
                {
                    string uri = "/User/Delete/";
                    var response = await _APIhelper.DeleteUserAsync(string.Concat(uri, id.ToString()));
                    Dictionary<string, string> dict = response as Dictionary<string, string>;
                    if (dict["StatusCode"] == "500")
                    {
                        RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("SearchForUser");
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SaveEdit(User user)
        {
            User oldUser = _SessionHelper.GetSessionUser(HttpContext);
            ViewBag.Edit = false;
            if (ModelState.IsValid)
            {
                try
                {
                    string uri = "/User/Edit";
                    var response = await _APIhelper.PostUserAsync(uri, user);
                    if (response.GetType() == typeof(Dictionary<string, string>))
                    {
                        Dictionary<string, string> dict = response as Dictionary<string, string>;
                        ModelState.AddModelError("userName", dict["Content"]);
                        ViewBag.Edit = true;
                        oldUser.UserName = user.UserName;
                        return View("ProfilePage", new ProfilePageViewModel { User = oldUser, Advert = await _Adhelper.ShowAd() });
                    }
                    var changedUser = response as User;
                    _SessionHelper.SetSessionUser(HttpContext, changedUser);
                    return View("ProfilePage", new ProfilePageViewModel() { User = changedUser, Advert = await _Adhelper.ShowAd() });
                }
                catch (Exception)
                {
                    ViewBag.msg = "Woops what happened?";
                }
            }
            return View("Index");
        }
    }
}
