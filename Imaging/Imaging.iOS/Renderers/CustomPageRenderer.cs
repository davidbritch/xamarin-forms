using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Imaging.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]
namespace Imaging.iOS
{
    public class CustomPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Element is IModalPage modalPage)
            {
                NavigationController.TopViewController.NavigationItem.LeftBarButtonItem =
                    new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, (s, e) => modalPage.Dismiss());
            }
        }

    }
}
