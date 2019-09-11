using Microsoft.AspNetCore.Components;
using NodaTime;
using System.Collections.Generic;

namespace NodaTimePicker
{
	public class TimePickerComponentBase : PickerComponentBase
	{
		#region Parameters

		/// <summary>Whether to use 24-hour time. Default is 12-hour time.</summary>
		[Parameter] public bool Is24Hour { get; set; } = false;

		/// <summary>The number of minutes to increment or decrement by.</summary>
		[Parameter] public int MinuteStep { get; set; } = 5;

		/// <summary>Gets or sets the time displayed on the <see cref="TimePicker"/>.</summary>
		[Parameter] public LocalTime SelectedTime { get; set; }

		[Parameter] public bool Inline { get; set; } = false;
		[Parameter] public bool Visible { get; set; } = true;

		[Parameter] public EventCallback<LocalTime> OnSelectedTimeChanged { get; set; }

		[Parameter] public RenderFragment UpArrowIcon { get; set; }
		[Parameter] public RenderFragment DownArrowIcon { get; set; }

		#endregion

		#region Overridden Methods

		protected override void OnParametersSet()
		{
			if (UpArrowIcon == null)
			{
				UpArrowIcon = builder => builder.AddContent(1, new MarkupString("&uarr;"));
			}
			if (DownArrowIcon == null)
			{
				DownArrowIcon = builder => builder.AddContent(1, new MarkupString("&darr;"));
			}
		}

		#endregion

		internal bool IsAM => SelectedTime.Hour < 12;
		internal bool IsPM => SelectedTime.Hour >= 12;

		void _onSelectedTimeChanged()
		{
			OnSelectedTimeChanged.InvokeAsync(SelectedTime);
			if (Inline == false && CloseOnSelect)
			{
				close();
			}

			StateHasChanged();
		}

		internal TimeViewMode ViewMode = TimeViewMode.Clock;

		internal string ClassName
		{
			get
			{
				if (!string.IsNullOrEmpty(Class))
				{
					return $"{CssClassGenerator.TimePickerMain(Inline)} {Class}";
				}
				else
				{
					return CssClassGenerator.TimePickerMain(Inline);
				}
			}
		}

		protected string MainStyle
		{
			get
			{
				var str = new List<string>()
				{
					"min-height:182px;"
				};

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

		internal string HourDisplay => Is24Hour ? SelectedTime.ToString("HH", null) : SelectedTime.ToString("hh", null);
		internal string MinuteDisplay => SelectedTime.ToString("mm", null);

		void DisplayClock()
		{
			ViewMode = TimeViewMode.Clock;
		}

		internal void DisplayHours()
		{
			ViewModeHours_IsAM = IsAM;
			ViewMode = TimeViewMode.Hours;
		}

		internal void DisplayMinutes()
		{
			ViewMode = TimeViewMode.Minutes;
		}

		internal void IncrementHour()
		{
			SelectedTime = SelectedTime.PlusHours(1);
			_onSelectedTimeChanged();
		}

		internal void DecrementHour()
		{
			SelectedTime = SelectedTime.PlusHours(-1);
			_onSelectedTimeChanged();
		}

		internal void IncrementMinute()
		{
			SelectedTime = SelectedTime.PlusMinutes(MinuteStep);
			_onSelectedTimeChanged();
		}

		internal void DecrementMinute()
		{
			SelectedTime = SelectedTime.PlusMinutes(-MinuteStep);
			_onSelectedTimeChanged();
		}

		internal void ToggleMeridiem()
		{
			if (IsAM)
				SelectedTime = SelectedTime.PlusHours(12);
			else if (IsPM)
				SelectedTime = SelectedTime.PlusHours(-12);

			_onSelectedTimeChanged();
		}

		internal void HourSelected(int hour)
		{
			SelectedTime = new LocalTime(hour, SelectedTime.Minute, SelectedTime.Second);
			ViewMode = TimeViewMode.Clock;
			_onSelectedTimeChanged();
		}

		internal void MinuteSelected(int minute)
		{
			SelectedTime = new LocalTime(SelectedTime.Hour, minute, SelectedTime.Second);
			ViewMode = TimeViewMode.Clock;
			_onSelectedTimeChanged();
		}

		internal bool ViewModeHours_IsAM { get; set; }

		protected void close()
		{
			if (Inline == false)
			{
				Visible = false;
			}
		}
	}
}
