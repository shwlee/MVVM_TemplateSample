using Define.Enums;
using Define.Interfaces.Dispatcher;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Services.Dispatcher
{
	public class DispatcherService : IDispatcherService
	{
		public async void BeginInvoke(Action action)
		{
			await Application.Current.Dispatcher.BeginInvoke(action, DispatcherPriority.Render);
		}

		public void BeginInvokeShutdown(InvokePriority priority)
		{
			var dispatcherPriority = (DispatcherPriority)Enum.Parse(typeof(InvokePriority), priority.ToString(), true);
			Application.Current.Dispatcher.BeginInvokeShutdown(dispatcherPriority);
		}

		public void Invoke(Action action)
		{
			Application.Current.Dispatcher.Invoke(action);
		}

		public bool CheckAccess()
		{
			return Application.Current.Dispatcher.CheckAccess();
		}

		public void VerifyAccess()
		{
			Application.Current.Dispatcher.VerifyAccess();
		}
	}
}
