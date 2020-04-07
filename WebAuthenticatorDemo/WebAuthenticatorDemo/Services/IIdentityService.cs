using System.Threading.Tasks;
using WebAuthenticatorDemo.Models;

namespace WebAuthenticatorDemo.Services
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();
        Task<UserToken> GetTokenAsync(string code);
        Task<string> GetAsync(string uri, string accessToken);
    }
}
