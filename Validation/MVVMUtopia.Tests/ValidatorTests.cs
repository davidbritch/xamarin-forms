using System;
using NUnit.Framework;

namespace MVVMUtopia.Tests
{
	[TestFixture]
	public class ValidatorTests
	{
		[Test]
		public void ValidationOfFieldWhenValidShouldSucceed()
		{
			var model = new MockValidatableModel { Title = "A valid title" };
			var target = new Validator(model);

			bool isValid = target.ValidateProperty("Title");

			Assert.IsTrue(isValid);
			Assert.IsTrue(target.GetAllErrors().Values.Count == 0);
		}

		[Test]
		public void ValidationOfFieldWhenInvalidShouldFail()
		{
			var model = new MockValidatableModel { Title = string.Empty };
			var target = new Validator(model);

			bool isValid = target.ValidateProperty("Title");

			Assert.IsFalse(isValid);
			Assert.IsFalse(target.GetAllErrors().Values.Count == 0);
		}

		[Test]
		public void ValidationOfFieldsWhenValidShouldSucceed()
		{
			var model = new MockValidatableModel
			{
				Title = "A valid title",
				Description = "A valid description"
			};
			var target = new Validator(model);

			bool isValid = target.ValidateProperties();

			Assert.IsTrue(isValid);
			Assert.IsTrue(target.GetAllErrors().Values.Count == 0);
		}

		[Test]
		public void ValidationOfFieldsWhenInvalidShouldFail()
		{
			// Invalid title
			var model = new MockValidatableModel
			{
				Title = string.Empty,
				Description = "A valid description"
			};
			var target = new Validator(model);

			bool isValid = target.ValidateProperties();

			Assert.IsFalse(isValid);
			Assert.IsFalse(target.GetAllErrors().Values.Count == 0);

			// Invalid description
			model = new MockValidatableModel
			{
				Title = "A valid title",
				Description = string.Empty
			};
			target = new Validator(model);

			isValid = target.ValidateProperties();

			Assert.IsFalse(isValid);
			Assert.IsFalse(target.GetAllErrors().Values.Count == 0);

			// Invalid title and description
			model = new MockValidatableModel
			{
				Title = "0123456789",
				Description = string.Empty
			};
			target = new Validator(model);

			isValid = target.ValidateProperties();

			Assert.IsFalse(isValid);
			Assert.IsFalse(target.GetAllErrors().Values.Count == 0);
		}

		[Test]
		public void ValidationOfNonexistentPropertyShouldThrow()
		{
			var model = new MockValidatableModel();
			var target = new Validator(model);

			var exception = Assert.Throws<ArgumentException>(() =>
			{
				target.ValidateProperty("DoesNotExist");
			});

			const string expectedMessage = "The entity does not contain a property with that name.\nParameter name: DoesNotExist";
			Assert.AreEqual(expectedMessage, exception.Message);
		}
	}
}
