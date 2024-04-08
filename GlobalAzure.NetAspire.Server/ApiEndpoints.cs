namespace GlobalAzure.NetAspire.Server
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Customers
        {
            private const string Base = $"{ApiBase}/customers";

            public const string Create = Base;

            public const string GetAll = Base;

            public const string GetById = $"{Base}/{{id:guid}}";
        }
    }
}
