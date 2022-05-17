using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string ProfileName { get; set; }
}