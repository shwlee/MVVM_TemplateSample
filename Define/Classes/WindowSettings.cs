using Define.Enums;
using Define.Interfaces.WindowServices;

namespace Define.Classes
{
	public class WindowSettings : IWindowSettings
	{
		public WindowSettings(
			string baseTitle, 
			WindowStyles style, 
			WindowResizeModes resizeMode,
			WindowSizeToContents sizeToContent, 
			WindowViewStates state)
		{
			this.BaseTitle = baseTitle;
			this.Style = style;
			this.ResizeMode = resizeMode;
			this.SizeToContent = sizeToContent;
			this.State = state;
		}

		public string BaseTitle { get; }

		public WindowStyles Style { get; }

		public WindowResizeModes ResizeMode { get; }

		public WindowSizeToContents SizeToContent { get; }

		public WindowViewStates State { get; }
	}
}
