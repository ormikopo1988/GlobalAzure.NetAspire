using GlobalAzure.NetAspire.Api.Contracts.Requests;
using GlobalAzure.NetAspire.Api.Contracts.Responses;
using GlobalAzure.NetAspire.Server.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace GlobalAzure.NetAspire.Server.Services
{
    public class UserValidatorClient : IUserValidatorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserValidatorClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsValidUsernameAsync(string username)
        {
            var client = _httpClientFactory.CreateClient("UserValidatorClient");

            var json = JsonSerializer.Serialize(
                new ValidateUsernameRequest
                {
                    GitHubUsername = username
                });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/usernames/validate", data);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response
                    .Content
                    .ReadFromJsonAsync<ValidateUsernameResponse>();

                return responseBody!.IsValid;
            }

            return false;
        }
    }
}
