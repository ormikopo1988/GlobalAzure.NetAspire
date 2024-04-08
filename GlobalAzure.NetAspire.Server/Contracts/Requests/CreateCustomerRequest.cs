namespace GlobalAzure.NetAspire.Server.Contracts.Requests
{
    public class CreateCustomerRequest
    {
        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required string GitHubUsername { get; init; }
    }
}
