using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Requests
{
    public class CreateAttributeTypeRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
