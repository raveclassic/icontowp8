using System;
using iConto.Model;
using System.Collections.Generic;

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

        public System.Threading.Tasks.Task<List<EntityType>> FindAll<EntityType>()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<List<EntityType>> Filter<EntityType>(List<KeyValuePair<string, string>> query)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<List<EntityType>> FindMany<EntityType>(long[] ids)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> GetAsync<ResponseType>(string resource, List<KeyValuePair<string, string>> query = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> PostAsync<ResponseType>(string resource, Dictionary<string, object> data = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> PutAsync<ResponseType>(string resource, Dictionary<string, object> data = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<ResponseType> DeleteAsync<ResponseType>(string resource)
        {
            throw new NotImplementedException();
        }
    }
}