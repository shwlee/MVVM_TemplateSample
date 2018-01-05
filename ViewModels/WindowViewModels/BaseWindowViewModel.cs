using Common.ServiceLocators;
using Common.Utils.RelayCommands;
using Define.Interfaces.WindowServices;
using System.Windows.Input;
using Define.Enums;
using Define.Interfaces.Dispatcher;
using ViewModels.Base;

namespace ViewModels.WindowViewModels
{
	public abstract class BaseWindowViewModel : ViewModelBase, IWindowContext
	{
		#region Fields

		private IWindowService _windowService;

		private RelayCommand _closeCommand;

		private RelayCommand _minimizeCommand;

		private RelayCommand<bool> _normalOrMaximizeCommand;

		private WindowSizeToContents _beforeMaximizedSizeToContent;

		private IWindowContent _contentContext;

		private IDispatcherService _dispatcher;

		#endregion

		protected IWindowService WindowService
		{
			get
			{
				if (this._windowService == null)
				{
					var container = ContainerResolver.GetContainer();
					this._windowService = container.Resolve<IWindowService>();
				}

				return this._windowService;
			}
		}

		protected IDispatcherService Dispatcher
		{
			get
			{
				if (this._dispatcher == null)
				{
					var container = ContainerResolver.GetContainer();
					this._dispatcher = container.Resolve<IDispatcherService>();
				}

				return this._dispatcher;
			}
		}

		public IWindowContent ContentContext
		{
			get { return this._contentContext; }
			set
			{
				this.WindowService.ChangeWindowSizeToContent(this, this.Setting.SizeToContent);
				this._contentContext = value;
				this.Dispatcher.BeginInvoke(() => this.WindowService.ChangeWindowSizeToContent(this, WindowSizeToContents.Manual));
			}
		}

		public IWindowSettings Setting { get; set; }

		#region Commands

		public ICommand CloseCommand =>
			this._closeCommand ?? (this._closeCommand = new RelayCommand(this.Close));

		protected virtual void Close(object obj)
		{
			this.ContentContext.Close();

			this.WindowService.CloseWindow(this);
		}

		public ICommand MinimizeCommand =>
			this._minimizeCommand ?? (this._minimizeCommand = new RelayCommand(this.Minimize));

		protected virtual void Minimize(object obj)
		{
			this.WindowService.ChangeWindowState(this, WindowViewStates.Minimized);
		}

		public ICommand NormalOrMaximizeCommand =>
			this._normalOrMaximizeCommand ??
			(this._normalOrMaximizeCommand = new RelayCommand<bool>(this.NormalOrMaximize));

		protected virtual void NormalOrMaximize(bool isToNormal)
		{
			var toState = isToNormal ? WindowViewStates.Normal : WindowViewStates.Maximized;
			this.WindowService.ChangeWindowState(this, toState);
		}

		#endregion
	}
}
