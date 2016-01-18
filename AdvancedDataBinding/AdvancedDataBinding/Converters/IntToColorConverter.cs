using System;
using System.Globalization;
using Xamarin.Forms;

namespace AdvancedDataBinding
{
	public class IntToColorConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			int minimumLength = System.Convert.ToInt32 (parameter);
			return (int)value >= minimumLength ? Color.Green : Color.Red;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}
}

