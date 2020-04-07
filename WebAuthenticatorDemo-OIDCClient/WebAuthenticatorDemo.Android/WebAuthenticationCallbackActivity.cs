using Android.App;
using Android.Content;
using Android.Content.PM;
using Xamarin.Essentials;

namespace WebAuthenticatorDemo.Droid
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionView},
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable},
        DataScheme = "io.identitymodel.native",
        DataHost = "callback")]
    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
    }
}
