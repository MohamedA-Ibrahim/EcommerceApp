using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.WebUI.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, 100000)]
        public double Price { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool Sold { get; set; } = false;

        public string CreatedBy { get; set; }

    }
}
