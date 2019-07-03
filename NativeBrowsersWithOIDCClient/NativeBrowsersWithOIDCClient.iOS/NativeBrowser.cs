using Xamarin.Forms;
using UIKit;
using Foundation;
using SafariServices;
using System.Threading.Tasks;
using NativeBrowsersWithOIDCClient.iOS;

[assembly: Dependency(typeof(NativeBrowser))]
namespace NativeBrowsersWithOIDCClient.iOS
{
    public class NativeBrowser : INativeBrowser
    {
        UIViewController rootViewController;
        SFSafariViewController safari;

        Task<string> INativeBrowser.LaunchBrowserAsync(string url)
        {
            var tcs = new TaskCompletionSource<string>();

            AppDelegate.CallbackHandler = async (response) =>
            {
                await safari.DismissViewControllerAsync(true);
                tcs.SetResult(response);
            };

            rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            safari = new SFSafariViewController(new NSUrl(url));
            rootViewController.PresentViewController(safari, true, null);

            return tcs.Task;
        }
    }
}
