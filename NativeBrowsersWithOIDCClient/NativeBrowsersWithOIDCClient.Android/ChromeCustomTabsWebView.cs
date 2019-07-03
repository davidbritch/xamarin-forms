using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.CustomTabs;
using System;
using System.Threading.Tasks;

namespace NativeBrowsersWithOIDCClient.Droid
{
    public class ChromeCustomTabsWebView
    {
        readonly Activity activity;
        CustomTabsActivityManager customTabs;

        public ChromeCustomTabsWebView(Activity context)
        {
            activity = context;
        }

        public Task<string> InvokeAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Missing url", nameof(url));
            }

            // Use a TCS to set the task result after the intent finishes
            var tcs = new TaskCompletionSource<string>();

            customTabs = new CustomTabsActivityManager(activity);
            var builder = new CustomTabsIntent.Builder(customTabs.Session)
                                              .SetToolbarColor(Color.Argb(255, 52, 152, 219))
                                              .SetShowTitle(true)
                                              .EnableUrlBarHiding();
            var customTabsIntent = builder.Build();

            // Ensure the intent is not kept in the history stack, which ensures navigating away will close it
            customTabsIntent.Intent.AddFlags(ActivityFlags.NoHistory);

            MainActivity.CustomUrlSchemeCallbackHandler = (response) =>
            {
                activity.StartActivity(typeof(MainActivity));
                tcs.SetResult(response);
            };

            // Launch
            customTabsIntent.LaunchUrl(activity, Android.Net.Uri.Parse(url));

            return tcs.Task;
        }
    }
}
