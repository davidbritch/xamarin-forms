using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace ExtendedFlexLayout
{
    public class ExtendedFlexLayout : FlexLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ExtendedFlexLayout), propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ExtendedFlexLayout));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldVal, object newVal)
        {
            IEnumerable newValue = newVal as IEnumerable;
            var layout = (ExtendedFlexLayout)bindable;

            var observableCollection = newValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged += layout.OnItemsSourceCollectionChanged;
            }

            layout.Children.Clear();
            if (newValue != null)
            {
                foreach (var item in newValue)
                {
                    layout.Children.Add(layout.CreateChildView(item));
                }
            }
        }

        View CreateChildView(object item)
        {
            if (ItemTemplate is DataTemplateSelector)
            {
                var dts = ItemTemplate as DataTemplateSelector;
                var itemTemplate = dts.SelectTemplate(item, null);
                itemTemplate.SetValue(BindableObject.BindingContextProperty, item);
                return (View)itemTemplate.CreateContent();
            }
            else
            {
                ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);
                return (View)ItemTemplate.CreateContent();
            }
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
            }

            if (e.OldItems != null)
            {
                Children.RemoveAt(e.OldStartingIndex);
            }

            if (e.NewItems != null)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    var item = e.NewItems[i];
                    var view = CreateChildView(item);
                    Children.Insert(e.NewStartingIndex + i, view);
                }
            }
        }
    }
}
