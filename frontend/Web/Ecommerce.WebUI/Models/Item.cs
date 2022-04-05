﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce.WebUI.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Description { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
