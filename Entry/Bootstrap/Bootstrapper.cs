using Common.ServiceLocators;
using Define.EventAggregators;
using Define.Interfaces.CommandManager;
using Define.Interfaces.CreateInstances;
using Define.Interfaces.Dispatcher;
using Define.Interfaces.WindowServices;
using Services.CommandManager;
using Services.CreateInstances;
using Services.Dispatcher;
using Services.Window;

namespace Entry.Bootstrap
{
	class Bootstrapper
	{
		public static void Initialize()
		{
			// TODO : 추후 필요한 것이 있으면 추가할 것.
			var container = ContainerResolver.GetContainer();

			container.RegisterType<IWindowService, WindowService>();
			container.RegisterType<IEventAggregator, EventAggregator>();
			container.RegisterType<ICommandManagerService, CommandManagerService>();
			container.RegisterType<IDispatcherService, DispatcherService>();
			container.RegisterType<IInstanceCreate, InstanceCreator>();
		}
	}
}
