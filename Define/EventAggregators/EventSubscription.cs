using System;
using System.Globalization;
using Define.Properties;

namespace Define.EventAggregators
{
	public class EventSubscription : IEventSubscription
	{
		private readonly IDelegateReference _actionReference;

		///<summary>
		/// Creates a new instance of <see cref="EventSubscription"/>.
		///</summary>
		///<param name="actionReference">A reference to a delegate of type <see cref="System.Action"/>.</param>
		///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
		///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action"/>.</exception>
		public EventSubscription(IDelegateReference actionReference)
		{
			if (actionReference == null)
			{
				throw new ArgumentNullException(nameof(actionReference));
			}

			if (actionReference.Target is Action == false)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, typeof(Action).FullName), nameof(actionReference));
			}

			this._actionReference = actionReference;
		}

		/// <summary>
		/// Gets the target <see cref="System.Action"/> that is referenced by the <see cref="IDelegateReference"/>.
		/// </summary>
		/// <value>An <see cref="System.Action"/> or <see langword="null" /> if the referenced target is not alive.</value>
		public Action Action => (Action)this._actionReference.Target;

		/// <summary>
		/// Gets or sets a <see cref="SubscriptionToken"/> that identifies this <see cref="IEventSubscription"/>.
		/// </summary>
		/// <value>A token that identifies this <see cref="IEventSubscription"/>.</value>
		public SubscriptionToken SubscriptionToken { get; set; }

		/// <summary>
		/// Gets the execution strategy to publish this event.
		/// </summary>
		/// <returns>An <see cref="System.Action"/> with the execution strategy, or <see langword="null" /> if the <see cref="IEventSubscription"/> is no longer valid.</returns>
		/// <remarks>
		/// If <see cref="Action"/>is no longer valid because it was
		/// garbage collected, this method will return <see langword="null" />.
		/// Otherwise it will return a delegate that evaluates the <see cref="Filter"/> and if it
		/// returns <see langword="true" /> will then call <see cref="InvokeAction"/>. The returned
		/// delegate holds a hard reference to the <see cref="Action"/> target
		/// <see cref="Delegate">delegates</see>. As long as the returned delegate is not garbage collected,
		/// the <see cref="Action"/> references delegates won't get collected either.
		/// </remarks>
		public virtual Action<object[]> GetExecutionStrategy()
		{
			var action = this.Action;
			if (action != null)
			{
				return arguments =>
				{
					this.InvokeAction(action);
				};
			}
			return null;
		}

		/// <summary>
		/// Invokes the specified <see cref="System.Action{TPayload}"/> synchronously when not overridden.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="action"/> is null.</exception>
		public virtual void InvokeAction(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			action();
		}
	}
}
