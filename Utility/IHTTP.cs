using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Utility
{
    public interface IHTTP
    {
        Task<ResponseType> GetAsync<ResponseType>(string resource, Dictionary<string, string> query = null);
        Task<ResponseType> PostAsync<ResponseType>(string resource, Dictionary<string, string> data = null);
        Task<ResponseType> PutAsync<ResponseType>(string resource, Dictionary<string, string> data = null);
        Task<ResponseType> DeleteAsync<ResponseType>(string resource);

        //ResponseType Get<ResponseType>(string resource, Dictionary<string, string> query = null);
        //ResponseType Post<ResponseType>(string resource, Dictionary<string, string> data = null);
        //ResponseType Put<ResponseType>(string resource, Dictionary<string, string> data = null);
        //ResponseType Delete<ResponseType>(string resource);
    }
}
