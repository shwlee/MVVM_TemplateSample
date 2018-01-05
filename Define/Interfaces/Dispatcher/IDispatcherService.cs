using System;
using Define.Enums;

namespace Define.Interfaces.Dispatcher
{
	public interface IDispatcherService
	{
		// NOTE : DispatcherOperation 이 필요하면 랩핑하여 반환하는 오버로딩을 만들 것.

		void BeginInvoke(Action action);
		
		void BeginInvokeShutdown(InvokePriority priority);

		void Invoke(Action action);

		bool CheckAccess();
		
		void VerifyAccess();
	}
}
