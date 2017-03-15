using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public class SecondPageViewModel : BindableBase
	{
		INavigationService navigationService;

		public ICommand NavigateBackCommand => new Command(async () => await GoBackAsync());

		public SecondPageViewModel(INavigationService navService)
		{
			navigationService = navService;
		}

		async Task GoBackAsync()
		{
			await navigationService.GoBackAsync();
		}
	}
}
