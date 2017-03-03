using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace MVVMUtopia
{
	public static class ViewModelLocator
	{
		static Dictionary<string, Func<object>> factories = new Dictionary<string, Func<object>>();

		static Func<Type, object> defaultViewModelFactory = type => Activator.CreateInstance(type);

		static Func<Type, Type> defaultViewTypeToViewModelTypeResolver = viewType =>
		{
			var viewName = viewType.FullName;
			viewName = viewName.Replace(".Views.", ".ViewModels.");
			var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
			var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
			return Type.GetType(viewModelName);
		};

		public static readonly BindableProperty AutoWireViewModelProperty =
			BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

		public static bool GetAutoWireViewModel(BindableObject bindable)
		{
			return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
		}

		public static void SetAutoWireViewModel(BindableObject bindable, bool value)
		{
			bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
		}

		static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var view = bindable as Element;
			if (view == null)
			{
				return;
			}

			// Try mappings first
			object viewModel = GetViewModelForView(view);

			// Fall back to convention based
			if (viewModel == null)
			{
				var viewModelType = defaultViewTypeToViewModelTypeResolver(view.GetType());
				if (viewModelType == null)
				{
					return;
				}
				viewModel = defaultViewModelFactory(viewModelType);
			}

			view.BindingContext = viewModel;
		}

		static object GetViewModelForView(Element view)
		{
			if (factories.ContainsKey(view.GetType().ToString()))
			{
				return factories[view.GetType().ToString()]();
			}
			return null;
		}

		public static void Register(string viewTypeName, Func<object> factory)
		{
			factories[viewTypeName] = factory;
		}

		public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
		{
			defaultViewModelFactory = viewModelFactory;
		}

		public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
		{
			defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
		}
	}
}
