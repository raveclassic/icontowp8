using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Utility
{
    public class HttpHelper
    {
        public static string BuildUrl(string resource, IEnumerable<KeyValuePair<string, string>> query = null)
        {
            var url = resource;

            if (query != null)
            {
                var queryString = String.Join("&", query.Select(pair => String.Format("{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value))).OrderBy(s => s).ToArray());
                url = String.Join("?", url, queryString);
            }

            return url;
        }
    }
}
