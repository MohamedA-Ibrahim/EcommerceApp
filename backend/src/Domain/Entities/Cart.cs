using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart : AuditableEntity
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
