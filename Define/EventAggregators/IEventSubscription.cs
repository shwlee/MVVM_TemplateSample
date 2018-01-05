using System;

namespace Define.EventAggregators
{
	public interface IEventSubscription
	{
		SubscriptionToken SubscriptionToken { get; set; }

		Action<object[]> GetExecutionStrategy();
	}
}
