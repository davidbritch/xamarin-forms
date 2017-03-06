using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public class SecondPageViewModel : BindableBase
	{
		INavigationService navigationService;

		public ICommand NavigateBackCommand => new Command(async () => await GoBack());

		public SecondPageViewModel(INavigationService navService)
		{
			navigationService = navService;
		}

		async Task GoBack()
		{
			await navigationService.GoBackAsync();
		}
	}
}
