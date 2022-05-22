using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string BuyerId { get; set; }

        [Required]
        public string SellerId { get; set; }

        [ForeignKey(nameof(SellerId))]
        public ApplicationUser Seller { get; set; }

        [ForeignKey(nameof(BuyerId))]
        public ApplicationUser Buyer { get; set; }


        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public DateTime? ShippingDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        //User shipping info
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string RecieverName { get; set; }

    }
}
