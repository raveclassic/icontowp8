using System;
using iConto.Model;

namespace iConto.Design
{
    public class DesignDataService : IDataService
    {
        //public void GetData(Action<DataItem, Exception> callback)
        //{
        //    // Use this to create design time data

        //    var item = new DataItem("Welcome to MVVM Light [design]");
        //    callback(item, null);
        //}

        public System.Threading.Tasks.Task<EntityType> FindOne<EntityType>(string id)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<EntityType>> FindAll<EntityType>()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<EntityType>> Filter<EntityType>(System.Collections.Generic.Dictionary<string, string> query)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<EntityType>> FindMany<EntityType>(object[] ids)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> GetAsync<ResponseType>(string url)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> PostAsync<ResponseType>(string url, System.Collections.Generic.Dictionary<string, string> data = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> PutAsync<ResponseType>(string url, System.Collections.Generic.Dictionary<string, string> data = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> DeleteAsync<ResponseType>(string url)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> GetAsync<ResponseType>(string resource, System.Collections.Generic.Dictionary<string, string> query = null)
        {
            throw new NotImplementedException();
        }
    }
}