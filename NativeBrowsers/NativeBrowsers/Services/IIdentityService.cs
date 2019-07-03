using System.Threading.Tasks;
using NativeBrowsers.Models;

namespace NativeBrowsers.Services
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();
        Task<UserToken> GetTokenAsync(string code);
    }
}
