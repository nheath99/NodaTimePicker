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
		
		internal LocalDate? SelectedDate { get; private set; }
		internal LocalDate MonthToDisplay { get; private set; }
		public ViewMode ViewMode { get; private set; } = ViewMode.Days;

		internal event Action OnSelected;
		internal event Action OnUpdated;
		internal event Action OnOpened;
		internal event Action OnClosed;
		internal event Action OnCleared;
		internal event Action OnToday;
		internal event Action OnStateChanged;

		internal event Action OnMonthSelected;

		void Selected() => OnSelected?.Invoke();
		void Updated() => OnUpdated?.Invoke();
		void Opened() => OnOpened?.Invoke();
		void Closed() => OnClosed?.Invoke();
		void Cleared() => OnCleared?.Invoke();
		void TodaySelected() => OnToday?.Invoke();
		void StateChanged() => OnStateChanged?.Invoke();
		void MonthSelected() => OnMonthSelected?.Invoke();

		internal int? SelectedMonth => SelectedDate?.Month;

		internal void SetSelectedDate(LocalDate selectedDate)
		{
			Console.WriteLine(nameof(SetSelectedDate));

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

		internal void SetDisplayMonth(int month)
		{
			Console.WriteLine(nameof(SetDisplayMonth));

			MonthToDisplay = new LocalDate(MonthToDisplay.Year, month, 1);			
		}

		internal void SetDisplayYear(int year)
		{
			Console.WriteLine(nameof(SetDisplayYear));

			MonthToDisplay = new LocalDate(year, MonthToDisplay.Month, 1);
		}

		internal void SetViewMode(ViewMode viewMode)
		{
			Console.WriteLine(nameof(SetViewMode));

			ViewMode = viewMode;
			MonthSelected();
			OnStateChanged();
		}

		internal void NextViewMode()
		{
			Console.WriteLine(nameof(NextViewMode));

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
			Console.WriteLine(nameof(PreviousViewMode));

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
			Console.WriteLine(nameof(NextMonth));

			MonthToDisplay = MonthToDisplay.PlusMonths(1);
			MonthSelected();
		}

		internal void PreviousMonth()
		{
			Console.WriteLine(nameof(PreviousMonth));

			MonthToDisplay = MonthToDisplay.PlusMonths(-1);
			MonthSelected();
		}
	}
}
