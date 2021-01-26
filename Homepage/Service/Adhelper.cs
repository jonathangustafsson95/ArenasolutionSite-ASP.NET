using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace Homepage.Service
{
    public class Adhelper
    {
        private APIhelper _APIhelper = new APIhelper();
        public async Task<Advert> ShowAd(string sponsor="")
        {
            Advert newAdvert;
            try
            {
                string uri = "/Advert/ShowAd/";
                var response = await _APIhelper.PostAdvertAsync(uri, sponsor);
                if (response.GetType() == typeof(string))
                {
                    newAdvert = new Advert() { AdvertName = null };
                }
                newAdvert = response as Advert;
            }
            catch (Exception)
            {
                newAdvert = new Advert() { AdvertName = null };
            }
            return newAdvert;
        }
    }
}
