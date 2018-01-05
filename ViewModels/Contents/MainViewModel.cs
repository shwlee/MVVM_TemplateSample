
using System.ComponentModel;
using System.Windows.Input;
using Common.ServiceLocators;
using Common.Utils.RelayCommands;
using Define.Interfaces.WindowServices;

namespace ViewModels.Contents
{
	public class MainViewModel : ContentViewModel
	{
		private RelayCommand _goNextCommand;

		public ICommand GoNextCommand
		{
			get { return this._goNextCommand ?? (this._goNextCommand = new RelayCommand(this.GoNext)); }
		}

		private void GoNext(object obj)
		{
			var next = new NextViewModel();

			var container = ContainerResolver.GetContainer();
			var windowService = container.Resolve<IWindowService>();
			windowService.ChangeContent(this, next);
		}
	}
}
