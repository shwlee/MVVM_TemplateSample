using System;
using Define.Interfaces.CommandManager;

namespace Services.CommandManager
{
	public class CommandManagerService : ICommandManagerService
	{
		public event EventHandler RequerySuggested
		{
			add { System.Windows.Input.CommandManager.RequerySuggested += value; }
			remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
		}

		public void InvalidateRequerySuggested()
		{
			System.Windows.Input.CommandManager.InvalidateRequerySuggested();
		}
	}
}
