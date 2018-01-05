using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define.Enums;
using Define.Interfaces.WindowServices;
using PropertyChanged;
using ViewModels.Base;

namespace ViewModels.Contents
{
	/// <summary>
	/// Window의 1차 Child. 여기서 Window의 Setting을 조정한다. Window의 Content가 되는 ViewModel은 모두 여기서 파생된다.
	/// </summary>
	public abstract class ContentViewModel : ViewModelBase, IWindowContent
	{
		public virtual void Close()
		{
		}
	}
}
