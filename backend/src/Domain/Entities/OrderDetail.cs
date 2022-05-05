using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail : AuditableEntity
    {
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ItemId { get; set; }

        public Item Item { get; set; }
        public int Count { get; set; } = 1;
        public double OrderTotal { get; set; }
    }
}
