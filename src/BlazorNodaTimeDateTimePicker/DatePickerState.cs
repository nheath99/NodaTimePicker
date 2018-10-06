using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorNodaTimeDateTimePicker
{
    public class DatePickerState
    {
		public DatePickerState()
		{
			Clock = SystemClock.Instance;
			MonthToDisplay = Today.StartOfMonth();			
		}

		internal IClock Clock { get; }
		internal LocalDate Today => Clock.Today();
					
		internal LocalDate? SelectedDate { get; set; }
		internal LocalDate MonthToDisplay { get; private set; }
		internal ViewMode ViewMode { get; set; } = ViewMode.Days;
		internal bool Visible { get; set; }
		internal bool Inline { get; set; }
		internal IsoDayOfWeek FirstDayOfWeek { get; set; } = IsoDayOfWeek.Monday;

		internal LocalDate MinDate { get; set; } = LocalDate.MinIsoValue;
		internal LocalDate MaxDate { get; set; } = LocalDate.MaxIsoValue;
		internal IEnumerable<LocalDate> DisabledDates { get; set; }
		internal IEnumerable<LocalDate> EnabledDates { get; set; }
		internal IEnumerable<IsoDayOfWeek> DaysOfWeekDisabled { get; set; }
		internal IEnumerable<(LocalDate start, LocalDate end)> DisabledDateIntervals { get; set; }
		internal Func<LocalDate, bool> DaysEnabledFunction { get; set; }
		internal bool WriteToLog { get; set; }
		
		internal event Action<LocalDate> OnSelected; // when a date is selected
		internal event Action OnCleared; // when the date is set to null
		internal event Action<LocalDate?> OnSelectedDateChanged; // when the selected date is changed

		internal event Action OnUpdated;
		internal event Action OnOpened; // when the datepicker is opened
		internal event Action OnClosed; // when the datepicker is closed
		
		internal event Action OnToday; // the Today button has been clicked
		internal event Action OnStateChanged;

		internal event Action OnMonthToDisplayChanged;
		internal event Action OnYearToDisplayChanged;
		internal event Action OnDecadeToDisplayChanged;
		internal event Action OnCenturyToDisplayChanged;
		
		internal void Log(string data)
		{
			if (WriteToLog)
				Log(data);
		}

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
		void Closed() => OnClosed?.Invoke();
		void TodaySelected() => OnToday?.Invoke();
		void StateChanged() => OnStateChanged?.Invoke();
		void MonthToDisplayChanged() => OnMonthToDisplayChanged?.Invoke();
		void YearToDisplayChanged() => OnYearToDisplayChanged?.Invoke();
		void DecadeToDisplayChanged() => OnDecadeToDisplayChanged?.Invoke();
		void CentureToDisplayChanged() => OnCenturyToDisplayChanged?.Invoke();

		internal int? SelectedMonth => SelectedDate?.Month;
		internal int? SelectedYear => SelectedDate?.Year;
		internal int? SelectedDecade
		{
			get
			{
				if (SelectedDate.HasValue)
					return (SelectedDate.Value.Year / 10) * 10;
				else
					return null;
			}
		}

		internal void SetSelectedDate(LocalDate selectedDate)
		{
			Log(nameof(SetSelectedDate));

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
			Log(nameof(SetSelectedDateToday));

			SetSelectedDate(Today);
		}

		internal void ClearSelectedDate()
		{
			Log(nameof(ClearSelectedDate));

			SelectedDate = null;

			StateChanged();
			SelectedDateChanged();
		}

		internal void SetDisplayMonth(int month)
		{
			Log(nameof(SetDisplayMonth));

			MonthToDisplay = new LocalDate(MonthToDisplay.Year, month, 1);			
		}

		internal void SetDisplayYear(int year)
		{
			Log(nameof(SetDisplayYear));

			MonthToDisplay = new LocalDate(year, MonthToDisplay.Month, 1);
		}

		internal void SetViewMode(ViewMode viewMode)
		{
			Log(nameof(SetViewMode));

			ViewMode = viewMode;
			MonthToDisplayChanged();
			StateChanged();
		}

		internal void NextViewMode()
		{
			Log(nameof(NextViewMode));

			switch (ViewMode)
			{
				case ViewMode.Days:
					SetViewMode(ViewMode.Months);
					break;
				case ViewMode.Months:
					SetViewMode(ViewMode.Years);
					break;
				case ViewMode.Years:
					SetViewMode(ViewMode.Decades);
					break;
				case ViewMode.Decades:
				default:
					break;
			}
		}

		internal void PreviousViewMode()
		{
			Log(nameof(PreviousViewMode));

			switch (ViewMode)
			{
				case ViewMode.Months:
					SetViewMode(ViewMode.Days);
					break;
				case ViewMode.Years:
					SetViewMode(ViewMode.Months);
					break;
				case ViewMode.Decades:
					SetViewMode(ViewMode.Years);
					break;			
				case ViewMode.Days:
				default:
					break;
			}
		}

		internal void NextMonth()
		{
			Log(nameof(NextMonth));

			MonthToDisplay = MonthToDisplay.PlusMonths(1);
			MonthToDisplayChanged();
		}

		internal void PreviousMonth()
		{
			Log(nameof(PreviousMonth));

			MonthToDisplay = MonthToDisplay.PlusMonths(-1);
			MonthToDisplayChanged();
		}

		internal void NextYear()
		{
			Log(nameof(NextYear));

			MonthToDisplay = MonthToDisplay.PlusYears(1);
			YearToDisplayChanged();
		}
		
		internal void PreviousYear()
		{
			Log(nameof(PreviousYear));

			MonthToDisplay = MonthToDisplay.PlusYears(-1);
			YearToDisplayChanged();
		}

		internal void NextDecade()
		{
			Log(nameof(NextDecade));

			MonthToDisplay = MonthToDisplay.PlusYears(10);
			DecadeToDisplayChanged();
			StateChanged();
		}

		internal void PreviousDecade()
		{
			Log(nameof(PreviousDecade));

			MonthToDisplay = MonthToDisplay.PlusYears(-10);
			DecadeToDisplayChanged();
			StateChanged();
		}

		internal void NextCentury()
		{
			Log(nameof(NextCentury));

			MonthToDisplay = MonthToDisplay.PlusYears(100);
			CentureToDisplayChanged();
			StateChanged();
		}

		internal void PreviousCentury()
		{
			Log(nameof(PreviousCentury));

			MonthToDisplay = MonthToDisplay.PlusYears(-100);
			CentureToDisplayChanged();
			StateChanged();
		}

		internal bool IsDayDisabled(LocalDate date)
		{
			if (DaysEnabledFunction != null)
			{
				return !DaysEnabledFunction(date);
			}
			else
			{
				if (EnabledDates != null)
				{
					if (EnabledDates.Contains(date) == false)
						return false;
				}

				if (DisabledDates != null)
				{
					if (DisabledDates.Contains(date))
						return true;					
				}

				if (date < MinDate)
					return true;
				if (date > MaxDate)
					return true;

				if (DaysOfWeekDisabled != null)
				{
					if (DaysOfWeekDisabled.Contains(date.DayOfWeek))
						return true;
				}

				if (DisabledDateIntervals != null)
				{
					if (DisabledDateIntervals.Any(x => date >= x.start && date <= x.end))
						return true;
				}

				return false;
			}
		}

		internal bool IsMonthDisabled(int month, int year)
		{
			// If any date in the month is enabled, enable the month
			if (DaysEnabledFunction != null)
			{
				var daysInMonth = NodaTime.CalendarSystem.Iso.GetDaysInMonth(year, month);
				for (int i = 1; i <= daysInMonth; i++)
				{
					var date = new LocalDate(year, month, i);
					if (DaysEnabledFunction(date))
						return true;
				}

				return false;
			}

			// If Month/Year is before MinDate, month is disabled
			if ((MinDate.Year > year) ||
				(MinDate.Year == year && MinDate.Month > month))
				return true;

			// If Month/Year is after MaxDate, month is disabled
			if ((MaxDate.Year < year) ||
				(MaxDate.Year == year && MinDate.Month < month))
				return true;

			// If EnabledDates contains any value falling within Month/Date, month is enabled
			if (EnabledDates != null && EnabledDates.Any(x => x.Year == year && x.Month == month))
				return false;

			return false;
		}

		internal bool IsYearDisabled(int year)
		{
			// Just ignore the function and enalbe the year.
			// We could calculate whether any day in the year is enabled, but that seems overkill at the moment
			if (DaysEnabledFunction != null)
				return false;

			// If Year is before MinDate, year is disabled
			if (MinDate.Year > year)
			return true;

			// If Year is after MaxDate, year is disabled
			if (MaxDate.Year < year)
				return true;

			return false;
		}

		internal bool IsDecadeDisabled(int decade)
		{
			// Just ignore the function and enable the decade.
			// We could calculate whether any day in the decade is enabled, but that seems overkill at the moment
			if (DaysEnabledFunction != null)
				return false;

			var minDateDecade = MinDate.Year.Decade();
			var maxDateDecade = MaxDate.Year.Decade();

			if (minDateDecade > decade)
				return true;

			if (maxDateDecade < decade)
				return true;

			return false;
		}

		internal void Open()
		{
			if (!Inline)
			{
				Visible = true;
				Opened();
			}
		}

		internal void Close()
		{
			if (!Inline)
			{
				Visible = false;
				Closed();
			}
		}
	}
}
