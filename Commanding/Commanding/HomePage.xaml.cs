using Xamarin.Forms;

namespace Commanding
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
			BindingContext = new DemoViewModel ();
		}
	}
}

