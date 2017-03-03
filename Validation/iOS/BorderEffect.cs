using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using MVVMUtopia.iOS;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace MVVMUtopia.iOS
{
	public class BorderEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				Control.Layer.BorderColor = UIColor.Red.CGColor;
				Control.Layer.BorderWidth = 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{
			try
			{
				Control.Layer.BorderWidth = 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}
	}
}
