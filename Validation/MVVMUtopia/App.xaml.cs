using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MVVMUtopia
{
	public partial class App : Application
	{
		INavigationService navigationService;

		public App()
		{
			InitializeComponent();
			InitializeServices();

			MainPage = new NavigationPage();
			navigationService.NavigateAsync("FirstPage");
		}

		void InitializeServices()
		{
			navigationService = new NavigationService();
			((NavigationService)navigationService).RegisterPage("FirstPage", () => new FirstPage());
			((NavigationService)navigationService).RegisterPage("SecondPage", () => new SecondPage());

			ViewModelLocator.Register(typeof(FirstPage).ToString(), () => new FirstPageViewModel(navigationService));
			ViewModelLocator.Register(typeof(SecondPage).ToString(), () => new SecondPageViewModel(navigationService));
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
