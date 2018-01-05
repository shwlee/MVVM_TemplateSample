using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace UICommon.MVVM.EventToCommandParts
{
	/// <summary>
	/// This <see cref="System.Windows.Interactivity.TriggerAction" /> can be
	/// used to bind any event on any FrameworkElement to an <see cref="ICommand" />.
	/// Typically, this element is used in XAML to connect the attached element
	/// to a command located in a ViewModel. This trigger can only be attached
	/// to a FrameworkElement or a class deriving from FrameworkElement.
	/// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
	/// and leave the CommandParameter and CommandParameterValue empty!</para>
	/// </summary>
	public partial class EventToCommand : TriggerAction<DependencyObject>
	{
		/// <summary>
		/// Gets or sets a value indicating whether the EventArgs passed to the
		/// event handler will be forwarded to the ICommand's Execute method
		/// when the event is fired (if the bound ICommand accepts an argument
		/// of type EventArgs).
		/// <para>For example, use a RelayCommand&lt;MouseEventArgs&gt; to get
		/// the arguments of a MouseMove event.</para>
		/// </summary>
		public bool PassEventArgsToCommand { get; set; }

		/// <summary>
		/// Gets or sets a converter used to convert the EventArgs when using
		/// <see cref="PassEventArgsToCommand"/>. If PassEventArgsToCommand is false,
		/// this property is never used.
		/// </summary>
		public IEventArgsConverter EventArgsConverter { get; set; }

		/// <summary>
		/// The <see cref="EventArgsConverterParameter" /> dependency property's name.
		/// </summary>
		public const string EventArgsConverterParameterPropertyName = "EventArgsConverterParameter";

		/// <summary>
		/// Gets or sets a parameters for the converter used to convert the EventArgs when using
		/// <see cref="PassEventArgsToCommand"/>. If PassEventArgsToCommand is false,
		/// this property is never used. This is a dependency property.
		/// </summary>
		public object EventArgsConverterParameter
		{
			get
			{
				return this.GetValue(EventArgsConverterParameterProperty);
			}
			set
			{
				this.SetValue(EventArgsConverterParameterProperty, value);
			}
		}

		/// <summary>
		/// Identifies the <see cref="EventArgsConverterParameter" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register(
			EventArgsConverterParameterPropertyName,
			typeof(object),
			typeof(global::UICommon.MVVM.EventToCommandParts.EventToCommand),
			new UIPropertyMetadata(null));

		/// <summary>
		/// Provides a simple way to invoke this trigger programatically
		/// without any EventArgs.
		/// </summary>
		public void Invoke()
		{
			this.Invoke(null);
		}

		/// <summary>
		/// Executes the trigger.
		/// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
		/// and leave the CommandParameter and CommandParameterValue empty!</para>
		/// </summary>
		/// <param name="parameter">The EventArgs of the fired event.</param>
		protected override void Invoke(object parameter)
		{
			if (this.AssociatedElementIsDisabled())
			{
				return;
			}

			var command = this.GetCommand();
			var commandParameter = this.CommandParameterValue;

			if (commandParameter == null && this.PassEventArgsToCommand)
			{
				commandParameter = this.EventArgsConverter == null
					? parameter
					: this.EventArgsConverter.Convert(parameter, this.EventArgsConverterParameter);
			}

			if (command != null && command.CanExecute(commandParameter))
			{
				command.Execute(commandParameter);
			}
		}

		private static void OnCommandChanged(global::UICommon.MVVM.EventToCommandParts.EventToCommand element, DependencyPropertyChangedEventArgs e)
		{
			if (element == null)
			{
				return;
			}

			if (e.OldValue != null)
			{
				((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
			}

			var command = (ICommand)e.NewValue;
			if (command != null)
			{
				command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
			}

			element.EnableDisableElement();
		}

		private bool AssociatedElementIsDisabled()
		{
			var element = this.GetAssociatedObject();
			return this.AssociatedObject == null || (element != null && !element.IsEnabled);
		}

		private void EnableDisableElement()
		{
			var element = this.GetAssociatedObject();

			if (element == null)
			{
				return;
			}

			var command = this.GetCommand();

			if (this.MustToggleIsEnabledValue && command != null)
			{
				element.IsEnabled = command.CanExecute(this.CommandParameterValue);
			}
		}

		private void OnCommandCanExecuteChanged(object sender, EventArgs e)
		{
			this.EnableDisableElement();
		}
	}
}
