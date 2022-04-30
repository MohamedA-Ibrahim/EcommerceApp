using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Category : AuditableEntity
{

    [Required] 
    [MaxLength(50)] 
    public string Name { get; set; }
}