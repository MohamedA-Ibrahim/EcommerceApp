using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Requests
{
    public class UpdateAttributeValueRequest
    {
        public int Id { get; set; }
        public string Value { get; set; }

    }
}
