using Xamarin.Forms;

namespace ItemSelectedBehavior
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
			BindingContext = new HomePageViewModel ();
		}
	}
}

