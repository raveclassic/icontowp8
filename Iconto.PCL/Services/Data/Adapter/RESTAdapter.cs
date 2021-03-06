﻿using Iconto.PCL.Services.Data.REST.Entities;
using Iconto.PCL.Services.Data.REST.Responses;
using Iconto.PCL.Services.Settings;
using Iconto.PCL.Utility;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Data.Serializer
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
            { typeof(Bank), "bank" },
            { typeof(REST.Entities.Wallet), "wallet" }
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

        private async Task<ResponseType> Request<ResponseType>(HttpMethod method, string url, Dictionary<string, object> data = null)
        {            
            var request = new HttpRequestMessage(method, url);

            var content = "";

            if (data != null)
            {
                content = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(content);
                if (method != HttpMethod.Get)
                {
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                }
            }

            Debugger.Log(0, "HTTP", String.Join(" ", method.Method, ICONTO_API_URL, HttpUtility.UrlDecode(url), content) + "\n");
            

            var response = await Client.SendAsync(request);
            var sid = cookieContainer.GetCookies(ICONTO_API_URL)[PHPSESSID].Value;
            SettingsService.Set(ICONTO_API_SID, sid);
            var body = await response.Content.ReadAsStringAsync();

            Debugger.Log(0, "HTTP", String.Join(" ", method.Method, ICONTO_API_URL, HttpUtility.UrlDecode(url), body, sid) + "\n\n");
            return Serializer.Deserialize<ResponseType>(body);
        }

        #endregion

        #region shortcut methods

        public Task<ResponseType> GetAsync<ResponseType>(List<KeyValuePair<string, string>> query = null)
        {
            return GetAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], query);
        }
        public Task<ResponseType> PostAsync<ResponseType>(Dictionary<string, object> data = null)
        {
            return PostAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], data);
        }
        public Task<ResponseType> PutAsync<ResponseType>(Dictionary<string, object> data = null)
        {
            return PutAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)], data);
        }
        public Task<ResponseType> DeleteAsync<ResponseType>()
        {
            return DeleteAsync<ResponseType>(_entityResourceMap[typeof(ResponseType)]);
        }

        public Task<ResponseType> GetAsync<ResponseType>(string resource, List<KeyValuePair<string, string>> query = null)
        {
            return Request<ResponseType>(HttpMethod.Get, HttpHelper.BuildUrl(resource, query));
        }
        public Task<ResponseType> PostAsync<ResponseType>(string resource, Dictionary<string, object> data = null)
        {
            return Request<ResponseType>(HttpMethod.Post, HttpHelper.BuildUrl(resource, null), data);
        }
        public Task<ResponseType> PutAsync<ResponseType>(string resource, Dictionary<string, object> data = null)
        {
            return Request<ResponseType>(HttpMethod.Put, HttpHelper.BuildUrl(resource, null), data);
        }
        public Task<ResponseType> DeleteAsync<ResponseType>(string resource)
        {
            return Request<ResponseType>(HttpMethod.Delete, HttpHelper.BuildUrl(resource, null));
        }

        #endregion
    }
}
