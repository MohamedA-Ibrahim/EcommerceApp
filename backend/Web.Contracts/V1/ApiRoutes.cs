namespace Web.Contracts.V1;

public static class ApiRoutes
{
    public const string Root = "api";
    public const string Version = "v1";
    public const string Base = Root + "/" + Version;

    public static class Categories
    {
        public const string GetAll = Base + "/categories";
        public const string Update = Base + "/categories/{categoryId}";
        public const string Delete = Base + "/categories/{categoryId}";
        public const string Get = Base + "/categories/{categoryId}";
        public const string Create = Base + "/categories";
    }

    public static class Items
    {
        public const string GetAll = Base + "/items";
        public const string GetUserItems = Base + "/user/items";
        public const string Update = Base + "/items/{itemId}";
        public const string Delete = Base + "/items/{itemId}";
        public const string Get = Base + "/items/{itemId}";
        public const string Create = Base + "/items";
    }

    public static class Images
    {
        public const string Upload = Base + "/images";
    }

    public static class Identity
    {
        public const string Login = Base + "/identity/login";
        public const string Register = Base + "/identity/register";
        public const string Delete = Base + "/identity/delete";
        public const string Refresh = Base + "/identity/refresh";
        public const string FacebookAuth = Base + "/identity/auth/fb";
    }

    public static class UserAddress
    {
        public const string GetUserAddress = Base + "/user/address";
        public const string Upsert = Base + "/user/address";
        public const string Delete = Base + "/user/address";

    }

    public static class Orders
    {
        public const string GetSellerOrders = Base + "/user/soldOrders";
        public const string GetBuyerOrders = Base + "/user/boughtOrders";
        public const string Get = Base + "/user/orders/{orderId}";
        public const string Create = Base + "/user/orders";

        public const string StartProcessing = Base + "/user/orders/startProcessing/{orderId}";
        public const string ConfirmPayment = Base + "/user/orders/updatePayment/{orderId}";
        public const string ShipOrder = Base + "/user/orders/ship/{orderId}";
        public const string CancelOrder = Base + "/user/orders/cancel/{orderId}";

    }

    public static class AttributeTypes
    {
        public const string GetByCategoryId = Base + "/attributeTypes/{categoryId}";
        public const string Create = Base + "/attributeTypes";
        public const string Update = Base + "/attributeTypes/{attributeTypeId}";
        public const string Delete = Base + "/attributeTypes/{attributeTypeId}";
    }

    public static class AttributeValues
    {
        public const string Create = Base + "/attributeValues";
        public const string Update = Base + "/attributeValues/{attributeValueId}";
        public const string GetByItemId = Base + "/attributeValues/{itemId}";

    }

}