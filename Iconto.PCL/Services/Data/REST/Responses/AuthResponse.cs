using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Data.REST.Responses
{
    [DataContract]
    public class AuthResponse
    {
        [DataMember(Name = "userId")]
        public long UserId { get; set; }
    }
}
