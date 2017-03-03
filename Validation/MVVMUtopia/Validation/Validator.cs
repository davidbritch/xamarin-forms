using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MVVMUtopia
{
	public class Validator : INotifyPropertyChanged
	{
		readonly INotifyPropertyChanged entityToValidate;
		readonly IDictionary<string, ReadOnlyCollection<string>> errors = new Dictionary<string, ReadOnlyCollection<string>>();

		public static readonly ReadOnlyCollection<string> EmptyErrorsCollection = new ReadOnlyCollection<string>(new List<string>());

		public IDictionary<string, ReadOnlyCollection<string>> Errors
		{
			get { return errors; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public ReadOnlyCollection<string> this[string propertyName]
		{
			get
			{
				return errors.ContainsKey(propertyName) ? errors[propertyName] : EmptyErrorsCollection;
			}
		}

		public bool IsValidationEnabled { get; set; }

		public Validator(INotifyPropertyChanged toValidate)
		{
			if (toValidate == null)
			{
				throw new ArgumentException("entityToValidate");
			}

			entityToValidate = toValidate;
			IsValidationEnabled = true;
		}

		public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
		{
			return new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(errors);
		}

		public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
		{
			if (entityErrors == null)
			{
				throw new ArgumentException("entityErrors");
			}

			errors.Clear();
			foreach (var item in entityErrors)
			{
				SetPropertyErrors(item.Key, item.Value);
			}

			OnPropertyChanged("Item[]");
			OnErrorsChanged(string.Empty);
		}

		public bool ValidateProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			var propertyInfo = entityToValidate.GetType().GetRuntimeProperty(propertyName);
			if (propertyInfo == null)
			{
				throw new ArgumentException("The entity does not contain a property with that name.", propertyName);
			}

			var propertyErrors = new List<string>();
			bool isValid = TryValidateProperty(propertyInfo, propertyErrors);
			bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);

			if (errorsChanged)
			{
				OnErrorsChanged(propertyName);
				OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
			}

			return isValid;
		}

		public bool ValidateProperties()
		{
			var propertiesWithChangedErrors = new List<string>();
			var propertiesToValidate = entityToValidate.GetType()
													   .GetRuntimeProperties()
													   .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

			foreach (PropertyInfo propertyInfo in propertiesToValidate)
			{
				var propertyErrors = new List<string>();
				TryValidateProperty(propertyInfo, propertyErrors);

				bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);
				if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyInfo.Name))
				{
					propertiesWithChangedErrors.Add(propertyInfo.Name);
				}
			}

			foreach (string propertyName in propertiesWithChangedErrors)
			{
				OnErrorsChanged(propertyName);
				OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
			}

			return errors.Values.Count == 0;
		}

		private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
		{
			var results = new List<ValidationResult>();
			var context = new ValidationContext(entityToValidate) { MemberName = propertyInfo.Name };
			var propertyValue = propertyInfo.GetValue(entityToValidate);

			bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(propertyValue, context, results);
			if (results.Any())
			{
				propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
			}

			return isValid;
		}

		bool SetPropertyErrors(string propertyName, IList<string> propertyNewErrors)
		{
			bool errorsChanged = false;

			if (!errors.ContainsKey(propertyName))
			{
				if (propertyNewErrors.Any())
				{
					errors.Add(propertyName, new ReadOnlyCollection<string>(propertyNewErrors));
					errorsChanged = true;
				}
			}
			else
			{
				if (propertyNewErrors.Count != errors[propertyName].Count || errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count)
				{
					if (propertyNewErrors.Any())
					{
						errors[propertyName] = new ReadOnlyCollection<string>(propertyNewErrors);
					}
					else
					{
						errors.Remove(propertyName);
					}

					errorsChanged = true;
				}
			}
			return errorsChanged;
		}

		void OnPropertyChanged(string propertyName)
		{
			var eventHandler = PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void OnErrorsChanged(string propertyName)
		{
			var eventHandler = ErrorsChanged;
			if (eventHandler != null)
			{
				eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}
	}
}
