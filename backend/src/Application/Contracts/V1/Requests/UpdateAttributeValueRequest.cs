using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Requests
{
    public class CreateAttributeValueRequest
    {
        public int ItemId { get; set; }
        public int AttributeTypeId { get; set; }
        public string AttributeValue { get; set; }

    }
}
