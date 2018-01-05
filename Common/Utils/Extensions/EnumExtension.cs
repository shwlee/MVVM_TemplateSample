using System;

namespace Common.Utils.Extensions
{
	public static class EnumExtension
	{
		public static T GetEnum<T>(this object target) where T : struct
		{
			// TODO : need validtion check.

			return (T)Enum.Parse(typeof(T), target.ToString(), true);
		}
	}
}
