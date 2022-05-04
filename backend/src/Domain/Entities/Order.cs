using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : AuditableEntity
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public string UserId { get; set; }

        public Item Item { get; set; }
    }
}
