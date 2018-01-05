using System;

namespace Common.Utils.RelayCommands
{
	public class RelayCommand : RelayCommand<object>
	{
		public RelayCommand(Action<object> execute) : this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute) :base(execute, canExecute)
		{
		}
	}
}
