using System;

namespace GlobalAzure.NetAspire.Server.Data.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string GitHubUsername { get; set; } = string.Empty;
    }
}
