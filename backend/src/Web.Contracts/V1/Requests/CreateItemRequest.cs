﻿namespace Web.Contracts.V1.Requests;

public class CreateItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public DateTime? ExpirationDate { get; set; }
}