﻿namespace Web.Contracts.V1.Requests
{
    public class CreateAttributeTypeRequest
    {
        public int CategoryId { get; set; }
        public string AttributeTypeName { get; set; }
    }
}
