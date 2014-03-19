using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.REST.Entities
{
    [DataContract]
    public class Card
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; set; }

        [DataMember(Name = "card_number")]
        public string CardNumber { get; set; }

        [DataMember(Name = "is_blocked")]
        public bool Blocked { get; set; }

        [DataMember(Name = "is_activated")]
        public bool Activated { get; set; }

        [DataMember(Name = "created_at")]
        public long CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public long UpdatedAt { get; set; }

        [DataMember(Name = "bank_id")]
        public long BankId { get; set; }
        public Bank Bank { get; set; }

        [DataMember(Name = "pan_id")]
        public long PanId { get; set; }

        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }

        [DataMember(Name = "type")]
        public byte Type { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "balance")]
        public double Balance { get; set; }

        [DataMember(Name = "card_tag_id")]
        public long CardTagId { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (this.BankId == 0)
            {
                this.Bank = new Bank()
                {
                    Id = 0,
                    Name = "Неизвестный банк"
                };
            }
        }
    }
}
