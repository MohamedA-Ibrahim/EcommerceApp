namespace WebApi.Contracts.V1.Requests;

public class CreateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

}