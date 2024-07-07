namespace GlobalAzure.NetAspire.Server.Models
{
    public class Result<T> where T : class
    {
        public Error Error { get; set; } = default!;

        public T Data { get; set; } = default!;

        public Result()
        {
        }
    }

    public class Error
    {
        public ErrorCode ErrorCode { get; set; }

        public string Message { get; set; } = default!;
    }

    public enum ErrorCode
    {
        Unspecified = 0,
        UnableToSaveCustomer = 1,
        CustomerNotFound = 2,
        CustomerWithSameGitHubUsernameExists = 3,
        InvalidGitHubUsername = 4
    }
}
