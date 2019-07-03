using Xamarin.Forms;
using NativeBrowsers.iOS;
using NativeBrowsers.Models;
using UIKit;
using Foundation;
using SafariServices;
using System.Threading.Tasks;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsers.iOS
{
    public class NativeBrowser : INativeBrowser
    {
        UIViewController rootViewController;
        SFSafariViewController safari;

        Task<BrowserResult> INativeBrowser.LaunchBrowserAsync(string url)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();

            AppDelegate.OpenUrlCallbackHandler = async (response) =>
            {
                await safari.DismissViewControllerAsync(true);

                tcs.SetResult(new BrowserResult
                {
                    Response = response,
                    ResultType = BrowserResultType.Success
                });
            };

            rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            safari = new SFSafariViewController(new NSUrl(url));
            rootViewController.PresentViewController(safari, true, null);

            return tcs.Task;
        }
    }
}
