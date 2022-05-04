using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string ProfileName { get; set; }
}