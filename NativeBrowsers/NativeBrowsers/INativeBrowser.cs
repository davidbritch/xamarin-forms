using System.Threading.Tasks;
using NativeBrowsers.Models;

namespace NativeBrowsers
{
    public interface INativeBrowser
    {
        Task<BrowserResult> LaunchBrowserAsync(string url);
    }
}
