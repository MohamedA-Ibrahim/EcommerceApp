namespace Web.Contracts.V1.Requests
{
    public class CreateAttributeValueRequest
    {
        public int ItemId { get; set; }
        public int AttributeTypeId { get; set; }
        public string AttributeValue { get; set; }

    }
}
