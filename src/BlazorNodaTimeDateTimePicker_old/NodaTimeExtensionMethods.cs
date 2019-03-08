using NodaTime;
using System;
using System.Collections.Generic;

namespace BlazorNodaTimeDateTimePicker
{
	internal static class NodaTimeExtensionMethods
	{
		#region IClock

		/// <summary>
		/// Returns a representation of the current <see cref="Instant"/> as a <see cref="LocalDateTime"/> type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static LocalDateTime Now(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime;
		}

		/// <summary>
		/// Returns the date of the current <see cref="Instant"/> as a <see cref="LocalDate"/> type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static LocalDate Today(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime.Date;
		}

		/// <summary>
		/// Returns the first day of the month of the specified <see cref="LocalDate"/>.
		/// </summary>
		/// <param name="currentDate"></param>
		/// <returns></returns>
		internal static LocalDate StartOfMonth(this LocalDate currentDate)
		{
			return currentDate.PlusDays(1 - currentDate.Day);
		}

		/// <summary>
		/// Returns the last day of the month of the specified <see cref="LocalDate"/>.
		/// </summary>
		/// <param name="currentDate"></param>
		/// <returns></returns>
		internal static LocalDate EndOfMonth(this LocalDate currentDate)
		{
			return currentDate.StartOfMonth().PlusMonths(1).PlusDays(-1);
		}

		/// <summary>
		/// Returns the first day before of the specified <see cref="LocalDate"/> that has the same <see cref="LocalDate.DayOfWeek"/> as <paramref name="startOfWeek"/>.
		/// </summary>
		/// <param name="currentDate"></param>
		/// <param name="startOfWeek"></param>
		/// <returns></returns>
		internal static LocalDate StartOfWeek(this LocalDate currentDate, IsoDayOfWeek startOfWeek)
		{
			LocalDate local = currentDate;
			while (local.DayOfWeek != startOfWeek)
			{
				local = local.PlusDays(-1);
			}

			return local;
		}

		/// <summary>
		/// Returns the last day of the week containing the specified <see cref="LocalDate"/>, where the week begins on <paramref name="startOfWeek"/>.
		/// </summary>
		/// <param name="currentDate"></param>
		/// <param name="startOfWeek"></param>
		/// <returns></returns>
		internal static LocalDate EndOfWeek(this LocalDate currentDate, IsoDayOfWeek startOfWeek)
		{
			var endOfWeek = startOfWeek.Prev();
			LocalDate local = currentDate;
			while (local.DayOfWeek != endOfWeek)
			{
				local = local.PlusDays(1);
			}

			return local;
		}

		/// <summary>
		/// Returns true if the day is <see cref="IsoDayOfWeek.Saturday"/> or <see cref="IsoDayOfWeek.Sunday"/>, false otherwise.
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		internal static bool IsWeekend(this IsoDayOfWeek day)
		{
			if (day == IsoDayOfWeek.Saturday ||
				day == IsoDayOfWeek.Sunday)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns true if the day is <see cref="IsoDayOfWeek.Monday"/>, <see cref="IsoDayOfWeek.Tuesday"/>, <see cref="IsoDayOfWeek.Wednesday"/>, <see cref="IsoDayOfWeek.Thursday"/> or <see cref="IsoDayOfWeek.Friday"/>, false otherwise.
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		internal static bool IsWeekday(this IsoDayOfWeek day)
		{
			if (day == IsoDayOfWeek.Saturday ||
				day == IsoDayOfWeek.Sunday)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Returns Midnight of the Date of the current <see cref="Instant"/> as a <see cref="LocalDateTime"/> type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static LocalDateTime StartOfToday(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime.Date.AtMidnight();
		}

		/// <summary>
		/// Returns the preceding <see cref="IsoDayOfWeek"/>.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static IsoDayOfWeek Prev(this IsoDayOfWeek source)
		{
			switch (source)
			{
				case IsoDayOfWeek.None:
					return IsoDayOfWeek.None;
				case IsoDayOfWeek.Monday:
					return IsoDayOfWeek.Sunday;
				case IsoDayOfWeek.Tuesday:
					return IsoDayOfWeek.Monday;
				case IsoDayOfWeek.Wednesday:
					return IsoDayOfWeek.Tuesday;
				case IsoDayOfWeek.Thursday:
					return IsoDayOfWeek.Wednesday;
				case IsoDayOfWeek.Friday:
					return IsoDayOfWeek.Thursday;
				case IsoDayOfWeek.Saturday:
					return IsoDayOfWeek.Friday;
				case IsoDayOfWeek.Sunday:
					return IsoDayOfWeek.Saturday;
				default:
					return IsoDayOfWeek.None;
			}
		}

		/// <summary>
		/// Returns the next <see cref="IsoDayOfWeek"/>
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static IsoDayOfWeek Next(this IsoDayOfWeek source)
		{
			switch (source)
			{
				case IsoDayOfWeek.None:
					return IsoDayOfWeek.None;
				case IsoDayOfWeek.Monday:
					return IsoDayOfWeek.Tuesday;
				case IsoDayOfWeek.Tuesday:
					return IsoDayOfWeek.Wednesday;
				case IsoDayOfWeek.Wednesday:
					return IsoDayOfWeek.Thursday;
				case IsoDayOfWeek.Thursday:
					return IsoDayOfWeek.Friday;
				case IsoDayOfWeek.Friday:
					return IsoDayOfWeek.Saturday;
				case IsoDayOfWeek.Saturday:
					return IsoDayOfWeek.Sunday;
				case IsoDayOfWeek.Sunday:
					return IsoDayOfWeek.Monday;
				default:
					return IsoDayOfWeek.None;
			}
		}

		#endregion
		
		/// <summary>
		/// Returns every <see cref="LocalDate"/> in-between <paramref name="startDateTime"/> and <paramref name="endDateTime"/>.
		/// </summary>
		/// <param name="startDateTime"></param>
		/// <param name="endDateTime"></param>
		/// <param name="inclusive">Include <paramref name="startDateTime"/> and <paramref name="endDateTime"/> in the return sequence.</param>
		/// <returns></returns>
		internal static IEnumerable<LocalDate> FullDaysBetween(this LocalDateTime startDateTime, LocalDateTime endDateTime, bool inclusive = false)
		{
			if (endDateTime < startDateTime)
				throw new ArgumentException($"{nameof(endDateTime)} must be later than {nameof(startDateTime)}.");

			var begin = inclusive ? startDateTime.Date : startDateTime.Date.PlusDays(1);
			var end = inclusive ? endDateTime.Date.PlusDays(1) : endDateTime.Date;
			var days = (end - begin).Days;

			for (int i = 0; i < days; i++)
			{
				yield return begin.PlusDays(i);
			}
		}
		
		internal static string ToStringAbbr(this IsoDayOfWeek isoDayOfWeek, System.Globalization.CultureInfo culture = null)
		{
			// Todo: #5
			// Return the Localized version of the week name

			if (culture == null)
				culture = System.Globalization.CultureInfo.InvariantCulture;

			switch (isoDayOfWeek)
			{
				case IsoDayOfWeek.Monday:
					return "Mo";
				case IsoDayOfWeek.Tuesday:
					return "Tu";
				case IsoDayOfWeek.Wednesday:
					return "We";
				case IsoDayOfWeek.Thursday:
					return "Th";
				case IsoDayOfWeek.Friday:
					return "Fr";
				case IsoDayOfWeek.Saturday:
					return "Sa";
				case IsoDayOfWeek.Sunday:
					return "Su";
				case IsoDayOfWeek.None:
				default:
					return string.Empty;
			}
		}

		/// <summary>
		/// Returns the first and last years of the decade that contains <paramref name="year"/>.
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		internal static (int, int) GetDecade(this int year)
		{
			var year0 = (year / 10) * 10; // has the effect of rounding down to first year of current decade
			var year9 = year0 + 9;

			return (year0, year9);
		}

		/// <summary>
		/// Returns the first and last years of the decade that contains <paramref name="date"/>.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		internal static (int, int) GetDecade(this LocalDate date)
		{
			return date.Year.GetDecade();
		}

		internal static string GetDecadeString(this LocalDate date, string format = "{0}-{1}")
		{
			// returns the bounding years of the decade of the specified date
			// e.g. 2018 -> 2010-2019
			//      2001 -> 2000-2009

			var (year0, year9) = date.GetDecade();

			return string.Format(format, year0, year9);
		}

		/// <summary>
		/// Returns the first and last decades of the century that contains <paramref name="year"/>.
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		internal static (int, int) GetCentury(this int year)
		{
			var year0 = (year / 100) * 100; // has the effect of rounding down to first year of current decade
			var year90 = year0 + 90;

			return (year0, year90);
		}
		/// <summary>
		/// Returns the first and last decades of the century that contains <paramref name="date"/>.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		internal static (int, int) GetCentury(this LocalDate date)
		{
			return date.Year.GetCentury();
		}

		internal static string GetCenturyString(this LocalDate date, string format = "{0}-{1}")
		{
			// returns the bounding decades of the century of the specified date
			// e.g. 2018 -> 2000-2090
			//      1985 -> 1900-1990

			var (year0, year90) = date.GetCentury();

			return string.Format(format, year0, year90);
		}
	}
}
