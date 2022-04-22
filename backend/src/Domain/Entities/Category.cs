using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get;set;}
    }
}
