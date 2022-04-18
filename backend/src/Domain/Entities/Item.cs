using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get;set;}

        [Required]
        [Range(1,10000)]
        public double Price { get; set; }

        public byte[] Image { get;set;}

        [Display(Name ="Category")]
        [Required]
        public int CategoryId { get;set;}
        public Category Category { get; set; }

        public DateTime? ExpirationDate { get;set;}

    }
}
