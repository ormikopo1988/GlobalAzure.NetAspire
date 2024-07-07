using System.Threading.Tasks;

namespace GlobalAzure.NetAspire.Api.Interfaces
{
    public interface IGitHubService
    {
        Task<bool> IsValidGitHubUserAsync(string username);
    }
}
