using Xamarin.Forms;

namespace CompiledBindings
{
    public partial class CompiledBindingsPage : ContentPage
    {
        public CompiledBindingsPage()
        {
            InitializeComponent();
            BindingContext = new CompiledBindingsViewModel();
        }
    }
}
