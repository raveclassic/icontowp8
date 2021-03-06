﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Clients.REST
{
    [DataContract]
    public class CommonResponse<ResponseContentType>
    {
        [DataMember(Name = "status")]
        public long Status { get; set; }

        [DataMember(Name = "msg", IsRequired = false)]
        public string Message { get; set; }

        [DataMember(Name = "data", IsRequired = false)]
        public ResponseContentType Data { get; set; }
    }

    [DataContract]
    public class CommonArrayResponse<ResponseContentType>
    {
        [DataMember(Name = "status")]
        public long Status { get; set; }

        [DataMember(Name = "msg", IsRequired = false)]
        public string Message { get; set; }

        [DataMember(Name = "data")]
        public ArraySubResponse<ResponseContentType> Data { get; set; }
    }

    [DataContract]
    public class ArraySubResponse<ResponseContentListType>
    {
        [DataMember(Name = "items")]
        public ResponseContentListType Items { get; set; }
    }
}
