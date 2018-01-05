using System;
using System.Globalization;
using Define.Properties;

namespace Define.EventAggregators
{
	public class EventSubscription<TPayload> : IEventSubscription
	{
		private readonly IDelegateReference _actionReference;
		private readonly IDelegateReference _filterReference;

		public Action<TPayload> Action => (Action<TPayload>)this._actionReference.Target;

		public Predicate<TPayload> Filter => (Predicate<TPayload>)this._filterReference.Target;

		public SubscriptionToken SubscriptionToken { get; set; }

		public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
		{
			if (actionReference == null)
			{
				throw new ArgumentNullException("actionReference");
			}

			if (actionReference.Target is Action<TPayload> == false)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, typeof(Action<TPayload>).FullName), "actionReference");
			}

			if (filterReference == null)
			{
				throw new ArgumentNullException("filterReference");
			}

			if (filterReference.Target is Predicate<TPayload> == false)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, typeof(Predicate<TPayload>).FullName), "filterReference");
			}

			this._actionReference = actionReference;
			this._filterReference = filterReference;
		}

		public virtual Action<object[]> GetExecutionStrategy()
		{
			var action = this.Action;
			var filter = this.Filter;
			if (action != null && filter != null)
			{
				return arguments =>
				{
					var payload = default(TPayload);
					if (arguments != null && arguments.Length > 0 && arguments[0] != null)
					{
						payload = (TPayload)arguments[0];
					}

					if (filter(payload) == false)
					{
						return;
					}

					this.InvokeAction(action, payload);
				};
			}

			return null;
		}

		public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			action(argument);
		}
	}
}
