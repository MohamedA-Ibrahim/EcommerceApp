using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public abstract class AuditableEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}