using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Clients.REST.Entities
{
    [DataContract]
    public class Address
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        //[DataMember(Name = "company_id")]
        //public long CompanyId { get; set; }

        //[DataMember(Name = "legal_id")]
        //public long LegalId { get; set; }

        //[DataMember(Name = "city_id")]
        //public long CityId { get; set; }

        //[DataMember(Name = "name")]
        //public string Name { get; set; }


        //[DataMember(Name = "latitude")]
        //public long Lat { get; set; }

        //[DataMember(Name = "longitude")]
        //public long Lon { get; set; }

        //[DataMember(Name = "contacts")]
        //public List<AddressContact> Contacts { get; set; }

        //id: 6,
        //company_id: 9,
        //legal_id: 10016,
        //country_id: 6,
        //city_id: 2621,
        //name: "MACARENA, ресторан",
        //company_name: "MACARENA, ресторан",
        //address: "московский 206",
        //phones: [
        //"+78129063900"
        //],
        //is_ecommerce: false,
        //latitude: 59.857174,
        //longitude: 30.322318,
        //google_place_id: "8e02401daf6033511d95dab7c0146e0d7523e65b",
        //google_place_ref: "CnRqAAAAPMvByMaKywd0iLMxcD5-5WzSA0w_fKRI9RLkYTjy7OiBuI4u0MA7AHAL0jf2ZJTzbuzvoAaNtZIyMY7qmjKE1Jd6vcmrYak_b_YkVfZOGVayEz8wBobtmT9XJWgtvThgWVSvbGRj-c9FXEIW0PbxxhIQpxotJhzbPbmWzfNn78-W5hoUPMb52k-okR_VCWnaapaHORarshw",
        //contacts: [
        //{
        //id: 188,
        //company_id: 0,
        //user_id: 644,
        //deleted: false,
        //position: "Employee",
        //position_type: 0,
        //legal_id: 0,
        //address_id: 6,
        //created_at: 1404291971,
        //send_sms: false
        //}
        //]
    }

    [DataContract]
    public class AddressContact
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "company_id")]
        public long CompanyId { get; set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; set; }

        [DataMember(Name = "position")]
        public string Position { get; set; }

        [DataMember(Name = "address_id")]
        public long AddressId { get; set; }
    }
}
