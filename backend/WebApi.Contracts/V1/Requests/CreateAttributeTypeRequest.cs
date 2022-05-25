namespace WebApi.Contracts.V1.Requests
{
    public class CreateAttributeTypeRequest
    {
        public int CategoryId { get; set; }
        public List<string> AttributeTypes { get; set; }
    }
}
