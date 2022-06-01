namespace Web.Contracts.V1.Responses
{
    public class UserAddressResponse
    {
        public string PhoneNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string RecieverName { get; set; }
    }
}
