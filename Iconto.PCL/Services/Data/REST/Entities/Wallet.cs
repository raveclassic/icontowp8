using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Data.REST.Entities
{
    [DataContract]
    public class Wallet
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "external_id")]
        public long ExternalId { get; set; }

        [DataMember(Name = "balance")]
        public double Balance { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }
    }
}
