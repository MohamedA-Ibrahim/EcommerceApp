
using System.ComponentModel.DataAnnotations;


namespace Web.Contracts.V1.Requests
{
    public class UpsertUserAddressRequest
    {
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
