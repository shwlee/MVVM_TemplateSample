using Common.ServiceLocators;
using Common.Utils.RelayCommands;
using Define.Interfaces.WindowServices;
using System.Windows.Input;
using ViewModels.Locator;

namespace ViewModels.Contents
{
	public class MainViewModel : ContentViewModel
	{
		private RelayCommand _goNextCommand;

		public ICommand GoNextCommand => this._goNextCommand ?? (this._goNextCommand = new RelayCommand(this.GoNext));

		private void GoNext(object obj)
		{
			var next = ViewModelLocator.GetViewModel<NextViewModel>();
			var container = ContainerResolver.GetContainer();
			var windowService = container.Resolve<IWindowService>();
			windowService.ChangeContent(this, next);
		}
	}
}
