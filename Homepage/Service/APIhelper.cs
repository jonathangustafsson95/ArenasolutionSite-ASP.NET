using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Homepage.Service
{
    public class APIhelper
    {
        private static HttpClient ApiClient;
        public string BaseUri { get; set; }
        public APIhelper()
        {
            BaseUri = "https://localhost:44377/api";
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<object> PostUserAsync(string uri, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.PostAsync(uri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<User>(content, new UserObjectConverter());
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetUserAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<User>(content, new UserObjectConverter());
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<User>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> DeleteUserAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.DeleteAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetLeagueAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<League>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<League>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> PostLeagueAsync(string uri, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.PostAsync(uri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<League>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> DeleteLeagueAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.DeleteAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<User>(content, new UserObjectConverter());
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetAdvertAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<Advert>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<Advert>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> PostAdvertAsync(string uri, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.PostAsync(uri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<Advert>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<Advert>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetTournamentAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<Tournament>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<Tournament>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetTournamentPlayerAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<TournamentPlayer>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<TournamentPlayer>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> PostTournamentAsync(string uri, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.PostAsync(uri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<Tournament>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<Tournament>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }
        public async Task<object> GetKnockoutAsync(string uri)
        {
            uri = string.Concat(BaseUri, uri);
            using HttpResponseMessage response = await ApiClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<Knockout>(content);
            }
            else if (token is JArray)
            {
                return JsonConvert.DeserializeObject<List<Knockout>>(content);
            }
            Dictionary<string, string> dict = new Dictionary<string, string> { { "StatusCode", response.StatusCode.ToString() }, { "Content", content } };
            return dict;
        }  
    }
}
