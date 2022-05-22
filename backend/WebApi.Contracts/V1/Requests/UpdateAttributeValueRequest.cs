namespace WebApi.Contracts.V1.Requests
{
    public class CreateAttributeValueRequest
    {
        public int AttributeTypeId { get; set; }
        public string AttributeValue { get; set; }

    }
}
