using System;
using System.Linq;
using Define.Interfaces.ServiceLocator;
using Unity;
using Unity.Lifetime;

namespace Services.ServiceLocator
{
	public class ComponentContainer : IComponentContainer
	{
		private readonly Lazy<UnityContainer> _lazyUnityContianer = new Lazy<UnityContainer>(() => new UnityContainer());

		public void RegisterType<TKeyType, TValueType>(bool isReuseRootComponent = true) where TValueType : TKeyType
		{
			ContainerControlledLifetimeManager lifeTimeManager = null;
			if (isReuseRootComponent)
			{
				lifeTimeManager = new ContainerControlledLifetimeManager();
			}

			this._lazyUnityContianer.Value.RegisterType<TKeyType, TValueType>(lifeTimeManager);
		}

		public void RegisterInstance(Type keyType, object value)
		{
			var lifeTimeManager = new ContainerControlledLifetimeManager();
			this._lazyUnityContianer.Value.RegisterInstance(keyType, value, lifeTimeManager);
		}

		public T Resolve<T>()
		{
			return this._lazyUnityContianer.Value.Resolve<T>();
		}

		public void Unregister<T>()
		{
			// 등록된 인스턴스만 제거한다. key와 value 의 타입은 유지.

			var registrations = this._lazyUnityContianer.Value.Registrations.Where(t => t.RegisteredType == typeof(T));
			foreach (var registration in registrations)
			{
				registration.LifetimeManager.RemoveValue();
			}
		}

		public void Release()
		{
			this._lazyUnityContianer.Value.Dispose();
		}
	}
}
