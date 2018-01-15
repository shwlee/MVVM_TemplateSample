using System;

namespace Define.Interfaces.ServiceLocator
{
	public interface IComponentContainer
	{
		void RegisterType<TKeyType, TValueType>(bool isReuseRootComponent = true) where TValueType : TKeyType;

		void RegisterInstance(Type keyType, object value);

		T Resolve<T>();

		bool IsRegistered<T>();

		void Unregister<T>();

		void Release();
	}
}
