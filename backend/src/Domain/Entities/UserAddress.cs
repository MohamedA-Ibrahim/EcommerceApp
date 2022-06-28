using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserAddress : AuditableEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string StreetAddress { get; set; }

        [Required]
        [MaxLength(40)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string RecieverName { get; set; }
    }
}
