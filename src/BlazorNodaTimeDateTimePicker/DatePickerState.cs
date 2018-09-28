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

		internal event Action<LocalDate> OnSelected; // when a date is selected
		internal event Action OnCleared; // when the date is set to null
		internal event Action<LocalDate?> OnSelectedDateChanged; // when the selected date is changed

		internal event Action OnUpdated;
		internal event Action OnOpened; // when the datepicker is opened
		internal event Action OnClosed; // when the datepicker is closed
		
		internal event Action OnToday; // the Today button has been clicked
		internal event Action OnStateChanged;

		internal event Action OnMonthToDisplayChanged;

		void SelectedDateChanged()
		{
			OnSelectedDateChanged?.Invoke(SelectedDate);

			if (SelectedDate.HasValue)
				OnSelected?.Invoke(SelectedDate.Value);
			else
				OnCleared?.Invoke();
		}
		
		void Updated() => OnUpdated?.Invoke();
		void Opened() => OnOpened?.Invoke();
		void TodaySelected() => OnToday?.Invoke();
		void StateChanged() => OnStateChanged?.Invoke();
		void MonthToDisplayChanged() => OnMonthToDisplayChanged?.Invoke();

		internal int? SelectedMonth => SelectedDate?.Month;

		internal void SetSelectedDate(LocalDate selectedDate)
		{
			Console.WriteLine(nameof(SetSelectedDate));

			if (SelectedDate != selectedDate)
			{
				SelectedDate = selectedDate;
				
				StateChanged();
				SelectedDateChanged();
			}

			MonthToDisplay = new LocalDate(selectedDate.Year, selectedDate.Month, 1);
			MonthToDisplayChanged();
		}

		internal void SetSelectedDateToday()
		{
			Console.WriteLine(nameof(SetSelectedDateToday));

			SetSelectedDate(Today);
		}

		internal void ClearSelectedDate()
		{
			Console.WriteLine(nameof(ClearSelectedDate));

			SelectedDate = null;

			StateChanged();
			SelectedDateChanged();
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
			MonthToDisplayChanged();
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
			MonthToDisplayChanged();
		}

		internal void PreviousMonth()
		{
			Console.WriteLine(nameof(PreviousMonth));

			MonthToDisplay = MonthToDisplay.PlusMonths(-1);
			MonthToDisplayChanged();
		}
	}
}
