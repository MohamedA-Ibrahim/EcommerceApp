namespace WebApi.Contracts.V1.Requests
{
    public class UpdateItemRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public byte[] Image { get; set; }
        public int CategoryId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
