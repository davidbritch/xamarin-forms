using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public class BindableBase : BindableObject
	{
		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value))
			{
				return false;
			}

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}
	}
}
