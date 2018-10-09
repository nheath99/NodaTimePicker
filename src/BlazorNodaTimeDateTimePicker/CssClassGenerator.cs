using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace BlazorNodaTimeDateTimePicker
{
	internal static class CssClassGenerator
	{
		internal static string Main(bool inline)
		{
			var str = "datepicker";
			if (inline == false)
				str += " datepicker-dropdown-menu";

			return str;
		}

		internal static string Day(LocalDate Date, DatePickerState State)
		{
			bool disabled = State.IsDayDisabled(Date);
			bool isToday = Date == State.Today;
			bool isOld = Date.Month < State.MonthToDisplay.Month;
			bool isNew = Date.Month > State.MonthToDisplay.Month;

			var sb = new List<string>
			{
				"day",
				Date.DayOfWeek.IsWeekday() ? "weekday" : "weekend"
			};

			if (isOld)
				sb.Add("old");
			if (isNew)
				sb.Add("new");
			if (isToday)
				sb.Add("today");
			if (State.SelectedDate == Date)
				sb.Add("active");
			if (disabled)
				sb.Add("disabled");

			return string.Join(" ", sb);
		}

		internal static string DayOfWeek(IsoDayOfWeek dayOfWeek, IEnumerable<IsoDayOfWeek> daysOfWeekDisabled)
		{
			var strClass = "dow";
			if (dayOfWeek.IsWeekday())
				strClass += " weekday";
			else
				strClass += " weekend";

			if (daysOfWeekDisabled != null)
			{
				if (daysOfWeekDisabled.Contains(dayOfWeek))
					strClass += " disabled";
			}

			return strClass;
		}

		internal static string Month(int month, bool disabled, DatePickerState state)
		{
			var strClass = "month";

			// Disabled
			if (disabled)
				strClass += " disabled";

			// Active
			if ((state.SelectedMonth.HasValue && month == state.SelectedMonth.Value)
				&& (state.SelectedYear == state.MonthToDisplay.Year))
				strClass += " active";

			return strClass;
		}

		internal static string Year(int year, bool disabled, DatePickerState state)
		{
			var (start, end) = state.MonthToDisplay.GetDecade();
			var strClass = "year";

			if (disabled)
				strClass += " disabled";

			if (state.SelectedYear.HasValue && year == state.SelectedYear.Value)
				strClass += " active";
			
			if (year < start)
				strClass += " old";
			else if (year > end)
				strClass += " new";

			return strClass;
		}

		internal static string Decade(int decade, bool disabled, DatePickerState State)
		{
			var (start, end) = State.MonthToDisplay.GetCentury();
			var strClass = "decade";

			if (State.SelectedDecade.HasValue && decade == State.SelectedDecade.Value)
				strClass += " active";

			if (disabled)
				strClass += " disabled";

			if (decade < start)
				strClass += " old";
			else if (decade > end)
				strClass += " new";

			return strClass;
		}
	}
}
