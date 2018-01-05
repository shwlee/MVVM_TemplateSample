using System;
using Define.Interfaces.ServiceLocator;
using Services.ServiceLocator;

namespace Common.ServiceLocators
{
	public class ContainerResolver
	{
		private static readonly Lazy<ComponentContainer> LazyComponentContainer = new Lazy<ComponentContainer>(() => new ComponentContainer());

		public static IComponentContainer GetContainer()
		{
			return LazyComponentContainer.Value;
		}
	}
}
