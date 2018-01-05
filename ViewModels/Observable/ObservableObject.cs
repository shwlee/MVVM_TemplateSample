using PropertyChanged;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels.Observable
{
	[ImplementPropertyChanged]
	public class ObservableObject : INotifyPropertyChanged, IDisposable
	{
		private bool _disposed;

		public event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.
		// The CallerMemberName attribute that is applied to the optional propertyName
		// parameter causes the property name of the caller to be substituted as an argument.
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			this.FireNotification(this, propertyName);
		}

		protected void Set(object sender, [CallerMemberName] string propertyName = "")
		{
			this.FireNotification(sender, propertyName);
		}

		private void FireNotification(object sender, string propertyName)
		{
			this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
		}

		#region IDisposable implement

		~ObservableObject()
		{
			this.DoDispose(false);
		}

		public void Dispose()
		{
			this.DoDispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void DoDispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}

			if (disposing)
			{
				// cleanup
			}

			this._disposed = true;
		}

		#endregion
	}
}
