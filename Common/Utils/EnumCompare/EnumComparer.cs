using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Utils.EnumCompare
{
	// Dictionary<TKey, TValue> TKey에 enum 사용 시 순회 시 compare할 때마다 GC 를 일으킴.
	// Enum 을 Key 로 사용할 때 GC를 발생시키지 않기 위해 comparer 사용.

	public static class EnumComparer
	{
		public static EnumComparer<TEnum> For<TEnum>()
			where TEnum : struct, IComparable, IConvertible, IFormattable
		{
			return EnumComparer<TEnum>.Instance;
		}
	}

	/// <summary>
	/// A fast and efficient implementation of <see cref="IEqualityComparer{T}"/> for Enum types.
	/// Useful for dictionaries that use Enums as their keys.
	/// </summary>
	/// <example>
	/// <code>
	/// var dict = new Dictionary&lt;DayOfWeek, string&gt;(EnumComparer&lt;DayOfWeek&gt;.Instance);
	/// </code>
	/// </example>
	/// <typeparam name="TEnum">The type of the Enum.</typeparam>
	public sealed class EnumComparer<TEnum> : IEqualityComparer<TEnum>
		where TEnum : struct, IComparable, IConvertible, IFormattable
	{
		private static readonly Func<TEnum, TEnum, bool> _equals;

		private static readonly Func<TEnum, int> _getHashCode;

		/// <summary>
		/// The singleton accessor.
		/// </summary>
		public static readonly EnumComparer<TEnum> Instance;


		/// <summary>
		/// Initializes the <see cref="EnumComparer{TEnum}"/> class
		/// by generating the GetHashCode and Equals methods.
		/// </summary>
		static EnumComparer()
		{
			_getHashCode = GenerateGetHashCode();
			_equals = GenerateEquals();
			Instance = new EnumComparer<TEnum>();
		}

		/// <summary>
		/// A private constructor to prevent user instantiation.
		/// </summary>
		private EnumComparer()
		{
			AssertTypeIsEnum();
			AssertUnderlyingTypeIsSupported();
		}

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <typeparamref name="TEnum"/> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="TEnum"/> to compare.</param>
		/// <returns>
		/// true if the specified objects are equal; otherwise, false.
		/// </returns>
		public bool Equals(TEnum x, TEnum y)
		{
			// call the generated method
			return _equals(x, y);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
		/// </exception>
		public int GetHashCode(TEnum obj)
		{
			// call the generated method
			return _getHashCode(obj);
		}

		private static void AssertTypeIsEnum()
		{
			if (typeof(TEnum).IsEnum)
			{
				return;
			}

			var message = $"The type parameter {typeof(TEnum)} is not an Enum. LcgEnumComparer supports Enums only.";
			throw new NotSupportedException(message);
		}

		private static void AssertUnderlyingTypeIsSupported()
		{
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			var supportedTypes =
				new[]{
						typeof (byte), typeof (sbyte), typeof (short), typeof (ushort),
						typeof (int), typeof (uint), typeof (long), typeof (ulong)
					};

			if (supportedTypes.Contains(underlyingType))
			{
				return;
			}

			var message =
				$"The underlying type of the type parameter {typeof(TEnum)} is {underlyingType}. " +
				"LcgEnumComparer only supports Enums with underlying type of " +
				"byte, sbyte, short, ushort, int, uint, long, or ulong.";
			throw new NotSupportedException(message);
		}

		/// <summary>
		/// Generates a comparison method similiar to this:
		/// <code>
		/// bool Equals(TEnum x, TEnum y)
		/// {
		///     return x == y;
		/// }
		/// </code>
		/// </summary>
		/// <returns>The generated method.</returns>
		private static Func<TEnum, TEnum, bool> GenerateEquals()
		{
			var xParam = Expression.Parameter(typeof(TEnum), "x");
			var yParam = Expression.Parameter(typeof(TEnum), "y");
			var equalExpression = Expression.Equal(xParam, yParam);
			return Expression.Lambda<Func<TEnum, TEnum, bool>>(equalExpression, new[] { xParam, yParam }).Compile();
		}

		/// <summary>
		/// Generates a GetHashCode method similar to this:
		/// <code>
		/// int GetHashCode(TEnum obj)
		/// {
		///     return ((int)obj).GetHashCode();
		/// }
		/// </code>
		/// </summary>
		/// <returns>The generated method.</returns>
		private static Func<TEnum, int> GenerateGetHashCode()
		{
			var objParam = Expression.Parameter(typeof(TEnum), "obj");
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			var convertExpression = Expression.Convert(objParam, underlyingType);
			var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
			return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, new[] { objParam }).Compile();
		}
	}
}
