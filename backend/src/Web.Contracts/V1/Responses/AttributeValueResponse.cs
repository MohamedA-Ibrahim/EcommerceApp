namespace Web.Contracts.V1.Responses
{
    public class AttributeValueResponse
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public AttributeTypeResponse AttributeType { get; set; }
    }
}
