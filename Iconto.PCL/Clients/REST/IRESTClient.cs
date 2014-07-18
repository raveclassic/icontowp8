using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Clients.REST
{
    public interface IRESTClient
    {
        Task Get(string resource, IEnumerable<KeyValuePair<string, string>> query = null);
        Task Post(string resource, dynamic data = null);
        Task Put(string resource, dynamic data = null);
        Task Delete(string resource, dynamic data = null);

        //Task<dynamic> GetJSON(string resource, List<KeyValuePair<string, string>> query = null);
        //Task<dynamic> PostJSON(string resource, dynamic data = null);
        //Task<dynamic> PutJSON(string resource, dynamic data = null);
        //Task<dynamic> DeleteJSON(string resource, dynamic data = null);

        Task<T> Get<T>(string resource, IEnumerable<KeyValuePair<string, string>> query = null) where T : class;
        Task<T> Post<T>(string resource, dynamic data = null) where T : class;
        Task<T> Put<T>(string resource, dynamic data = null) where T : class;
        Task<T> Delete<T>(string resource, dynamic data = null) where T : class;

        Task<IEnumerable<T>> GetList<T>(string resource, IEnumerable<KeyValuePair<string, string>> query = null) where T : class;
        Task<IEnumerable<T>> PostList<T>(string resource, dynamic data = null) where T : class;
        Task<IEnumerable<T>> PutList<T>(string resource, dynamic data = null) where T : class;
        Task<IEnumerable<T>> DeleteList<T>(string resource, dynamic data = null) where T : class;
    }
}
