using Common.Utils.Extensions;
using Define.Enums;
using Define.Interfaces.WindowServices;
using System;
using System.Windows;
using System.Windows.Shell;

namespace Views.Windows
{
	public abstract class BaseWindow : Window, IWindowView
	{
		private SizeToContent _beforeMaximizedSizeToContent;

		private WindowChrome _chrome;

		public IWindowSettings Settings { get; private set; }

		public bool IsClosed { get; set; }

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
			this.Settings = settings;

			this.ResizeMode = settings.ResizeMode.GetEnum<ResizeMode>();
			this.SizeToContent = settings.SizeToContent.GetEnum<SizeToContent>();
			this.WindowState = settings.State.GetEnum<WindowState>();

			this._chrome = new WindowChrome
			{
				CaptionHeight = 0,
				CornerRadius = new CornerRadius(),
				GlassFrameThickness = new Thickness(),
				ResizeBorderThickness = new Thickness(5)
			};

			WindowChrome.SetWindowChrome(this, this._chrome);
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

		internal void ControlWindowChrome(bool isApplyShell)
		{
			WindowChrome.SetWindowChrome(this, isApplyShell ? this._chrome : null);
		}
	}
}
