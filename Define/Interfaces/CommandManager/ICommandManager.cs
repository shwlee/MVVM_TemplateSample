using System;

namespace Define.Interfaces.CommandManager
{
	public interface ICommandManagerService
	{
		event EventHandler RequerySuggested;

		void InvalidateRequerySuggested();
	}
}
