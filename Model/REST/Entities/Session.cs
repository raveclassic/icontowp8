using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.REST.Entities
{
    [DataContract]
    public class Session
    {
        [DataMember(Name = "sessionId")]
        public string Id { get; set; }
    }
}
