using System;

namespace Define.EventAggregators
{
	public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
	{
		private readonly Guid _token;

		private Action<SubscriptionToken> _unsubscribeAction;

		public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
		{
			this._unsubscribeAction = unsubscribeAction;
			this._token = Guid.NewGuid();
		}

		public bool Equals(SubscriptionToken other)
		{
			return other != null && Equals(this._token, other._token);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || this.Equals(obj as SubscriptionToken);
		}

		public override int GetHashCode()
		{
			return this._token.GetHashCode();
		}

		public virtual void Dispose()
		{
			if (this._unsubscribeAction != null)
			{
				this._unsubscribeAction(this);
				this._unsubscribeAction = null;
			}

			GC.SuppressFinalize(this);
		}
	}
}
