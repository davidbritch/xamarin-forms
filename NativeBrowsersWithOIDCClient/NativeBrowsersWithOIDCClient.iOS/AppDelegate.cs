using System;
using Foundation;
using UIKit;

namespace NativeBrowsersWithOIDCClient.iOS
{

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static Action<string> CallbackHandler { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            CallbackHandler(url.AbsoluteString);
            CallbackHandler = null;
            return true;
        }
    }
}
