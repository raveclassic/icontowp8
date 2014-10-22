using Iconto.PCL.Exceptions;
using Iconto.PCL.Stores.Settings;
using Iconto.PCL.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Clients.REST
{
    public class RESTClient : IRESTClient
    {
        private readonly ISettingsStore SettingsStore;
        private string Sid { get; set; }

        private const string ICONTO_API_SID = "PHPSESSID";
        private Uri ICONTO_API_URI = new Uri(@"http://api.dev.iconto.net/rest/2.0/");
        //private Uri ICONTO_API_URI = new Uri(@"http://api.iconto.net/rest/2.0/");

        private JSONSerializer Serializer;

        private CookieContainer cookieContainer;
        private HttpClient client;
        private HttpClient Client
        {
            get
            {
                if (client != null) return client;

                cookieContainer = new CookieContainer();
                
                if (!String.IsNullOrEmpty(Sid))
                {
                    cookieContainer.Add(ICONTO_API_URI, new Cookie(ICONTO_API_SID, Sid, "/", "iconto.net"));
                }

                var httpClientHanler = new HttpClientHandler()
                {
                    CookieContainer = cookieContainer
                };

                client = new HttpClient(httpClientHanler);
                client.DefaultRequestHeaders.Add("X-Suppress-HTTP-Code", "1");

                return client;
            }
        }

        public RESTClient(ISettingsStore settingsStore)
        {
            this.SettingsStore = settingsStore;
            this.Sid = SettingsStore.Sid;
            this.Serializer = new JSONSerializer();
        }

        private async Task<string> Request(HttpMethod method, string url, dynamic data = null)
        {
            //var request = new HttpRequestMessage(method, url);
            var request = new HttpRequestMessage(method, String.Format("{0}{1}", ICONTO_API_URI, url));

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

            Debugger.Log(0, "HTTP", String.Join(" ", method.Method, ICONTO_API_URI, HttpUtility.UrlDecode(url), content) + "\n");

            var response = await Client.SendAsync(request);

            var sid = "";
            try
            {
                var responseCookies = cookieContainer.GetCookies(ICONTO_API_URI);
                sid = responseCookies[ICONTO_API_SID].Value;
                SettingsStore.Sid = sid;
            }
            catch (NullReferenceException nre)
            {
            }
            
            
            var body = await response.Content.ReadAsStringAsync();

            Debugger.Log(0, "HTTP", String.Join(" ", method.Method, ICONTO_API_URI, HttpUtility.UrlDecode(url), body, sid) + "\n\n");

            return body;
        }

        public async Task Get(string resource, IEnumerable<KeyValuePair<string, string>> query = null)
        {
            CheckStatus(await Request(HttpMethod.Get, HttpHelper.BuildUrl(resource, query)));
        }
        public async Task Post(string resource, dynamic data = null)
        {
            try
            {
                CheckStatus(await Request(HttpMethod.Get, resource, data));
            }
            catch (ProtocolViolationException pve)
            {
            }
        }
        public async Task Put(string resource, dynamic data = null)
        {
            CheckStatus(await Request(HttpMethod.Get, resource, data));
        }
        public async Task Delete(string resource, dynamic data = null)
        {
            CheckStatus(await Request(HttpMethod.Get, resource, data));
        }

        //public async Task<dynamic> GetJSON(string resource, IEnumerable<KeyValuePair<string, string>> query = null)
        //{
        //    var response = await Request(HttpMethod.Get, HttpHelper.BuildUrl(resource, query));
        //    return GetData<object>(response);
        //}
        //public async Task<dynamic> PostJSON(string resource, dynamic data = null)
        //{
        //    var response = await Request(HttpMethod.Post, resource, data);
        //    return GetData<object>(response);
        //}
        //public async Task<dynamic> PutJSON(string resource, dynamic data = null)
        //{
        //    var response = await Request(HttpMethod.Put, resource, data);
        //    return GetData<object>(response);
        //}
        //public async Task<dynamic> DeleteJSON(string resource, dynamic data = null)
        //{
        //    var response = await Request(HttpMethod.Delete, resource, data);
        //    return GetData<object>(response);
        //}

        public async Task<T> Get<T>(string resource, IEnumerable<KeyValuePair<string, string>> query = null) where T : class
        {
            var response = await Request(HttpMethod.Get, HttpHelper.BuildUrl(resource, query));
            return GetData<T>(response);            
        }
        public async Task<T> Post<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Post, resource, data);
            return GetData<T>(response);
        }
        public async Task<T> Put<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Put, resource, data);
            return GetData<T>(response);
        }
        public async Task<T> Delete<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Delete, resource, data);
            return GetData<T>(response);
        }

        public async Task<IEnumerable<T>> GetList<T>(string resource, IEnumerable<KeyValuePair<string, string>> query = null) where T : class
        {
            var response = await Request(HttpMethod.Get, HttpHelper.BuildUrl(resource, query));
            return GetListData<T>(response);     
        }

        public async Task<IEnumerable<T>> PostList<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Post, resource, data);
            return GetListData<T>(response);
        }

        public async Task<IEnumerable<T>> PutList<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Put, resource, data);
            return GetListData<T>(response);
        }

        public async Task<IEnumerable<T>> DeleteList<T>(string resource, dynamic data = null) where T : class
        {
            var response = await Request(HttpMethod.Delete, resource, data);
            return GetListData<T>(response);
        }

        private void CheckStatus(string response)
        {
            var result = Serializer.Deserialize<CommonResponse<object>>(response);
            if (result.Status != 0)
            {
                throw new ApiException(result.Status, result.Message);
            }
        }

        private T GetData<T>(string response)
        {
            var result = Serializer.Deserialize<CommonResponse<T>>(response);
            if (result.Status != 0)
            {
                throw new ApiException(result.Status, result.Message);
            }
            else
            {
                return result.Data;
            }
        }

        private IEnumerable<T> GetListData<T>(string response)
        {
            var result = Serializer.Deserialize<CommonArrayResponse<IEnumerable<T>>>(response);
            if (result.Status != 0)
            {
                throw new ApiException(result.Status, result.Message);
            }
            else
            {
                return result.Data.Items;
            }
        }
    }
}
