using System.Threading.Tasks;

namespace NativeBrowsers.Services
{
    public interface IRequestProvider
    {
        Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret);
    }
}
