using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Exceptions
{
    public class ApiException : Exception
    {
        public long Status { get; set; }

        public ApiException(long status, string msg)
            : base(msg)
        {
            Status = status;
        }
    }
}
