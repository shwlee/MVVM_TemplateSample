using Define.Interfaces.CreateInstances;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Services.CreateInstances
{
	public class InstanceCreator : IInstanceCreate
	{
		public object Create(Type type)
		{
			//return this.CreateInstanceByReflection(type);
			//return this.CreateInstanceByActivator(type);
			//return this.CreateInstanceByExpression(type);
			return this.CreateInstanceByLCG(type);
		}

		public T Create<T>() where T : class
		{
			var type = typeof (T);
			if (type.IsInterface)
			{
				throw new ArgumentException("This generic type argument is interface. It doesn't allowed interface type in generic type argument.");
			}

			return this.Create(type) as T;
		}

		private object CreateInstanceByReflection(Type type)
		{
			var constrcutor = type.GetConstructor(Type.EmptyTypes);
			return constrcutor?.Invoke(null);
		}

		private object CreateInstanceByActivator(Type type)
		{
			return Activator.CreateInstance(type);
		}

		private object CreateInstanceByExpression(Type type)
		{
			var creator = Expression.Lambda<Func<object>>(Expression.New(type.GetConstructor(Type.EmptyTypes))).Compile();

			return creator();
		}

		private object CreateInstanceByLCG(Type type)
		{
			var emptyConstructor = type.GetConstructor(Type.EmptyTypes);
			var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
			ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
			ilGenerator.Emit(OpCodes.Nop);
			ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
			ilGenerator.Emit(OpCodes.Ret);
			var creator = dynamicMethod.CreateDelegate(typeof(Func<object>));

			return creator.DynamicInvoke();
		}
	}
}
