using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.REST.Entities
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "login")]
        public string Login { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "birthday")]
        public string Birthday { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        //login: ''
        //nickname: ''
        //birthday: ''
        //sex: 0
        //marriage: 0
        //country: 0
        //city: 0
        //email: ''
        //first_name: ''
        //last_name: ''
        //image:
        //    url: ''
    }
}
