namespace WebApi.Contracts.V1
{
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
            public const string Update = Base + "/items/{itemId}";
            public const string Delete = Base + "/items/{itemId}";
            public const string Get = Base + "/items/{itemId}";
            public const string Create = Base + "/items";
        }

        public static class Images
        {
            public const string Upload = Base + "/images";
            public const string Get = Base + "/images/{imageName}";
            public const string Delete = Base + "/images/{imageName}";

        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }
    }


}
