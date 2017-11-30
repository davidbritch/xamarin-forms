using Xamarin.Forms;

namespace DependencyServiceAndLocalContext
{
    public partial class DependencyServiceAndLocalContextPage : ContentPage
    {
        public DependencyServiceAndLocalContextPage()
        {
            InitializeComponent();

            versionNumberLabel.Text = DependencyService.Get<IVersionHelper>().GetVersionNumber();
        }
    }
}
