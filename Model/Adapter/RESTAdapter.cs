using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using iConto.Model.Serializer;
using iConto.Services.Settings;
using iConto.Utility;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.Adapter
{
    public class RESTAdapter : IAdapter, IHTTP
    {
        #region static

        private const string ICONTO_API_SID = "ICONTO_API_SID";
        private const string PHPSESSID = "PHPSESSID";

        private static Uri ICONTO_API_URL = new Uri(@"https://api.dev.iconto.net/rest/2.0/");
        private static Dictionary<Type, string> _entityResourceMap = new Dictionary<Type, string>()
        {
            { typeof(Session), "session" },
            { typeof(User), "user" },
            { typeof(Card), "card" },
            { typeof(Bank), "bank" }
        };

        #endregion static

        private ISerializer Serializer { get; set; }

        private ISettingsService SettingsService { get; set; }

        private CookieContainer cookieContainer;
        private HttpClient client;
        public HttpClient Client
        {
            get
            {
                if (client != null) return client;

                cookieContainer = new CookieContainer();
                var sid = SettingsService.Get(ICONTO_API_SID);
                if (sid != null)
                {
                    cookieContainer.Add(ICONTO_API_URL, new Cookie(PHPSESSID, (string)sid, "/", "iconto.net"));
                }

                var httpClientHanler = new HttpClientHandler()
                {
                    CookieContainer = cookieContainer
                };

                client = new HttpClient(httpClientHanler)
                {
                    BaseAddress = ICONTO_API_URL
                };
                client.DefaultRequestHeaders.Add("X-Suppress-HTTP-Code", "1");

                return client;
            }
        }

        public RESTAdapter(ISerializer serializer)
        {
            Serializer = serializer;
            SettingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
        }

        public async Task<EntityType> FindOne<EntityType>(string id)
        {
            var resource = String.Format("{0}/{1}", _entityResourceMap[typeof(EntityType)], id);

            var response = await GetAsync<CommonResponse<EntityType>>(resource);

            return response.Data;
        }

        public async Task<List<EntityType>> FindAll<EntityType>()
        {
            var resource = _entityResourceMap[typeof(EntityType)];

            var response = await GetAsync<CommonArrayResponse<EntityType>>(resource);

            return response.Data.Items;
        }

        public async Task<List<EntityType>> FindMany<EntityType>(long[] ids)
        {
            if (ids.Length == 0) return new List<EntityType>();

            List<KeyValuePair<string, string>> query = new List<KeyValuePair<string, string>>();
            foreach (var id in ids)
            {
                query.Add(new KeyValuePair<string, string>("ids[]", id.ToString()));
            }
            var response = await GetAsync<CommonArrayResponse<EntityType>>(_entityResourceMap[typeof(EntityType)], query);

            return response.Data.Items;
        }

        public async Task<List<EntityType>> Filter<EntityType>(List<KeyValuePair<string, string>> query)
        {
            var response = await GetAsync<CommonArrayResponse<EntityType>>(_entityResourceMap[typeof(EntityType)], query);

            return response.Data.Items;
        }

        #region privates

        private async Task<ResponseType> Request<ResponseType>(HttpMethod method, string url, Dictionary<string, string> data = null)
        {            
            var request = new HttpRequestMessage(method, url);

            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data));
                if (method != HttpMethod.Get)
                {
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                }
            }

            var response = await Client.SendAsync(request);
            var sid = cookieContainer.GetCookies(ICONTO_API_URL)[PHPSESSID].Value;
            SettingsService.Set(ICONTO_API_SID, sid);
            var body = await response.Content.ReadAsStringAsync();

            return Serializer.Deserialize<ResponseType>(body);
        }

        public static string BuildUrl(string resource, string id = null, List<KeyValuePair<string, string>> query = null)
        {
            var url = resource;

            if (id != null)
            {
                url = String.Join("/", url, id);
            }
            
            if (query != null)
            {
                var queryString = String.Join("&", query.Select(pair => String.Format("{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value))).OrderBy(s => s).ToArray());
                url = String.Join("?", url, queryString);
            }

            return url;
        }

        #endregion

        #region shortcut methods

        public Task<ResponseType> GetAsync<ResponseType>(List<KeyValuePair<string, string>> query = null)
        {
            return GetAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], query);
        }
        public Task<ResponseType> PostAsync<ResponseType>(Dictionary<string, string> data = null)
        {
            return PostAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], data);
        }
        public Task<ResponseType> PutAsync<ResponseType>(Dictionary<string, string> data = null)
        {
            return PutAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], data);
        }
        public Task<ResponseType> DeleteAsync<ResponseType>()
        {
            return DeleteAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)]);
        }

        public Task<ResponseType> GetAsync<ResponseType>(string resource, List<KeyValuePair<string, string>> query = null)
        {
            return Request<ResponseType>(HttpMethod.Get, BuildUrl(resource, null, query));
        }
        public Task<ResponseType> PostAsync<ResponseType>(string resource, Dictionary<string, string> data = null)
        {
            return Request<ResponseType>(HttpMethod.Post, BuildUrl(resource, null, null), data);
        }
        public Task<ResponseType> PutAsync<ResponseType>(string resource, Dictionary<string, string> data = null)
        {
            return Request<ResponseType>(HttpMethod.Put, BuildUrl(resource, null, null), data);
        }
        public Task<ResponseType> DeleteAsync<ResponseType>(string resource)
        {
            return Request<ResponseType>(HttpMethod.Delete, BuildUrl(resource, null, null));
        }

        #endregion
    }
}
