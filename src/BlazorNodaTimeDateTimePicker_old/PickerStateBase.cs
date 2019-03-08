namespace BlazorNodaTimeDateTimePicker
{
	/// <summary>
	/// Base class for the state machines.
	/// </summary>
	public abstract class PickerStateBase
	{
		internal bool Visible { get; set; }
		internal bool Inline { get; set; }
	}
}
