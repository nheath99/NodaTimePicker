using NodaTime;
using System;

namespace BlazorNodaTimeDateTimePicker
{
    public class DatePickerState
    {
		public DatePickerState()
		{
			Today = SystemClock.Instance.Today();
			MonthToDisplay = Today.StartOfMonth();
		}

		public LocalDate Today { get; }
		
		public string Val { get; set; } = "123";
		public LocalDate? SelectedDate { get; private set; }
		public ViewMode ViewMode { get; private set; } = ViewMode.Days;
		public LocalDate MonthToDisplay { get; set; }

		public event Action OnSelected;
		public event Action OnUpdated;
		public event Action OnOpened;
		public event Action OnClosed;
		public event Action OnCleared;
		public event Action OnToday;

		internal event Action OnStateChanged;

		void Selected() => OnSelected?.Invoke();
		void Updated() => OnUpdated?.Invoke();
		void Opened() => OnOpened?.Invoke();
		void Closed() => OnClosed?.Invoke();
		void Cleared() => OnCleared?.Invoke();
		void TodaySelected() => OnToday?.Invoke();
		void StateChanged() => OnStateChanged?.Invoke();

		internal void SetSelectedDate(LocalDate selectedDate)
		{
			//SelectedDate = selectedDate;
			if (SelectedDate != selectedDate)
			{
				SelectedDate = selectedDate;

				if (selectedDate.Month < MonthToDisplay.Month)
					PreviousMonth();
				else if (selectedDate.Month > MonthToDisplay.Month)
					NextMonth();

				StateChanged();
				Selected();
			}
		}

		internal void SetViewMode(ViewMode viewMode)
		{
			ViewMode = viewMode;

			OnStateChanged();
		}

		internal void NextViewMode()
		{
			switch (ViewMode)
			{
				case ViewMode.Days:
					ViewMode = ViewMode.Months;
					break;
				case ViewMode.Months:
					ViewMode = ViewMode.Years;
					break;
				case ViewMode.Years:
					ViewMode = ViewMode.Decades;
					break;
				case ViewMode.Decades:
				default:
					break;
			}
		}

		internal void PreviousViewMode()
		{
			switch (ViewMode)
			{
				case ViewMode.Months:
					ViewMode = ViewMode.Days;
					break;
				case ViewMode.Years:
					ViewMode = ViewMode.Months;
					break;
				case ViewMode.Decades:
					ViewMode = ViewMode.Years;
					break;			
				case ViewMode.Days:
				default:
					break;
			}
		}

		internal void NextMonth()
		{
			MonthToDisplay = MonthToDisplay.PlusMonths(1);
		}

		internal void PreviousMonth()
		{
			MonthToDisplay = MonthToDisplay.PlusMonths(-1);
		}
	}
}
