using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class AuditableEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    [MaxLength(50)]
    public string? LastModifiedBy { get; set; }
}