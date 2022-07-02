namespace Web.Contracts.V1.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public ApplicationUserResponse Buyer { get; set; }
        public OrderItemResponse Item { get; set; }
        public DateTime OrderDate { get; set; }

        public DateTime? ShippingDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        //User shipping info
        public string PhoneNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string RecieverName { get; set; }
    }
}
