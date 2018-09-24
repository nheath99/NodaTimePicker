using NodaTime;
using System;
using System.Collections.Generic;

namespace BlazorNodaTimeDateTimePicker
{
	internal static class NodaTimeExtensionMethods
	{
		#region IClock

		/// <summary>
		/// Returns a representation of the Current Instant as a LocalDateTime type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static LocalDateTime Now(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime;
		}

		/// <summary>
		/// Returns the Date of the Current Instant as a LocalDate type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static LocalDate Today(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime.Date;
		}

		public static LocalDate StartOfMonth(this LocalDate currentDate)
		{
			return currentDate.PlusDays(1 - currentDate.Day);
		}

		public static LocalDate EndOfMonth(this LocalDate currentDate)
		{
			return currentDate.StartOfMonth().PlusMonths(1).PlusDays(-1);
		}

		public static LocalDate StartOfWeek(this LocalDate currentDate, IsoDayOfWeek startOfWeek)
		{
			LocalDate local = currentDate;
			while (local.DayOfWeek != startOfWeek)
			{
				local = local.PlusDays(-1);
			}

			return local;
		}

		public static LocalDate EndOfWeek(this LocalDate currentDate, IsoDayOfWeek startOfWeek)
		{
			var endOfWeek = startOfWeek.Prev();
			LocalDate local = currentDate;
			while (local.DayOfWeek != endOfWeek)
			{
				local = local.PlusDays(1);
			}

			return local;
		}

		public static bool IsWeekend(this IsoDayOfWeek day)
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

		public static bool IsWeekday(this IsoDayOfWeek day)
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
		/// Returns Midnight of the Date of the Current Instant as a LocalDateTime type in UTC.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static LocalDateTime StartOfToday(this IClock source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.GetCurrentInstant().InUtc().LocalDateTime.Date.AtMidnight();
		}

		public static IsoDayOfWeek Prev(this IsoDayOfWeek source)
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

		public static IsoDayOfWeek Next(this IsoDayOfWeek source)
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

		public static IEnumerable<LocalDate> FullDaysBetween(this LocalDateTime startDateTime, LocalDateTime endDateTime, bool inclusive = false)
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
	}
}
