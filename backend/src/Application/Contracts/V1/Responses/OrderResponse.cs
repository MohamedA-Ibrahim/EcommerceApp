using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public DateTime SellingDate { get; set; }
    }
}
