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
	}
}
