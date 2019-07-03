using NativeBrowsers.Models;
using NativeBrowsers.UWP;
using System.Net;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsers.UWP
{
    public class NativeBrowser : INativeBrowser
    {
        public async Task<BrowserResult> LaunchBrowserAsync(string url)
        {
            var browser = new WebAuthenticationBrokerBrowser();
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;

            url = url.Replace(WebUtility.UrlEncode(Constants.RedirectUri), WebUtility.UrlEncode(redirectUri));
            Constants.RedirectUri = redirectUri;

            return await browser.InvokeAsync(url);
        }
    }
}
