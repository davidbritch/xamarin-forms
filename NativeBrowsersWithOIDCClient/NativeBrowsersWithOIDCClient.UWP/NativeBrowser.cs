using NativeBrowsersWithOIDCClient.UWP;
using System.Net;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsersWithOIDCClient.UWP
{
    public class NativeBrowser : INativeBrowser
    {
        public async Task<string> LaunchBrowserAsync(string url)
        {
            var browser = new WebAuthenticationBrokerBrowser();
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;

            url = url.Replace(WebUtility.UrlEncode(Constants.RedirectUri), WebUtility.UrlEncode(redirectUri));
            Constants.RedirectUri = redirectUri;

            return await browser.InvokeAsync(url);
        }
    }
}
