using NodaTime;

namespace BlazorNodaTimeDateTimePicker
{
	public class TimePickerState : PickerStateBase
	{		
		internal LocalTime SelectedTime { get; set; }
		internal int MinuteStep { get; set; } = 5;

		internal bool IsAM => SelectedTime.Hour < 12;
		internal bool IsPM => SelectedTime.Hour >= 12;

		internal void IncrementHour()
		{
			SelectedTime = SelectedTime.PlusHours(1);
		}

		internal void DecrementHour()
		{
			SelectedTime = SelectedTime.PlusHours(-1);
		}

		internal void IncrementMinute()
		{
			SelectedTime = SelectedTime.PlusMinutes(MinuteStep);
		}

		internal void DecrementMinute()
		{
			SelectedTime = SelectedTime.PlusMinutes(-MinuteStep);
		}

		internal void SetAM()
		{
			if (IsPM)
			{
				SelectedTime = SelectedTime.PlusHours(-12);
			}
		}

		internal void SetPM()
		{
			if (IsAM)
			{
				SelectedTime = SelectedTime.PlusHours(12);
			}
		}

		internal void ToggleMeridiem()
		{
			if (IsAM)
				SetPM();
			else if (IsPM)
				SetAM();
		}

		internal void SetHour(int hour)
		{
			SelectedTime = new LocalTime(hour, SelectedTime.Minute, SelectedTime.Second);			
		}

		internal void SetMinute(int minute)
		{
			SelectedTime = new LocalTime(SelectedTime.Hour, minute, SelectedTime.Second);
		}
	}
}
