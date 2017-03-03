using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MVVMUtopia.Droid;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace MVVMUtopia.Droid
{
	public class BorderEffect : PlatformEffect
	{
		Drawable originalBackground;

		protected override void OnAttached()
		{
			try
			{
				originalBackground = Control.Background;

				var shape = new ShapeDrawable(new RectShape());
				shape.Paint.Color = Android.Graphics.Color.Red;
				shape.Paint.StrokeWidth = 5;
				shape.Paint.SetStyle(Paint.Style.Stroke);
				Control.SetBackground(shape);
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
				Control.Background = originalBackground;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}
	}
}
