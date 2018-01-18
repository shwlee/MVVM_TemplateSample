using System;

namespace Define.Interfaces.CreateInstances
{
	public interface IInstanceCreate
	{
		object Create(Type type, params object[] args);

		T Create<T>(params object[] args) where T : class;
	}
}
