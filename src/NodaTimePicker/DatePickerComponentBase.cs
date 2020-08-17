using Microsoft.AspNetCore.Components;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NodaTimePicker
{
	public class DatePickerComponentBase : PickerComponentBase
	{
		#region Parameters

		/// <summary>If true, detailed information will be logged to the Console.</summary>
		[Parameter] public bool Logging { get; set; } = false;
		/// <summary> Specify the culture to display dates and text in. Default is InvariantCulture.</summary>
		[Parameter] public CultureInfo FormatProvider { get; set; } = CultureInfo.CurrentUICulture;
		/// <summary>If true, a row will be displayed with the day-of-week names. If false, it will not.</summary>
		[Parameter] public bool DisplayDaysOfWeek { get; set; } = true;
		/// <summary>In <see cref="ViewMode.Days"/>, the format of the month and year in the header. Defaults to MMMM yyyy, i.e. October 2018.</summary>
		[Parameter] public string DayViewHeaderFormat { get; set; } = "MMMM yyyy";
		/// <summary>In <see cref="ViewMode.Months"/>, the format of the year in the header. Defaults to yyyy, i.e. 2018.</summary>
		[Parameter] public string MonthViewHeaderFormat { get; set; } = "yyyy";
		/// <summary>If true, the Today button will be displayed. If false, it will be hidden.</summary>
		[Parameter] public bool ShowToday { get; set; } = true;
		/// <summary>If true, the Clear button will be displayed. If false, it will be hidden.</summary>
		[Parameter] public bool ShowClear { get; set; } = true;
		/// <summary>The text to display on the Today button.</summary>
		[Parameter] public string TodayText { get; set; } = "Today";
		/// <summary>The text to display on the Clear button.</summary>
		[Parameter] public string ClearText { get; set; } = "Clear";
		/// <summary>The text to display on the Close button.</summary>
		[Parameter] public string CloseText { get; set; } = "Close";
		/// <summary>The <see cref="ViewMode"/> to display on initialization. Defaults to <see cref="ViewMode.Days"/>.</summary>
		[Parameter] public ViewMode ViewMode { get; set; } = ViewMode.Days;
		/// <summary>The granularity of date selection. Defaults to <see cref="ViewMode.Days"/>.</summary>
		[Parameter] public ViewMode MinimumSelectionMode { get; set; } = ViewMode.Days;
		/// <summary>Whether to show the week number in <see cref="ViewMode"/> = <see cref="ViewMode.Days"/>. Default is <see cref="false"/></summary>
		[Parameter] public bool DisplayWeekNumber { get; set; } = false;
		/// <summary>The text to display on the Week number heading, if <see cref="DisplayWeekNumber"/> is set to true</summary>
		[Parameter] public string WeekAbbreviation { get; set; } = "Wk";
		/// <summary>The content to display in the Previous button. Defaults to the Less Than character (&lt;)</summary>
		[Parameter] public RenderFragment PrevContent { get; set; } = builder => builder.AddContent(1, new MarkupString("&lt;"));
		/// <summary>The content to display in the Previous button. Defaults to the Greater Than character (&gt;)</summary>
		[Parameter] public RenderFragment NextContent { get; set; } = builder => builder.AddContent(1, new MarkupString("&gt;"));
		/// <summary>The day of the week to start from, when in <see cref="ViewMode.Days"/>.</summary>
		[Parameter] public IsoDayOfWeek FirstDayOfWeek { get; set; } = IsoDayOfWeek.Monday;

		internal LocalDate? _selectedDate;
		/// <summary>The currently selected date.</summary>
		[Parameter]
		public LocalDate? SelectedDate
		{
			get => _selectedDate;
			set
			{
				_selectedDate = value;
				DisplaySelectedMonth();
			}
		}

		/// <summary>The earliest date that can be selected, inclusive. A value of null indicates that there is no minimum date.</summary>
		[Parameter] public LocalDate? MinDate { get; set; }
		/// <summary>The latest date that can be selected, inclusive. A value of null indicates that there is no maximum date.</summary>
		[Parameter] public LocalDate? MaxDate { get; set; }
		/// <summary>Specific dates that cannot be selected.</summary>
		[Parameter] public IEnumerable<LocalDate> DisabledDates { get; set; }
		/// <summary>Specific dates that can be selected. Note this overrides any disabled state.</summary>
		[Parameter] public IEnumerable<LocalDate> EnabledDates { get; set; }
		/// <summary>Days of the week that cannot be selected.</summary>
		[Parameter] public IEnumerable<IsoDayOfWeek> DaysOfWeekDisabled { get; set; }
		/// <summary>Inclusive date intervals that cannot be selected.</summary>
		[Parameter] public IEnumerable<(LocalDate start, LocalDate end)> DisabledDateIntervals { get; set; }
		/// <summary>Whether to display the DatePicker with display:inline (true) or display:block (false).</summary>
		[Parameter] public bool Inline { get; set; } = false;
		/// <summary>If <see cref="Inline"></see> = false, whether the DatePicker is visible or not.</summary>
		[Parameter] public bool Visible { get; set; } = true;
		/// <summary></summary>
		[Parameter] public bool DisplayWeekYearSelectionMode { get; set; } = false;
		/// <summary>An event that is invoked whenever the <see cref="SelectedDate"/> value is changed.</summary>
		[Parameter] public EventCallback<LocalDate?> OnSelectedDateChanged { get; set; }
		/// <summary>An event that is invoked when the a day is selected.</summary>
		[Parameter] public EventCallback<LocalDate> OnSelected { get; set; }
		/// <summary>An event that is invoked when the selected day is cleared.</summary>
		[Parameter] public EventCallback OnCleared { get; set; }
		/// <summary>An event that is invoked when the UI is updated.</summary>
		[Parameter] public EventCallback OnUpdated { get; set; }
		/// <summary>An event that is invoked when the ViewMode is changed.</summary>
		[Parameter] public EventCallback<ViewMode> OnViewModeChanged { get; set; }
		[Parameter] public EventCallback<LocalDate> OnMonthChanged { get; set; }
		[Parameter] public EventCallback<int> OnYearChanged { get; set; }
		/// <summary>An event that is invoked when the DatePicker is opened.</summary>
		[Parameter] public EventCallback OnOpened { get; set; }
		/// <summary>An event that is invoked when the DatePicker is closed.</summary>
		[Parameter] public EventCallback OnClosed { get; set; }

		[Parameter] public EventCallback OnDisabled { get; set; }

		[Parameter] public EventCallback OnEnabled { get; set; }


		[Parameter] public bool CanNavigate { get; set; } = true;
		[Parameter] public bool HideOldAndNew { get; set; } = false;
		[Parameter] public bool CanSelectDisabled { get; set; } = false;

		[Parameter] public CalendarSystem CalendarSystem { get; set; } = CalendarSystem.Iso;

		#endregion

		#region Overriden Methods

		protected override void OnInitialized()
		{
			Log(nameof(OnInitialized));

			Clock = SystemClock.Instance;

			if (SelectedDate != null)
			{
				MonthToDisplay = new LocalDate(SelectedDate.Value.Year, SelectedDate.Value.Month, 1).WithCalendar(CalendarSystem);
			}
			else
			{
				MonthToDisplay = Today.StartOfMonth();
			}
		}

		protected override void OnParametersSet()
		{
			// This method is executed after component initialization and each time the component is rendered.

			Log(nameof(OnParametersSet));

			ViewMode = ViewMode < MinimumSelectionMode ? MinimumSelectionMode : ViewMode;

			switch (ViewMode)
			{
				case ViewMode.Days:
					RenderDays();
					break;
				case ViewMode.Months:
					RenderMonths();
					break;
				case ViewMode.Years:
					RenderYears();
					break;
				case ViewMode.Decades:
					RenderDecades();
					break;
				default:
					break;
			}
		}

		#endregion

		#region Html Classes and Styles

		internal string ClassName
		{
			get
			{
				if (!string.IsNullOrEmpty(Class))
				{
					return $"{CssClassGenerator.DatePickerMain(Inline)} {Class}";
				}
				else
				{
					return CssClassGenerator.DatePickerMain(Inline);
				}
			}
		}

		internal string ContentClassName => CssClassGenerator.DatePickerContent(DisplayWeekNumber);

		internal string MainStyle
		{
			get
			{
				var str = new List<string>();

				if (Inline == false && Visible == false)
				{
					str.Add("display:none;");
				}
				if (!string.IsNullOrEmpty(Width))
				{
					str.Add($"width:{Width};");
				}
				if (!string.IsNullOrEmpty(MaxWidth))
				{
					str.Add($"max-width:{MaxWidth};");
				}

				str.Add(Style);

				return string.Join(" ", str);
			}
		}

		#endregion

		#region Display Strings

		internal string DayOfWeekAbbreviation(IsoDayOfWeek isoDayOfWeek) => FormatProvider.DateTimeFormat.GetShortestDayName(isoDayOfWeek.ToDayOfWeek());

		internal string MonthText => MonthToDisplay.ToString(DayViewHeaderFormat, FormatProvider);

		internal string MonthName(int month) => FormatProvider.DateTimeFormat.GetAbbreviatedMonthName(month);

		internal string YearText => MonthToDisplay.ToString(MonthViewHeaderFormat, FormatProvider);

		internal string DecadeText => MonthToDisplay.GetDecadeString();

		internal string CenturyText => MonthToDisplay.GetCenturyString();

		#endregion

		#region Display

		internal void Open()
		{
			if (!Inline)
			{
				Visible = true;
				OnOpened.InvokeAsync(null).Wait();
			}
		}

		internal void Close()
		{
			if (!Inline)
			{
				Visible = false;
				OnClosed.InvokeAsync(null).Wait();
			}
		}

		#endregion

		#region Selected Date

		internal void SelectedDateChanged()
		{
			OnSelectedDateChanged.InvokeAsync(SelectedDate).Wait();

			if (SelectedDate.HasValue)
				OnSelected.InvokeAsync(SelectedDate.Value).Wait();
			else
				OnCleared.InvokeAsync(null).Wait();
		}

		internal void SetSelectedDate(LocalDate selectedDate)
		{
			Log(nameof(SetSelectedDate));

			if (SelectedDate != selectedDate)
			{
				SelectedDate = selectedDate;

				SelectedDateChanged();
			}
		}

		internal void ClearSelectedDate()
		{
			Log(nameof(ClearSelectedDate));

			SelectedDate = null;

			OnCleared.InvokeAsync(null).Wait();
		}

		internal void DisplaySelectedMonth()
		{
			if (SelectedDate.HasValue)
			{
				MonthToDisplay = SelectedDate.Value.StartOfMonth();

				RenderDays();
			}
		}

		#endregion

		#region Days

		internal IEnumerable<LocalDate> Days { get; private set; }

		internal void RenderDays()
		{
			Log(nameof(RenderDays));

			var startOfWeekOfMonth = MonthToDisplay.StartOfWeek(FirstDayOfWeek);

			Log(startOfWeekOfMonth.ToString());

			var endOfMonth = MonthToDisplay.EndOfMonth();
			var endOfWeekOfMonth = endOfMonth.EndOfWeek(FirstDayOfWeek);

			Days = GetDaysBetween(startOfWeekOfMonth, endOfWeekOfMonth);
		}
		#endregion

		#region Months

		internal bool[] disabledMonths;

		void RenderMonths()
		{
			Log(nameof(RenderMonths));

			disabledMonths = new bool[12];

			for (int i = 0; i < 12; i++)
			{
				disabledMonths[i] = IsMonthDisabled(i + 1, MonthToDisplay.Year);
			}
		}

		/// <summary>Callback for when a Month has been selected.</summary>
		/// <param name="eventArgs"></param>
		/// <param name="month"></param>
		internal void MonthClicked(EventArgs eventArgs, int month)
		{
			Log(nameof(MonthClicked));

			if (disabledMonths[month - 1] == false)
			{
				SetDisplayMonth(month);
				if (MinimumSelectionMode == ViewMode.Months)
				{
					SetSelectedMonth(month);
				}
				else
				{
					PreviousViewMode();
				}
			}
		}

		internal bool CanNextMonth()
		{

			var next = MonthToDisplay.PlusMonths(1);
			return !this.MaxDate.HasValue || this.MaxDate >= next.StartOfMonth();
		}

		internal bool CanPreviousMonth()
		{
			var next = MonthToDisplay.PlusMonths(-1);
			return !this.MinDate.HasValue || this.MinDate <= next.EndOfMonth();
		}

		/// <summary>Increments the MonthToDisplay value by one month.</summary>
		/// <param name="eventArgs"></param>
		internal void NextMonth(EventArgs eventArgs)
		{
			if (CanNextMonth())
			{
				Log(nameof(NextMonth));

				MonthToDisplay = MonthToDisplay.PlusMonths(1);
				RenderDays();
				OnMonthChanged.InvokeAsync(MonthToDisplay).Wait();
				_onUpdated();
			}

		}

		/// <summary>Decrements the MonthToDisplay value by one month.</summary>
		/// <param name="eventArgs"></param>
		internal void PreviousMonth()
		{
			if (CanPreviousMonth())
			{
				Log(nameof(PreviousMonth));

				MonthToDisplay = MonthToDisplay.PlusMonths(-1);
				RenderDays();
				OnMonthChanged.InvokeAsync(MonthToDisplay).Wait();
				_onUpdated();
			}

		}

		/// <summary>Displays the Month selection mode, i.e. a list of months of the year.</summary>
		/// <param name="eventArgs"></param>
		internal void SelectMonth(EventArgs eventArgs)
		{
			if (this.CanNavigate)
			{
				Log(nameof(SelectMonth));

				RenderMonths();
				NextViewMode();
			}

		}

		internal void SetSelectedMonth(int month)
		{
			Log(nameof(SetSelectedMonth));

			var selectedDate = new LocalDate(MonthToDisplay.Year, month, 1).WithCalendar(CalendarSystem);

			SelectedDate = selectedDate;
			MonthToDisplay = selectedDate;
			OnMonthChanged.InvokeAsync(MonthToDisplay).Wait();
		}

		internal void SetDisplayMonth(int month)
		{
			Log(nameof(SetDisplayMonth));

			MonthToDisplay = new LocalDate(MonthToDisplay.Year, month, 1).WithCalendar(CalendarSystem);
		}

		#endregion

		#region Years

		internal int yearStart, yearEnd;
		internal Dictionary<int, bool> disabledYears;

		void RenderYears()
		{
			Log(nameof(RenderYears));

			(yearStart, yearEnd) = MonthToDisplay.GetDecade();

			disabledYears = new Dictionary<int, bool>();

			for (int i = yearStart - 1; i <= yearEnd + 1; i++)
			{
				disabledYears.Add(i, IsYearDisabled(i));
			}
		}

		internal void YearClicked(EventArgs eventArgs, int year)
		{
			Log(nameof(YearClicked));

			if (disabledYears[year] == false)
			{
				SetDisplayYear(year);
				if (MinimumSelectionMode == ViewMode.Years)
				{
					SetSelectedYear(year);
				}
				else
				{
					RenderMonths();
					PreviousViewMode();
				}
			}
		}

		internal void NextYear(EventArgs eventArgs)
		{
			Log(nameof(NextYear));

			MonthToDisplay = MonthToDisplay.PlusYears(1);
			RenderMonths();
			_onUpdated();
		}

		internal void PreviousYear(EventArgs eventArgs)
		{
			Log(nameof(PreviousYear));

			MonthToDisplay = MonthToDisplay.PlusYears(-1);
			RenderMonths();
			_onUpdated();
		}

		internal void SelectYear(EventArgs eventArgs)
		{
			Log(nameof(SelectYear));

			RenderYears();
			NextViewMode();
		}

		internal void SetSelectedYear(int year)
		{
			Log(nameof(SetSelectedMonth));

			var selectedDate = new LocalDate(year, 1, 1).WithCalendar(CalendarSystem);

			SelectedDate = selectedDate;
			MonthToDisplay = selectedDate;
		}

		internal void SetDisplayYear(int year)
		{
			Log(nameof(SetDisplayYear));

			MonthToDisplay = new LocalDate(year, MonthToDisplay.Month, 1).WithCalendar(CalendarSystem);
		}

		#endregion

		#region Decades

		internal int decadeStart, decadeEnd;
		internal Dictionary<int, bool> disabledDecades;

		void RenderDecades()
		{
			Log(nameof(RenderDecades));

			(decadeStart, decadeEnd) = MonthToDisplay.GetCentury();

			disabledDecades = new Dictionary<int, bool>();

			for (int i = decadeStart - 10; i <= decadeEnd + 10; i += 10)
			{
				disabledDecades.Add(i, IsDecadeDisabled(i));
			}
		}

		internal void DecadeClicked(EventArgs eventArgs, int decade)
		{
			Log(nameof(DecadeClicked));

			if (disabledDecades[decade] == false)
			{
				SetDisplayYear(decade);
				if (MinimumSelectionMode == ViewMode.Decades)
				{
					SetSelectedYear(decade);
				}
				else
				{
					RenderYears();
					PreviousViewMode();
				}
			}
		}

		internal void NextDecade(EventArgs eventArgs)
		{
			Log(nameof(NextDecade));

			MonthToDisplay = MonthToDisplay.PlusYears(10);
			RenderYears();
			_onUpdated();
		}

		internal void PreviousDecade(EventArgs eventArgs)
		{
			Log(nameof(PreviousDecade));

			MonthToDisplay = MonthToDisplay.PlusYears(-10);
			RenderYears();
			_onUpdated();
		}

		internal void SelectDecade(EventArgs eventArgs)
		{
			Log(nameof(SelectDecade));

			RenderDecades();
			NextViewMode();
		}

		#endregion

		#region Centuries

		internal void NextCentury(EventArgs eventArgs)
		{
			Log(nameof(NextCentury));

			MonthToDisplay = MonthToDisplay.PlusYears(100);
			RenderDecades();
			_onUpdated();
		}

		internal void PreviousCentury(EventArgs eventArgs)
		{
			Log(nameof(PreviousCentury));

			MonthToDisplay = MonthToDisplay.PlusYears(-100);
			RenderDecades();
			_onUpdated();
		}

		#endregion

		#region EventHandlers

		internal void DayClicked(EventArgs eventArgs, LocalDate date)
		{
			Log(nameof(DayClicked));

			if (CanSelectDisabled || !IsDayDisabled(date))
			{
				SetSelectedDate(date);
			}
		}

		internal void TodayClicked(EventArgs eventArgs)
		{
			Log(nameof(TodayClicked));

			SetSelectedDate(Today);
		}

		internal void ClearClicked(EventArgs eventArgs)
		{
			Log(nameof(ClearClicked));

			ClearSelectedDate();
		}

		internal void CloseClicked(EventArgs eventArgs)
		{
			Log(nameof(CloseClicked));

			if (!Inline)
			{
				Close();
			}
		}

		#endregion

		#region Enabled/Disabled Selections

		internal Func<LocalDate, bool> DaysEnabledFunction { get; set; }

		public bool IsDayDisabled(LocalDate date)
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
					{
						return true;
					}
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
				var daysInMonth = CalendarSystem.GetDaysInMonth(year, month);
				for (int i = 1; i <= daysInMonth; i++)
				{
					var date = new LocalDate(year, month, i).WithCalendar(CalendarSystem);
					if (DaysEnabledFunction(date))
						return true;
				}

				return false;
			}

			// If Month/Year is before MinDate, month is disabled
			if (MinDate.HasValue)
			{
				if ((MinDate.Value.Year > year) ||
					(MinDate.Value.Year == year && MinDate.Value.Month > month))
					return true;
			}

			// If Month/Year is after MaxDate, month is disabled
			if (MaxDate.HasValue)
			{
				if ((MaxDate.Value.Year < year) ||
					(MaxDate.Value.Year == year && MaxDate.Value.Month < month))
					return true;
			}

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
			if (MinDate.HasValue && MinDate.Value.Year > year)
				return true;

			// If Year is after MaxDate, year is disabled
			if (MaxDate.HasValue && MaxDate.Value.Year < year)
				return true;

			return false;
		}

		internal bool IsDecadeDisabled(int decade)
		{
			// Just ignore the function and enable the decade.
			// We could calculate whether any day in the decade is enabled, but that seems overkill at the moment
			if (DaysEnabledFunction != null)
				return false;

			var minDateDecade = MinDate?.Year.Decade();
			var maxDateDecade = MaxDate?.Year.Decade();

			if (minDateDecade.HasValue && minDateDecade > decade)
				return true;

			if (maxDateDecade.HasValue && maxDateDecade < decade)
				return true;

			return false;
		}

		#endregion

		#region ViewMode

		internal void SetViewMode(ViewMode viewMode)
		{
			Log(nameof(SetViewMode));

			if (viewMode != ViewMode)
			{
				ViewMode = viewMode;
				OnViewModeChanged.InvokeAsync(viewMode).Wait();
				RenderDays();
			}
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

		#endregion

		#region Logging

		internal Action<string> LogFunction = d => Console.WriteLine(d);
		internal void Log(string data)
		{
			if (Logging)
				LogFunction?.Invoke(data);
		}

		#endregion

		#region Helper Methods / Properties

		internal int? SelectedMonth => SelectedDate?.Month;
		internal int? SelectedYear => SelectedDate?.Year;
		internal int? SelectedDecade => SelectedDate?.Year.Decade();
		internal IClock Clock { get; set; }
		internal LocalDate Today => Clock.Today().WithCalendar(CalendarSystem);
		internal LocalDate MonthToDisplay { get; private set; }

		internal static IEnumerable<LocalDate> GetDaysBetween(LocalDate start, LocalDate end)
		{
			if (start > end)
			{
				yield break;
			}

			do
			{
				yield return start;
				start = start.PlusDays(1);
			} while (start <= end);
		}

		internal IEnumerable<int> GetWeeks
		{
			get
			{
				if (Days != null && Days.Count() > 0)
				{
					int i = 0;
					foreach (var day in Days)
					{
						if (i % 7 == 0)
						{
							yield return NodaTime.Calendars.WeekYearRules.Iso.GetWeekOfWeekYear(day);
						}

						i++;
					}
				}
				else
				{
					yield break;
				}
			}
		}

		#endregion

		#region Miscellaneous

		internal void _onUpdated()
		{
			Log(nameof(OnUpdated));

			OnUpdated.InvokeAsync(null).Wait();
		}

		#endregion
	}
}
