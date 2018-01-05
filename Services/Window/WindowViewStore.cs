using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define.Interfaces.WindowServices;

namespace Services.Window
{
	class WindowViewStore
	{
		internal IWindowView View { get; set; }

		internal IWindowContext Context { get; set; }
	}
}
