using Android.App;
using Xamarin.Forms;
using NativeBrowsers.Droid;
using NativeBrowsers.Models;
using System.Threading.Tasks;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsers.Droid
{
    public class NativeBrowser : INativeBrowser
    {
        async Task<BrowserResult> INativeBrowser.LaunchBrowserAsync(string url)
        {
            var browser = new ChromeCustomTabsWebView((Activity)MainActivity.Instance);
            return await browser.InvokeAsync(url);
        }
    }
}
