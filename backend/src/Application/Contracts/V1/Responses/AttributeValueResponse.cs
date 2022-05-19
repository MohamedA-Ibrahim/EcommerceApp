﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Responses
{
    public class AttributeValueResponse
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public AttributeTypeResponse AttributeType { get; set; }
    }
}
