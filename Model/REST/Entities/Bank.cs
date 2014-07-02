using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.REST.Entities
{
    [DataContract]
    public class Bank
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "image")]
        public BankImage Image { get; set; }
    }

    [DataContract]
    public class BankImage
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Url.StartsWith(@"//"))
            {
                this.Url = "http:" + this.Url;
            }
        }
    }
}
