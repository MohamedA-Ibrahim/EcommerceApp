using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public abstract class AuditableEntity
{
    public int Id { get; set; }
}