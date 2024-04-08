using System.Threading.Tasks;

namespace GlobalAzure.NetAspire.Server.Interfaces
{
    public interface IGitHubService
    {
        Task<bool> IsValidGitHubUser(string username);
    }
}
