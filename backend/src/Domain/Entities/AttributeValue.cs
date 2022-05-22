using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AttributeValue : AuditableEntity
    {
        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        public int? ItemId { get; set; }
        public int AttributeTypeId { get; set; }
        public virtual Item Item { get; set; }
        public virtual AttributeType AttributeType { get; set; }


    }
}
