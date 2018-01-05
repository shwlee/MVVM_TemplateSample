using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Define.EventAggregators
{
	public abstract class EventBase
	{
		private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

		public SynchronizationContext SynchronizationContext { get; set; }

		protected ICollection<IEventSubscription> Subscriptions => this._subscriptions;

		protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
		{
			if (eventSubscription == null)
			{
				throw new ArgumentNullException("eventSubscription");
			}

			eventSubscription.SubscriptionToken = new SubscriptionToken(this.Unsubscribe);
			lock (this.Subscriptions)
			{
				this.Subscriptions.Add(eventSubscription);
			}

			return eventSubscription.SubscriptionToken;
		}

		protected virtual void InternalPublish(params object[] arguments)
		{
			foreach (var action in this.PruneAndReturnStrategies())
			{
				action(arguments);
			}
		}

		public virtual void Unsubscribe(SubscriptionToken token)
		{
			lock (this.Subscriptions)
			{
				var subscription = this.Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
				if (subscription == null)
				{
					return;
				}

				this.Subscriptions.Remove(subscription);
			}
		}

		public virtual bool Contains(SubscriptionToken token)
		{
			lock (this.Subscriptions)
			{
				return this.Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token) != null;
			}
		}

		private List<Action<object[]>> PruneAndReturnStrategies()
		{
			var list = new List<Action<object[]>>();
			lock (this.Subscriptions)
			{
				for (var subscriptionCount = this.Subscriptions.Count - 1; subscriptionCount >= 0; --subscriptionCount)
				{
					var localAction = this._subscriptions[subscriptionCount].GetExecutionStrategy();
					if (localAction == null)
					{
						this._subscriptions.RemoveAt(subscriptionCount);
					}
					else
					{
						list.Add(localAction);
					}
				}
			}

			return list;
		}
	}
}
