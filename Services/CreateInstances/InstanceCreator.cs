using Define.Interfaces.CreateInstances;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Services.CreateInstances
{
	public class InstanceCreator : IInstanceCreate
	{
		static readonly Dictionary<int, Type> GenericFunctions = new Dictionary<int, Type>
		{
			{0, typeof(Func<>)},
			{1, typeof(Func<,>)},
			{2, typeof(Func<,,>)},
			{3, typeof(Func<,,,>)},
			{4, typeof(Func<,,,,>)},
			{5, typeof(Func<,,,,,>)},
			{6, typeof(Func<,,,,,,>)},
			{7, typeof(Func<,,,,,,,>)},
			{8, typeof(Func<,,,,,,,,>)},
			{9, typeof(Func<,,,,,,,,,>)},
			{10, typeof(Func<,,,,,,,,,,>)},
			{11, typeof(Func<,,,,,,,,,,,>)},
			{12, typeof(Func<,,,,,,,,,,,,>)},
			{13, typeof(Func<,,,,,,,,,,,,,>)},
			{14, typeof(Func<,,,,,,,,,,,,,,>)},
			{15, typeof(Func<,,,,,,,,,,,,,,,>)},
		};

		private BindingFlags _constructorInvokeFlags =
			BindingFlags.OptionalParamBinding |
			BindingFlags.InvokeMethod |
			BindingFlags.CreateInstance;

		private BindingFlags _getConstructorFlags = BindingFlags.Instance | BindingFlags.Public;
		
		public object Create(Type type, params object[] args)
		{
			//return this.CreateInstanceByReflection(type, args);
			//return this.CreateInstanceByActivator(type, args);
			//return this.CreateInstanceByExpression(type, args);
			return this.CreateInstanceByLCG(type, args);
		}

		public T Create<T>(params object[] args) where T : class
		{
			var type = typeof(T);
			if (type.IsInterface)
			{
				throw new ArgumentException("This generic type argument is interface. It doesn't allowed interface type in generic type argument.");
			}

			return this.Create(type, args) as T;
		}

		private object CreateInstanceByReflection(Type type, params object[] args)
		{
			if (args == null || args.Length == 0)
			{
				var parameterlessConstrcutor = type.GetConstructor(Type.EmptyTypes);
				return parameterlessConstrcutor?.Invoke(null);
			}

			var paramTypes = args.Select(a => a.GetType()).ToArray();

			var constrcutor = type.GetConstructor(this._getConstructorFlags, null, CallingConventions.HasThis, paramTypes, null);

			return constrcutor?.Invoke(this._constructorInvokeFlags, null, args, CultureInfo.InvariantCulture);
		}

		private object CreateInstanceByActivator(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, args);
		}

		private object CreateInstanceByExpression(Type type, params object[] args)
		{
			if (args == null || args.Length == 0)
			{
				var parameterlessConstrcutor = type.GetConstructor(Type.EmptyTypes);
				var parameterlessCreator = Expression.Lambda(Expression.New(parameterlessConstrcutor)).Compile();
				return parameterlessCreator.DynamicInvoke();
			}

			var paramTypes = args.Select(a => a.GetType()).ToArray();

			var constrcutor = type.GetConstructor(this._getConstructorFlags, null, CallingConventions.HasThis, paramTypes, null);
			if (constrcutor == null)
			{
				throw new NullReferenceException();
			}
			
			var parapeters = paramTypes.Select(Expression.Parameter).ToArray();
			var con = paramTypes.Concat(new Type[] {typeof(object)}).ToArray();
			var cachedFuncType = GenericFunctions[args.Length];
			var funcType = cachedFuncType.MakeGenericType(con);
			var lambda = Expression.Lambda(funcType, Expression.New(constrcutor, parapeters), parapeters);
			var creator = lambda.Compile();

			return creator.DynamicInvoke(args);
		}

		private object CreateInstanceByLCG(Type type, params object[] args)
		{
			ConstructorInfo constructor;
			Type[] paramTypes = null;
			var paramCount = 0;

			var isParameterless = args.Length == 0;
			if (isParameterless)
			{
				constructor = type.GetConstructor(Type.EmptyTypes);
			}
			else
			{
				paramTypes = args.Select(a => a.GetType()).ToArray();

				constructor = type.GetConstructor(this._getConstructorFlags, null, CallingConventions.HasThis, paramTypes, null);
				if (constructor == null)
				{
					throw new NullReferenceException();
				}

				paramCount = args.Length;
			}
			
			var dynamicMethod = new DynamicMethod(".ctor", type, paramTypes, true);
			ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
			ilGenerator.Emit(OpCodes.Nop);
			
			if (isParameterless == false)
			{
				// Load `this` and call base constructor with arguments
				ilGenerator.Emit(OpCodes.Ldarg_0);
				for (var i = 1; i < paramCount; ++i)
				{
					ilGenerator.Emit(OpCodes.Ldarg, i);
				}
			}

			ilGenerator.Emit(OpCodes.Newobj, constructor);
			ilGenerator.Emit(OpCodes.Ret);
			
			var funcType = isParameterless ? 
				typeof (Func<object>) : 
				GenericFunctions[paramCount].MakeGenericType(paramTypes.Concat(new[] { typeof(object) }).ToArray());

			var creator = dynamicMethod.CreateDelegate(funcType);

			return creator.DynamicInvoke(args);
		}
	}
}
