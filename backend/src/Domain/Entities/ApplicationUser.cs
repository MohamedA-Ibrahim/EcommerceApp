using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string ProfileName { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}