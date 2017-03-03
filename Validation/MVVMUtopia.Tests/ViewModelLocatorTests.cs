using System;
using NUnit.Framework;

namespace MVVMUtopia.Tests
{
	[TestFixture]
	public class ViewModelLocatorTests
	{
		[Test]
		public void AutoWireViewModelWithFactoryRegistration()
		{
			var page = new MockPage();

			ViewModelLocator.Register(typeof(MockPage).ToString(), () => new MockPageViewModel());
			ViewModelLocator.SetAutoWireViewModel(page, true);

			Assert.IsNotNull(page.BindingContext);
			Assert.IsInstanceOf(typeof(MockPageViewModel), page.BindingContext);
		}

		[Test]
		public void AutoWireViewModelWithCustomResolver()
		{
			var page = new MockPage();

			ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
			{
				var viewName = viewType.FullName;
				var viewModelName = string.Format("{0}ViewModel", viewName);
				return Type.GetType(viewModelName);
			});

			ViewModelLocator.SetAutoWireViewModel(page, true);

			Assert.IsNotNull(page.BindingContext);
			Assert.IsInstanceOf(typeof(MockPageViewModel), page.BindingContext);
		}

		[Test]
		public void AutoWireViewModelWithCustomResolverAndFactory()
		{
			var page = new MockPage();

			ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
			{
				var viewName = viewType.FullName;
				var viewModelName = string.Format("{0}ViewModel", viewName);
				return Type.GetType(viewModelName);
			});

			ViewModelLocator.SetDefaultViewModelFactory((viewModelType) =>
			{
				return Activator.CreateInstance(viewModelType) as ViewModelBase;
			});

			ViewModelLocator.SetAutoWireViewModel(page, true);

			Assert.IsNotNull(page.BindingContext);
			Assert.IsInstanceOf(typeof(MockPageViewModel), page.BindingContext);
		}
	}
}
