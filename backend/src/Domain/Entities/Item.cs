using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Item : AuditableEntity
{

    [Required] 
    public string Name { get; set; }

    public string Description { get; set; }

    [Required] 
    [Range(1, 10000)] 
    public double Price { get; set; }

    public string? ImageUrl { get; set; }

    [Display(Name = "Category")]
    [Required]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime? ExpirationDate { get; set; }
}