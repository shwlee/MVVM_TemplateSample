using Common.Utils.Extensions;
using Define.Enums;
using Define.Interfaces.WindowServices;
using System;
using System.Windows;

namespace Views.Windows
{
	public abstract class BaseWindow : Window, IWindowView
	{
		private SizeToContent _beforeMaximizedSizeToContent;

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this.IsClosed = true;
		}

		public void SetOwner(IWindowView windowView)
		{
			if (windowView is Window == false)
			{
				return;
			}

			var ownerWindow = (Window)windowView;
			this.Owner = ownerWindow;
		}

		public void Initialize(IWindowSettings settings)
		{
			this.ResizeMode = settings.ResizeMode.GetEnum<ResizeMode>();
			this.SizeToContent = settings.SizeToContent.GetEnum<SizeToContent>();
			this.WindowState = settings.State.GetEnum<WindowState>();
		}

		public void MoveWindow()
		{
			this.DragMove();
		}

		public void ChangeWindowState(WindowViewStates states)
		{
			var toState = states.GetEnum<WindowState>();
			switch (toState)
			{
				case WindowState.Maximized:
					this._beforeMaximizedSizeToContent = this.SizeToContent;
					this.SizeToContent = SizeToContent.Manual;
					break;
				case WindowState.Normal:
				case WindowState.Minimized:
					this.SizeToContent = this._beforeMaximizedSizeToContent;
					break;
			}

			this.WindowState = toState;
		}

		public void ChangeSizeToContent(WindowSizeToContents sizeToContent)
		{
			this.SizeToContent = sizeToContent.GetEnum<SizeToContent>();
		}

		public bool IsClosed { get; set; }
	}
}
