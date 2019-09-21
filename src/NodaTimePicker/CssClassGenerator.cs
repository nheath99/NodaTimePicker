using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace NodaTimePicker
{
	internal static class CssClassGenerator
	{
		const string datepicker_prefix = "datepicker";
		const string timepicker_prefix = "timepicker";
		const string datetimepicker_prefix = "datetimepicker";

		/// <summary>The value of the class attribute for the root html element of the DatePicker.</summary>
		/// <param name="inline">Whether the DatePicker is being displayed inline or not.</param>
		/// <returns></returns>
		internal static string DatePickerMain(bool inline)
		{
			var str = datepicker_prefix;
			if (inline == false)
				str += $" {datepicker_prefix}-dropdown-menu";
			
			return str;
		}

		internal static string DatePickerContent(bool displayWeekNumbers)
		{
			var str = "datepicker-content";
			if (displayWeekNumbers == true)
				str += " datepicker-content-show-week-numbers";

			return str;
		}

		/// <summary>The value of the class attribute for the Day element.</summary>
		/// <param name="Date">The day to render.</param>
		/// <param name="State"></param>
		/// <returns></returns>
		internal static string Day(LocalDate Date, DatePickerComponentBase State)
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

		internal static string Month(int month, bool disabled, DatePickerComponentBase state)
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

		internal static string Year(int year, bool disabled, DatePickerComponentBase state)
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

		internal static string Decade(int decade, bool disabled, DatePickerComponentBase state)
		{
			var (start, end) = state.MonthToDisplay.GetCentury();
			var strClass = "decade";

			if (state.SelectedDecade.HasValue && decade == state.SelectedDecade.Value)
				strClass += " active";

			if (disabled)
				strClass += " disabled";

			if (decade < start)
				strClass += " old";
			else if (decade > end)
				strClass += " new";

			return strClass;
		}
		
		internal static string TimePickerMain(bool inline)
		{
			var str = timepicker_prefix;
			if (inline == false)
				str += $" {timepicker_prefix}-dropdown-menu";

			return str;
		}

		internal static string DateTimePickerMain(bool inline)
		{
			var str = datetimepicker_prefix;
			if (inline == false)
				str += $" {datetimepicker_prefix}-dropdown-menu";

			return str;
		}
	}
}
