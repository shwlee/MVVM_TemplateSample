using System;

namespace Define.EventAggregators
{
	public interface IDelegateReference
	{
		Delegate Target { get; }
	}
}
