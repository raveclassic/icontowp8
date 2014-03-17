using iConto.Model.Adapter;
using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using iConto.Model.Serializer;
using iConto.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iConto.Model
{
    public class DataService : IDataService
    {
        private RESTAdapter _adapter;

        public DataService()
        {
            _adapter = new RESTAdapter(new JSONSerializer());
        }

        //public void GetData(Action<DataItem, Exception> callback)
        //{
        //    // Use this to connect to the actual data service

        //    var item = new DataItem("Welcome to MVVM Light");
        //    callback(item, null);

        //    _adapter.GetAsync<CommonResponse<Session>>("session").ContinueWith(async sessionTask =>
        //    {
        //        var authData = new Dictionary<string, string>()
        //        {
        //            { "login", "79043373051" },
        //            { "password", "1234" }
        //        };
        //        var response = await _adapter.PostAsync<CommonResponse<object>>("auth", authData);

        //        Console.WriteLine(response);
        //    });
        //}

        public Task<ResponseType> GetAsync<ResponseType>(string url, Dictionary<string, string> query = null)
        {
            return _adapter.GetAsync<ResponseType>(url, query);
        }

        public Task<ResponseType> PostAsync<ResponseType>(string url, Dictionary<string, string> data = null)
        {
            return _adapter.PostAsync<ResponseType>(url, data);
        }

        public Task<ResponseType> PutAsync<ResponseType>(string url, Dictionary<string, string> data = null)
        {
            return _adapter.PutAsync<ResponseType>(url, data);
        }

        public Task<ResponseType> DeleteAsync<ResponseType>(string url)
        {
            return _adapter.DeleteAsync<ResponseType>(url);
        }

        public Task<EntityType> FindOne<EntityType>(string id)
        {
            return _adapter.FindOne<EntityType>(id);
        }

        public Task<List<EntityType>> FindAll<EntityType>()
        {
            return _adapter.FindAll<EntityType>();
        }

        public Task<List<EntityType>> Filter<EntityType>(Dictionary<string, string> query)
        {
            return _adapter.Filter<EntityType>(query);
        }

        public Task<List<EntityType>> FindMany<EntityType>(object[] ids)
        {
            return _adapter.FindMany<EntityType>(ids);
        }


        //public ResponseType> GetAsync<ResponseType>(string url, Dictionary<string, string> query = null)
        //{
        //    return _adapter.GetAsync<ResponseType>(url, query);
        //}

        //public ResponseType> PostAsync<ResponseType>(string url, Dictionary<string, string> data = null)
        //{
        //    return _adapter.PostAsync<ResponseType>(url, data);
        //}

        //public ResponseType> PutAsync<ResponseType>(string url, Dictionary<string, string> data = null)
        //{
        //    return _adapter.PutAsync<ResponseType>(url, data);
        //}

        //public ResponseType> DeleteAsync<ResponseType>(string url)
        //{
        //    return _adapter.DeleteAsync<ResponseType>(url);
        //}
    }
}