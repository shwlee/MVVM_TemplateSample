using System.Windows;
using System.Windows.Input;

namespace UICommon.MVVM.EventToCommandParts
{
	public partial class EventToCommand
	{
		/// <summary>
		/// Identifies the <see cref="CommandParameter" /> dependency property
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
			"CommandParameter",
			typeof(object),
			typeof(EventToCommand),
			new PropertyMetadata(
				null,
				(s, e) =>
				{
					var sender = s as EventToCommand;

					if (sender?.AssociatedObject == null)
					{
						return;
					}

					sender.EnableDisableElement();
				}));

		/// <summary>
		/// Identifies the <see cref="Command" /> dependency property
		/// </summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command",
			typeof(ICommand),
			typeof(EventToCommand),
			new PropertyMetadata(
				null,
				(s, e) => EventToCommand.OnCommandChanged(s as EventToCommand, e)));

		/// <summary>
		/// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
		/// </summary>
		public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
			"MustToggleIsEnabled",
			typeof(bool),
			typeof(EventToCommand),
			new PropertyMetadata(
				false,
				(s, e) =>
				{
					var sender = s as EventToCommand;

					if (sender?.AssociatedObject == null)
					{
						return;
					}

					sender.EnableDisableElement();
				}));

		private object _commandParameterValue;

		private bool? _mustToggleValue;

		/// <summary>
		/// Gets or sets the ICommand that this trigger is bound to. This
		/// is a DependencyProperty.
		/// </summary>
		public ICommand Command
		{
			get
			{
				return (ICommand)this.GetValue(CommandProperty);
			}

			set
			{
				this.SetValue(CommandProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an object that will be passed to the <see cref="Command" />
		/// attached to this trigger. This is a DependencyProperty.
		/// </summary>
		public object CommandParameter
		{
			get
			{
				return this.GetValue(CommandParameterProperty);
			}

			set
			{
				this.SetValue(CommandParameterProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an object that will be passed to the <see cref="Command" />
		/// attached to this trigger. This property is here for compatibility
		/// with the Silverlight version. This is NOT a DependencyProperty.
		/// For databinding, use the <see cref="CommandParameter" /> property.
		/// </summary>
		public object CommandParameterValue
		{
			get
			{
				return this._commandParameterValue ?? this.CommandParameter;
			}

			set
			{
				this._commandParameterValue = value;
				this.EnableDisableElement();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the attached element must be
		/// disabled when the <see cref="Command" /> property's CanExecuteChanged
		/// event fires. If this property is true, and the command's CanExecute 
		/// method returns false, the element will be disabled. If this property
		/// is false, the element will not be disabled when the command's
		/// CanExecute method changes. This is a DependencyProperty.
		/// </summary>
		public bool MustToggleIsEnabled
		{
			get
			{
				return (bool)this.GetValue(MustToggleIsEnabledProperty);
			}

			set
			{
				this.SetValue(MustToggleIsEnabledProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the attached element must be
		/// disabled when the <see cref="Command" /> property's CanExecuteChanged
		/// event fires. If this property is true, and the command's CanExecute 
		/// method returns false, the element will be disabled. This property is here for
		/// compatibility with the Silverlight version. This is NOT a DependencyProperty.
		/// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
		/// </summary>
		public bool MustToggleIsEnabledValue
		{
			get
			{
				return this._mustToggleValue ?? this.MustToggleIsEnabled;
			}

			set
			{
				this._mustToggleValue = value;
				this.EnableDisableElement();
			}
		}

		/// <summary>
		/// Called when this trigger is attached to a DependencyObject.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.EnableDisableElement();
		}

		/// <summary>
		/// This method is here for compatibility
		/// with the WPF version.
		/// </summary>
		/// <returns>The object to which this trigger
		/// is attached casted as a FrameworkElement.</returns>
		private FrameworkElement GetAssociatedObject()
		{
			return this.AssociatedObject as FrameworkElement;
		}

		/// <summary>
		/// This method is here for compatibility
		/// with the WPF version.
		/// </summary>
		/// <returns>The command that must be executed when
		/// this trigger is invoked.</returns>
		private ICommand GetCommand()
		{
			return this.Command;
		}
	}
}
