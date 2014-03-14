using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using iConto.Model.Serializer;
using iConto.Utility;
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

        private static Uri ICONTO_API_URL = new Uri(@"https://api.iconto.net/rest/2.0");
        private static Dictionary<Type, string> _entityResourceMap = new Dictionary<Type, string>()
        {
            { typeof(Session), "session" },
            { typeof(User), "user" }
        };

        #endregion static

        private ISerializer Serializer { get; set; }

        private string sid;       

        public RESTAdapter(ISerializer serializer)
        {
            Serializer = serializer;
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

        public async Task<List<EntityType>> FindMany<EntityType>(object[] ids)
        {
            if (ids.Length == 0) return new List<EntityType>();

            Dictionary<string, string> query = new Dictionary<string, string>();
            foreach (var id in ids)
            {
                query.Add("ids[]", id.ToString());
            }

            var response = await GetAsync<CommonArrayResponse<EntityType>>(_entityResourceMap[typeof(EntityType)], query);

            return response.Data.Items;
        }

        public async Task<List<EntityType>> Filter<EntityType>(Dictionary<string, string> query)
        {
            var response = await GetAsync<CommonArrayResponse<EntityType>>(_entityResourceMap[typeof(EntityType)], query);

            return response.Data.Items;
        }

        #region privates

        private async Task<ResponseType> Request<ResponseType>(HttpMethod method, string url, Dictionary<string, string> data = null)
        {            
            var client = new HttpClient();

            var request = new HttpRequestMessage(method, url);

            var content = "";
            if (data != null)
            {
                content = JsonConvert.SerializeObject(data);
            } 
            request.Content = new StringContent(content);
            //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content.Headers.Add("Content-Type", "application/json");
            request.Content.Headers.Add("X-Suppress-HTTP-Code", "1");

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            return Serializer.Deserialize<ResponseType>(body);
        }

        public static string BuildUrl(string resource, string id = null, Dictionary<string, string> query = null)
        {
            var url = String.Format("{0}/{1}", ICONTO_API_URL, resource);
            if (id != null)
            {
                url = String.Format("{0}/{1}", url, id);
            }
            var uriBuilder = new UriBuilder(url);
            if (query != null)
            {
                uriBuilder.Query = String.Join("&", query.Select(pair => String.Format("{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value))).OrderBy(s => s).ToArray());
            }

            return uriBuilder.ToString();
        }

        #endregion

        #region shortcut methods

        public Task<ResponseType> GetAsync<ResponseType>(string resource, Dictionary<string, string> query = null)
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
