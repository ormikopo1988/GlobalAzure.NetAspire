using System.Threading.Tasks;

namespace GlobalAzure.NetAspire.Server.Interfaces
{
    public interface IUserValidatorClient
    {
        Task<bool> IsValidUsernameAsync(string username);
    }
}
