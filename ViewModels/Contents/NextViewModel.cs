using Common.ServiceLocators;
using Common.Utils.RelayCommands;
using Define.Interfaces.WindowServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Locator;

namespace ViewModels.Contents
{
	public class NextViewModel : ContentViewModel
	{
		private RelayCommand _goMainCommand;

		public ICommand GoMainCommand => this._goMainCommand ?? (this._goMainCommand = new RelayCommand(this.GoMain));

		private void GoMain(object obj)
		{
			var container = ContainerResolver.GetContainer();
			var windowService = container.Resolve<IWindowService>();
			var main= ViewModelLocator.GetViewModel<MainViewModel>();
			windowService.ChangeContent(this, main);
		}
	}
}
