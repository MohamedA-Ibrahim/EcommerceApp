using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Responses
{
    public class SellerOrderResponse
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }


        //User shipping info
        public string PhoneNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string RecieverName { get; set; }
    }
}
