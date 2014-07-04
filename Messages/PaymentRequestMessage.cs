using Iconto.PCL.Services.Data.REST.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Messages
{
    public class PaymentRequestMessage
    {
        public CreateOrderResponse OrderResponse { get; private set; }
        public string Sid { get; private set; }

        public PaymentRequestMessage(CreateOrderResponse order, string sid)
        {
            OrderResponse = order;
            Sid = sid;
        }
    }
}
