using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MVVMUtopia.Tests
{
	public class MockValidatableModel : INotifyPropertyChanged
	{
		[Required]
		[RegularExpression("^[A-Z][ a-zA-Z]+$")]
		[CustomValidation(typeof(MockValidatableModel), "ValidateTitle")]
		public string Title { get; set; }

		[Required]
		[RegularExpression("^[A-Z][ a-zA-Z]+$")]
		[CustomValidation(typeof(MockValidatableModel), "ValidateDescription")]
		public string Description { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public static ValidationResult ValidateTitle(object value, ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace((string)value) || ((string)value).Length < 5)
			{
				return Task.FromResult(new ValidationResult("Title must have at least 5 characters")).Result;
			}
			return Task.FromResult(ValidationResult.Success).Result;
		}

		public static ValidationResult ValidateDescription(object value, ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace((string)value) || ((string)value).Length < 5)
			{
				return Task.FromResult(new ValidationResult("Description must have at least 5 characters")).Result;
			}
			return Task.FromResult(ValidationResult.Success).Result;
		}
	}
}
