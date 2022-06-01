namespace Web.Contracts.V1.Requests
{
    public class CreateOrderRequest
    {
        public int ItemId { get; set; }

        public string PhoneNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string RecieverName { get; set; }
    }
}
