using System.ComponentModel.DataAnnotations;

namespace MVVMUtopia
{
	public class User : ValidatableBase
	{
		string forename, surname;

		const string NAMESREGEXPATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

		[Required(ErrorMessage = "This field is required.")]
		[RegularExpression(NAMESREGEXPATTERN, ErrorMessage = "This field contains invalid characters.")]
		[StringLength(10, MinimumLength = 2, ErrorMessage = "This field requires a minimum of 2 characters and a maximum of 10.")]
		public string Forename
		{
			get { return forename; }
			set { SetProperty(ref forename, value); }
		}

		[Required(ErrorMessage = "This field is required.")]
		[RegularExpression(NAMESREGEXPATTERN, ErrorMessage = "This field contains invalid characters.")]
		[StringLength(15, MinimumLength = 2, ErrorMessage = "This field requires a minimum of 2 characters and a maximum of 15.")]
		public string Surname
		{
			get { return surname; }
			set { SetProperty(ref surname, value); }
		}
	}
}
