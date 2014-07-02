using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.REST.Responses
{
    [DataContract]
    public class CreateOrderResponse
    {
        [DataMember(Name = "form_url")]
        public string FormUrl { get; set; }
    }
}
