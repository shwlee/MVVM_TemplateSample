using Common.ServiceLocators;
using Define.Enums;
using Define.Interfaces.WindowServices;
using Entry.Bootstrap;
using System.Windows;
using Define.Classes;
using ViewModels.Contents;
using Views.Windows;

namespace Entry
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			this.ShutdownMode = ShutdownMode.OnMainWindowClose;

			// To Assembly load.
			var preLoadViewType = typeof(CommonWindow);

			Bootstrapper.Initialize();

			var container = ContainerResolver.GetContainer();
			var windowManager = container.Resolve<IWindowService>();

			var mainViewModel = new MainViewModel();
			var setting = new WindowSettings(
				"Test Main", 
				WindowStyles.ToolWindow, 
				WindowResizeModes.CanResize,
				WindowSizeToContents.WidthAndHeight, 
				WindowViewStates.Normal);
			

			var entryWindow = windowManager.GetOrCreateWindow(mainViewModel, setting);
			entryWindow.Show();
		}
	}
}
