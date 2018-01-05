using System;
using System.Collections.Generic;
using System.Threading;

namespace Define.EventAggregators
{
	public class EventAggregator : IEventAggregator
	{
		private readonly Dictionary<Type, EventBase> _events = new Dictionary<Type, EventBase>();

		private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;
		
		public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
		{
			lock (this._events)
			{
				EventBase eventBase;
				if (this._events.TryGetValue(typeof (TEventType), out eventBase))
				{
					return (TEventType)eventBase;
				}

				var eventType = Activator.CreateInstance<TEventType>();
				eventType.SynchronizationContext = this._syncContext;
				this._events[typeof(TEventType)] = eventType;

				return eventType;
			}
		}
	}
}
