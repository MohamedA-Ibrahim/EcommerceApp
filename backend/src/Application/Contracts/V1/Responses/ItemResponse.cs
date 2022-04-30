﻿namespace Application.Contracts.V1.Responses
{
    public class ItemResponse
    {
        public int Id{get;set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        public CategoryResponse Category { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
