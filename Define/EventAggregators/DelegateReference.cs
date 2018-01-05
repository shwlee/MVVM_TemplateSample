using System;
using System.Reflection;

namespace Define.EventAggregators
{
	public class DelegateReference : IDelegateReference
	{
		private readonly Delegate _reference;

		private readonly WeakReference _weakReference;

		private readonly MethodInfo _method;

		private readonly Type _delegateType;


		public Delegate Target => this._reference ?? this.TryGetDelegate();

		public DelegateReference(Delegate reference, bool keepReferenceAlive)
		{
			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}

			if (keepReferenceAlive)
			{
				this._reference = reference;
			}
			else
			{
				this._weakReference = new WeakReference(reference.Target);
				this._method = reference.Method;
				this._delegateType = reference.GetType();
			}
		}

		private Delegate TryGetDelegate()
		{
			if (this._method.IsStatic)
			{
				return Delegate.CreateDelegate(this._delegateType, null, this._method);
			}

			var target = this._weakReference.Target;
			return target != null ? Delegate.CreateDelegate(this._delegateType, target, this._method) : null;
		}
	}
}
