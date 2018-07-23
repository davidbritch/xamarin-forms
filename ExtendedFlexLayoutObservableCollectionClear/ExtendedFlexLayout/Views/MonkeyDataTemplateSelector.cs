using Xamarin.Forms;

namespace ExtendedFlexLayout
{
    public class MonkeyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalMonkeyTemplate { get; set; }
        public DataTemplate CynicalMonkeyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((Monkey)item).Name.Contains("Face-Palm") ? CynicalMonkeyTemplate: NormalMonkeyTemplate; 
        }
    }
}
