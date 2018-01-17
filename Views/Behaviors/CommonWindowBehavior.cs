using Define.Enums;
using Define.Interfaces.WindowServices;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shell;
using System.Windows.Threading;
using Common.Utils.Extensions;
using UICommon.Utils;
using Views.Windows;

namespace Views.Behaviors
{
	public class CommonWindowBehavior : Behavior<CommonWindow>
	{
		#region Fields

		private bool _restoreWindowState;

		#endregion

		#region Methods

		#region Behaviors

		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.Loaded += this.AssociatedObjectOnLoaded;
			this.AssociatedObject.ContentRendered += this.AssociatedObjectOnContentRendered;
			this.AssociatedObject.StateChanged += this.AssociatedObjectOnStateChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.xHeaderPanel.PreviewMouseLeftButtonDown -= this.HeaderGridOnMouseLeftButtonDown;
			this.AssociatedObject.xHeaderPanel.MouseMove -= this.HeaderPanelOnMouseMove;
			this.AssociatedObject.ContentRendered -= this.AssociatedObjectOnContentRendered;
			this.AssociatedObject.StateChanged -= this.AssociatedObjectOnStateChanged;
		}

		#endregion

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
		{
			this.AssociatedObject.xHeaderPanel.MouseLeftButtonDown += this.HeaderGridOnMouseLeftButtonDown;
		}

		private void AssociatedObjectOnContentRendered(object sender, EventArgs e)
		{
			// WindowChrome 제거 시 SizeToContent가 정상동작 하지 않는 현상 수정.

			if (this.AssociatedObject.SizeToContent == SizeToContent.Manual)
			{
				return;
			}
			
			this.AssociatedObject.SizeToContent = SizeToContent.Manual;
		}

		private void AssociatedObjectOnStateChanged(object sender, EventArgs e)
		{
			var currentState = this.AssociatedObject.WindowState;
			switch (currentState)
			{
				case WindowState.Maximized:
					this.AssociatedObject.xHeaderPanel.MouseMove += this.HeaderPanelOnMouseMove;
					this._restoreWindowState = true;
					this.AssociatedObject.ControlWindowChrome(false);
					break;
				case WindowState.Normal:
				case WindowState.Minimized:
					this.AssociatedObject.xHeaderPanel.MouseMove -= this.HeaderPanelOnMouseMove;
					this.AssociatedObject.ControlWindowChrome(true);
					break;
			}
		}

		private void HeaderPanelOnMouseMove(object sender, MouseEventArgs e)
		{
			if (this._restoreWindowState == false)
			{
				return;
			}

			if (e.LeftButton == MouseButtonState.Released)
			{
				return;
			}

			this._restoreWindowState = false;

			var mousePosition = e.GetPosition(this.AssociatedObject);
			this.SetNormalPositionFromMaximized(mousePosition);
			this.AssociatedObject.DragMove();
		}

		private void SetNormalPositionFromMaximized(Point mousePoint)
		{
			var mouseScreenPoint = this.AssociatedObject.PointToScreen(mousePoint);
			var prevWidth = this.AssociatedObject.ActualWidth;

			// Normal 상태에서 Mouse Pointer의 적절한 위치를 지정하기 위한 Left,Top 설정.
			this.AssociatedObject.Top = mouseScreenPoint.Y - mousePoint.Y;
			this.AssociatedObject.WindowState = WindowState.Normal;

			// ActualWidth, ActualHeight는 이제 Normal의 Size를 가리킴.
			var bound = ScreenUtil.GetTotalScreenBound();

			var moveLeft = (mouseScreenPoint.X - mousePoint.X / prevWidth * this.AssociatedObject.ActualWidth) / ScreenUtil.GetCurrentMonitorDpi().X;

			// Screen 좌우 경계를 넘어가면 좌우 경계에 닿는 값을 Left로 지정.
			if (moveLeft < bound.Left)
			{
				moveLeft = bound.Left;
			}
			else if (moveLeft + this.AssociatedObject.ActualWidth > bound.Right)
			{
				moveLeft = bound.Right - this.AssociatedObject.ActualWidth;
			}

			this.AssociatedObject.Left = moveLeft;
		}

		private void HeaderGridOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var currentState = this.AssociatedObject.WindowState;

			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
			{
				this.AssociatedObject.WindowState = currentState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
				return;
			}

			this.AssociatedObject.DragMove();
		}

		#endregion
	}
}
