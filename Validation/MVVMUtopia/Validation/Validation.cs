using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public static class Validation
	{
		public static readonly BindableProperty ErrorsProperty =
			BindableProperty.CreateAttached(
				"Errors",
				typeof(ReadOnlyCollection<string>),
				typeof(Validation),
				Validator.EmptyErrorsCollection,
				propertyChanged: OnPropertyErrorsChanged);

		public static ReadOnlyCollection<string> GetErrors(BindableObject element)
		{
			return (ReadOnlyCollection<string>)element.GetValue(ErrorsProperty);
		}

		public static void SetErrors(BindableObject element, ReadOnlyCollection<string> value)
		{
			element.SetValue(ErrorsProperty, value);
		}

		static void OnPropertyErrorsChanged(BindableObject element, object oldValue, object newValue)
		{
			var view = element as View;
			if (view == null | oldValue == null || newValue == null)
			{
				return;
			}

			var propertyErrors = (ReadOnlyCollection<string>)newValue;
			if (propertyErrors.Any())
			{
				view.Effects.Add(new BorderEffect());
			}
			else
			{
				var effectToRemove = view.Effects.FirstOrDefault(e => e is BorderEffect);
				if (effectToRemove != null)
				{
					view.Effects.Remove(effectToRemove);
				}
			}
		}
	}
}
