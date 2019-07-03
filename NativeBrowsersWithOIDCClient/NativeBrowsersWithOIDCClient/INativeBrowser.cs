using System.Threading.Tasks;

namespace NativeBrowsersWithOIDCClient
{
    public interface INativeBrowser
    {
        Task<string> LaunchBrowserAsync(string url);
    }
}
