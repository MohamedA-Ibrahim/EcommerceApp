using Application.Enums;
using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }

        [Required]
        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        [Display(Name = "Is Order Closed")]
        public bool IsClosed { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public DateTime? ShippingDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        [Required]
        public virtual OrderStatus OrderStatus { get; set; }

        [Required]
        public virtual PaymentStatus PaymentStatus { get; set; }

        #region User shipping info

        [Required, Display(Name = "Reciever Phone Number")]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "Recieve Street Address")]
        public string StreetAddress { get; set; }

        [Required, Display(Name = "Recieve City")]
        public string City { get; set; }

        [Required, Display(Name = "Reciever Name")]
        public string RecieverName { get; set; }

        #endregion

    }
}
