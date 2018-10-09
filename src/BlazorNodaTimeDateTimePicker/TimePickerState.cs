using NodaTime;

namespace BlazorNodaTimeDateTimePicker
{
	internal class TimePickerState
    {
		internal TimePickerState()
		{
			Clock = SystemClock.Instance;
		}

		internal IClock Clock { get; }
		internal LocalTime SelectedTime { get; set; }
		internal bool Visible { get; set; }
		internal bool Inline { get; set; }

		internal bool IsAM => SelectedTime.Hour < 12;
		internal bool IsPM => SelectedTime.Hour >= 12;
	}
}
