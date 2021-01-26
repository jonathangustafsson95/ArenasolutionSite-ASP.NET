using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Homepage.Service
{
    public class SessionHelper
    {
        /// <summary>
        /// Används för att hämta en inloggad user från session. Kollar först om session finns.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>User(subklass)</returns>
        public User GetSessionUser(HttpContext httpContext)
        {
            bool SessionExist = httpContext.Session.TryGetValue("UserSession", out byte[] vs);
            if (!SessionExist)
                return null;
            string sessionString = httpContext.Session.GetString("UserSession");

            var newUser = JsonConvert.DeserializeObject<User>(sessionString, new UserObjectConverter());
            return newUser;
        }
        /// <summary>
        /// Används för att lägga in en user i session.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="user"></param>
        public void SetSessionUser(HttpContext httpContext, User user)
        {
            httpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));
        }
        public string GetSearchString(HttpContext httpContext)
        {
            if (!httpContext.Session.TryGetValue("SearchString", out _))
                return null;
            return httpContext.Session.GetString("SearchString");
        }
        public string GetBackFromProfile(HttpContext httpContext)
        {
            if (!httpContext.Session.TryGetValue("BackFromProfile", out _))
                return null;
            return httpContext.Session.GetString("BackFromProfile");
        }
    }
}
