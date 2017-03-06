using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public class FirstPageViewModel : BindableBase
	{
		INavigationService navigationService;

		User user;
		public User User
		{
			get { return user; }
			set { SetProperty(ref user, value); }
		}

		public ICommand NavigateCommand => new Command(async () => await NavigateToSecondPage());

		public FirstPageViewModel(INavigationService navService)
		{
			navigationService = navService;
			user = new User();
		}

		async Task NavigateToSecondPage()
		{
			if (user.ValidateProperties())
			{
				await navigationService.NavigateAsync("SecondPage");
			}
		}
	}
}
