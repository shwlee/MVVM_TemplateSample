using System;
using System.Windows.Input;
using Common.ServiceLocators;
using Define.Interfaces.CommandManager;

namespace Common.Utils.RelayCommands
{
	public class RelayCommand<T> : ICommand
	{
		private ICommandManagerService _commandManager;

		private readonly Predicate<T> _canExecute;

		private readonly Action<T> _execute;

		private ICommandManagerService CommandManager
		{
			get
			{
				this.InitCommandManagerService();
				return this._commandManager;
			}
		}

		public RelayCommand(Action<T> execute) : this(execute, null)
		{
			this._execute = execute;
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			this._execute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				// need PresetaionCore.dll
				// CommandManager 를 분리할 경우 ServiceLocator를 이용해 interface로 처리.
				//CommandManager.RequerySuggested += value;
				this.CommandManager.RequerySuggested += value;
			}
			remove
			{
				this.CommandManager.RequerySuggested -= value;
			}
		}

		private void InitCommandManagerService()
		{
			if (this._commandManager != null)
			{
				return;
			}

			// TODO : 이하 구문 실패 시 예외 발생 처리 필요.
			var container = ContainerResolver.GetContainer();
			this._commandManager = container.Resolve<ICommandManagerService>();
		}
	}
}
