
namespace UICommon.MVVM.EventToCommandParts
{
	/// <summary>
	/// The definition of the converter used to convert an EventArgs
	/// in the <see cref="UICommon.MVVM.EventToCommandParts.EventToCommand"/> class, if the
	/// <see cref="UICommon.MVVM.EventToCommandParts.EventToCommand.PassEventArgsToCommand"/> property is true.
	/// Set an instance of this class to the <see cref="UICommon.MVVM.EventToCommandParts.EventToCommand.EventArgsConverter"/>
	/// property of the EventToCommand instance.
	/// </summary>
	public interface IEventArgsConverter
	{
		/// <summary>
		/// The method used to convert the EventArgs instance.
		/// </summary>
		/// <param name="value">An instance of EventArgs passed by the
		/// event that the EventToCommand instance is handling.</param>
		/// <param name="parameter">An optional parameter used for the conversion. Use
		/// the <see cref="UICommon.MVVM.EventToCommandParts.EventToCommand.EventArgsConverterParameter"/> property
		/// to set this value. This may be null.</param>
		/// <returns>The converted value.</returns>
		object Convert(object value, object parameter);
	}
}
