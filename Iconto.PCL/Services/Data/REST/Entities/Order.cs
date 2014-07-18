using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Data.REST.Entities
{
    [DataContract]
    public class Order
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
        
        [DataMember(Name = "redirect_url")]
        public string RedirectUrl { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "type")]
        public byte Type { get; set; }

        [DataMember(Name = "verification_type", IsRequired = false)]
        public string VerificationType { get; set; }
    }
}
