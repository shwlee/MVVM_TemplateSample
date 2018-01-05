using System;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace UICommon.Utils
{
	public static class ScreenUtil
	{
		private static Point _dpiPoint;

		private static IntPtr _previousLParam;

		static ScreenUtil()
		{
			// 현재 모니터의 Dpi 값을 찾아 보관한다.
			var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
			var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

			var dpiX = (int)dpiXProperty.GetValue(null, null) / 96.0;
			var dpiY = (int)dpiYProperty.GetValue(null, null) / 96.0;

			_dpiPoint = new Point(dpiX, dpiY);
		}

		public static Point GetCurrentMonitorDpi()
		{
			return _dpiPoint;
		}

		public static Rect GetPrimaryScreenRegion()
		{
			return new Rect(Screen.PrimaryScreen.Bounds.Left,
				Screen.PrimaryScreen.Bounds.Top,
				Screen.PrimaryScreen.Bounds.Width,
				Screen.PrimaryScreen.Bounds.Height);
		}

		/// <summary>
		/// 전체 화면 영역을 구하는 함수
		/// </summary>
		/// <returns>전체 화면 영역을 나타내는 값이 반환된다.</returns>
		public static Rect GetTotalScreenBound()
		{
			var resultRect = new Rect();
			var dpiX = _dpiPoint.X;
			var dpiY = _dpiPoint.Y;

			foreach (var screen in Screen.AllScreens)
			{
				resultRect.Union(
					new Rect(
						screen.Bounds.X / dpiX,
						screen.Bounds.Y / dpiY,
						screen.Bounds.Width / dpiX,
						screen.Bounds.Height / dpiY));
			}

			return resultRect;
		}
	}
}
