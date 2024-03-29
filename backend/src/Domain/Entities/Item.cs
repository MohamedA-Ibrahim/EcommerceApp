﻿using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Item : AuditableEntity
{

    [Required]
    [MaxLength(300)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(1, 100000)]
    public double Price { get; set; }
    public string ImageUrl { get; set; }

    [Display(Name = "Category")]
    [Required]
    public int CategoryId { get; set; }

    public string SellerId { get; set; }
    public ApplicationUser Seller { get; set; }

    public Category Category { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool Sold { get; set; } = false;
    public virtual ICollection<AttributeValue> AttributeValues { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}