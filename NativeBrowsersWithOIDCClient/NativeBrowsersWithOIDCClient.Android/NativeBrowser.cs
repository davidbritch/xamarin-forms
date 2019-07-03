using System.Threading.Tasks;
using Xamarin.Forms;
using Android.App;
using NativeBrowsersWithOIDCClient.Droid;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsersWithOIDCClient.Droid
{
    public class NativeBrowser : INativeBrowser
    {
        async Task<string> INativeBrowser.LaunchBrowserAsync(string url)
        {
            var browser = new ChromeCustomTabsWebView((Activity)MainActivity.Instance);
            return await browser.InvokeAsync(url);
        }
    }
}
