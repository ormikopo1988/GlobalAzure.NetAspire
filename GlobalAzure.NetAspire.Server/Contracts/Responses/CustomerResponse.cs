using System;

namespace GlobalAzure.NetAspire.Server.Contracts.Responses
{
    public class CustomerResponse
    {
        public required Guid Id { get; init; }

        public required string FullName { get; init; }

        public required string GitHubUsername { get; init; }
    }
}
