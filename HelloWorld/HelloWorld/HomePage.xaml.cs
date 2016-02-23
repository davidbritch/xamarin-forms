using System;
using Xamarin.Forms;

namespace HelloWorld
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

		void OnValueChanged (object sender, ValueChangedEventArgs args)
		{
			valueLabel.Text = args.NewValue.ToString ("F3");
		}

		async void OnButtonClicked (object sender, EventArgs args)
		{
			await DisplayAlert ("Clicked", "The button was clicked", "OK");
		}
	}
}

