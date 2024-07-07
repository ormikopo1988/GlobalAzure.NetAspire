namespace GlobalAzure.NetAspire.Api.Contracts.Responses
{
    public class ValidateUsernameResponse
    {
        public required string GitHubUsername { get; init; }

        public required bool IsValid { get; init; }
    }
}
