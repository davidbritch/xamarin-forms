using System;
using Xamarin.Forms;

namespace TodoREST
{
	public partial class TodoListPage : ContentPage
	{
		bool alertShown = false;

		public TodoListPage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

			if (Constants.RestUrl.Contains ("developer.xamarin.com")) {
				if (!alertShown) {
					await DisplayAlert (
						"Hosted Back-End",
						"This app is running against Xamarin's read-only REST service. To create, edit, and delete data you must update the service endpoint to point to your own hosted REST service.",
						"OK");
					alertShown = true;				
				}
			}

			listView.ItemsSource = await App.TodoManager.GetTasksAsync ();
		}

		async void OnAddItemActivated (object sender, EventArgs e)
		{
            await Navigation.PushAsync(new TodoItemPage(true)
            {
                BindingContext = new TodoItem
                {
                    ID = Guid.NewGuid().ToString()
                }
            });
		}

		async void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
            await Navigation.PushAsync(new TodoItemPage
            {
                BindingContext = e.SelectedItem as TodoItem
            });
		}
	}
}
