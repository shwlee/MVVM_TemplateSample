using System;
using System.Linq;
using Define.Properties;

namespace Define.EventAggregators
{
	public class PubSubEvent<TPayload> : EventBase
	{
		public SubscriptionToken Subscribe(Action<TPayload> action)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
		{
			return this.Subscribe(action, threadOption, false);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
		}

		public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
		{
			var actionReference = (IDelegateReference)new DelegateReference(action, keepSubscriberReferenceAlive);
			var filterReference = filter == null ? (IDelegateReference)new DelegateReference((Predicate<TPayload>)(predicate => true), true) : new DelegateReference(filter, keepSubscriberReferenceAlive);

			EventSubscription<TPayload> eventSubscription;
			switch (threadOption)
			{
				case ThreadOption.PublisherThread:
					eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
					break;
				case ThreadOption.UIThread:
					if (this.SynchronizationContext == null)
					{
						throw new InvalidOperationException(Resources.EventAggregatorNotConstructedOnUIThread);
					}

					eventSubscription = new DispatcherEventSubscription<TPayload>(actionReference, filterReference, this.SynchronizationContext);
					break;
				case ThreadOption.BackgroundThread:
					eventSubscription = new BackgroundEventSubscription<TPayload>(actionReference, filterReference);
					break;
				default:
					eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
					break;
			}

			return this.InternalSubscribe(eventSubscription);
		}

		public virtual void Publish(TPayload payload)
		{
			this.InternalPublish((object)payload);
		}

		public virtual void Unsubscribe(Action<TPayload> subscriber)
		{
			lock (this.Subscriptions)
			{
				var subscription = (IEventSubscription)this.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
				if (subscription == null)
				{
					return;
				}

				this.Subscriptions.Remove(subscription);
			}
		}

		public virtual bool Contains(Action<TPayload> subscriber)
		{
			IEventSubscription eventSubscription;
			lock (this.Subscriptions)
			{
				eventSubscription = this.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
			}

			return eventSubscription != null;
		}
	}
}
