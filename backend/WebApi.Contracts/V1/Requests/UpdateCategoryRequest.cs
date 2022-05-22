namespace WebApi.Contracts.V1.Requests;

public class UpdateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}