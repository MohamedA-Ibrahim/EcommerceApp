﻿using Domain.Common;
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

        public Item Item { get; set; }

        [Required]
        public string SellerId { get; set; }

        [Required]
        public string BuyerId { get; set; }

        public DateTime SellingDate { get; set; }

    }
}
