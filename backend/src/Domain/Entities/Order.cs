using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string BuyerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        //tracking number for shipping
        public string? TrackingNumber { get; set; }
       
        //For integration with paymob 
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        //User shipping info
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get;set;}

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string RecieverName { get; set; }

    }
}
