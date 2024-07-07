namespace GlobalAzure.NetAspire.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Customers
        {
            private const string Base = $"{ApiBase}/usernames";

            public const string Validate = $"{Base}/validate";
        }
    }
}
