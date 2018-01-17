using System.Diagnostics.Contracts;
using Common.ServiceLocators;
using Common.Utils.RelayCommands;
using Define.Interfaces.WindowServices;
using System.Windows.Input;
using Define.Interfaces.CreateInstances;
using ViewModels.Locator;

namespace ViewModels.Contents
{
	public class MainViewModel : ContentViewModel
	{
		private RelayCommand _goNextCommand;

		public ICommand GoNextCommand => this._goNextCommand ?? (this._goNextCommand = new RelayCommand(this.GoNext));

		private void GoNext(object obj)
		{
			var container = ContainerResolver.GetContainer();
			var instanceCreator = container.Resolve<IInstanceCreate>();
			var viewModel = instanceCreator.Create<NextViewModel>();

			return;
			var windowService = container.Resolve<IWindowService>();
			var next = ViewModelLocator.GetViewModel<NextViewModel>();
			windowService.ChangeContent(this, next);
		}
	}
}
