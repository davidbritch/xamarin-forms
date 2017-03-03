using System;

namespace MVVMUtopia.Tests
{
	public class MockNavigationService
	{
		public Func<string, object, bool> NavigateDelegate { get; set; }
		public Action GoBackDelegate { get; set; }
		public Func<bool> CanGoBackDelegate { get; set; }
		public Action ClearHistoryDelegate { get; set; }

		public bool Navigate(string pageToken, object parameter)
		{
			return NavigateDelegate(pageToken, parameter);
		}

		public void GoBack()
		{
			GoBackDelegate();
		}

		public bool CanGoBack()
		{
			return CanGoBackDelegate();
		}

		void ClearHistory()
		{
			ClearHistoryDelegate();
		}
	}
}
