using System;

namespace GlobalAzure.NetAspire.Server.Dtos
{
    public class CustomerDto
    {
        public required Guid Id { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }
        
        public required string GitHubUsername { get; init; }
    }
}
