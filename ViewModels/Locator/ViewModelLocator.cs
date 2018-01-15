using Common.ServiceLocators;
using Define.Interfaces.ServiceLocator;
using System;

namespace ViewModels.Locator
{
	public static class ViewModelLocator
	{
		private static IComponentContainer _container;

		private static IComponentContainer Container
		{
			get
			{
				CheckInitialized();
				return _container;
			}
		}

		public static void RegisterViewModel<T>()
		{
			Container.RegisterType<T, T>();
		}

		public static T GetViewModel<T>()
		{
			if (Container.IsRegistered<T>() == false)
			{
				Container.RegisterType<T, T>();
			}

			return Container.Resolve<T>();
		}

		private static void CheckInitialized()
		{
			if (_container != null)
			{
				return;
			}

			
			_container = ContainerResolver.GetContainer();
			if (_container == null)
			{
				throw new NullReferenceException("Main service locator container is not created.");
			}
		}
	}
}
