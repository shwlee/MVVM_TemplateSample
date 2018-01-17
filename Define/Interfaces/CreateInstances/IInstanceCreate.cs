using System;

namespace Define.Interfaces.CreateInstances
{
	public interface IInstanceCreate
	{
		object Create(Type type);

		T Create<T>() where T : class;
	}
}
