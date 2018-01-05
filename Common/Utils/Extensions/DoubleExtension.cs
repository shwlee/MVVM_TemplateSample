using System;

namespace Common.Utils.Extensions
{
	public static class DoubleExtension
	{
		/// <summary>
		/// 두 double 값이 double 지수 오차 범위 내에서 같음을 확인한다.
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <returns></returns>
		public static bool CloseTo(this double x1, double x2)
		{
			return Math.Abs(x1 - x2) < 0.000000000000001;
		}
	}
}
