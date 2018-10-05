using NodaTime;
using Xunit;
using BlazorNodaTimeDateTimePicker;

namespace BlazorNodaTimeDateTimePicker.Tests
{
	public class NodaTimeExtensionMethodUnitTests
	{
		[Theory]
		[InlineData(2018, 1, 1)]
		[InlineData(2018, 1, 2)]
		[InlineData(2018, 1, 3)]
		[InlineData(2018, 1, 4)]
		[InlineData(2018, 1, 5)]
		[InlineData(2018, 1, 6)]
		[InlineData(2018, 1, 30)]
		[InlineData(2018, 1, 31)]
		[InlineData(2018, 2, 1)]
		[InlineData(2018, 2, 2)]
		[InlineData(2018, 2, 3)]
		[InlineData(2018, 2, 27)]
		[InlineData(2018, 2, 28)]
		public void StartOfMonth(int year, int month, int day)
		{
			// Arrange
			var currentDate = new LocalDate(year, month, day);

			// Act
			var result = currentDate.StartOfMonth();

			// Assert
			Assert.Equal(year, result.Year);
			Assert.Equal(month, result.Month);
			Assert.Equal(1, result.Day);
		}

		[Theory]
		[InlineData(2018, 1, 1)]
		[InlineData(2018, 1, 2)]
		[InlineData(2018, 1, 3)]
		[InlineData(2018, 1, 4)]
		[InlineData(2018, 1, 5)]
		[InlineData(2018, 1, 6)]
		[InlineData(2018, 1, 30)]
		[InlineData(2018, 1, 31)]
		public void EndOfMonth(int year, int month, int day)
		{
			// Arrange
			var currentDate = new LocalDate(year, month, day);

			// Act
			var result = currentDate.EndOfMonth();

			// Assert
			Assert.Equal(year, result.Year);
			Assert.Equal(month, result.Month);
			Assert.Equal(31, result.Day);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Monday)]
		[InlineData(IsoDayOfWeek.Tuesday)]
		[InlineData(IsoDayOfWeek.Wednesday)]
		[InlineData(IsoDayOfWeek.Thursday)]
		[InlineData(IsoDayOfWeek.Friday)]
		public void IsWeekday_true(IsoDayOfWeek dow)
		{
			// Arrange
			// Act
			var result = dow.IsWeekday();

			// Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Saturday)]
		[InlineData(IsoDayOfWeek.Sunday)]
		public void IsWeekday_false(IsoDayOfWeek dow)
		{
			// Arrange
			// Act
			var result = dow.IsWeekday();

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Saturday)]
		[InlineData(IsoDayOfWeek.Sunday)]
		public void IsWeekend_true(IsoDayOfWeek dow)
		{
			// Arrange
			// Act
			var result = dow.IsWeekend();

			// Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Monday)]
		[InlineData(IsoDayOfWeek.Tuesday)]
		[InlineData(IsoDayOfWeek.Wednesday)]
		[InlineData(IsoDayOfWeek.Thursday)]
		[InlineData(IsoDayOfWeek.Friday)]
		public void IsWeekend_false(IsoDayOfWeek dow)
		{
			// Arrange
			// Act
			var result = dow.IsWeekend();

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Monday, IsoDayOfWeek.Tuesday)]
		[InlineData(IsoDayOfWeek.Tuesday, IsoDayOfWeek.Wednesday)]
		[InlineData(IsoDayOfWeek.Wednesday, IsoDayOfWeek.Thursday)]
		[InlineData(IsoDayOfWeek.Thursday, IsoDayOfWeek.Friday)]
		[InlineData(IsoDayOfWeek.Friday, IsoDayOfWeek.Saturday)]
		[InlineData(IsoDayOfWeek.Saturday, IsoDayOfWeek.Sunday)]
		[InlineData(IsoDayOfWeek.Sunday, IsoDayOfWeek.Monday)]
		[InlineData(IsoDayOfWeek.None, IsoDayOfWeek.None)]
		public void Next(IsoDayOfWeek candidate, IsoDayOfWeek expected)
		{
			// Arrange
			// Act
			var result = candidate.Next();

			// Assert
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(IsoDayOfWeek.Monday, IsoDayOfWeek.Sunday)]
		[InlineData(IsoDayOfWeek.Tuesday, IsoDayOfWeek.Monday)]
		[InlineData(IsoDayOfWeek.Wednesday, IsoDayOfWeek.Tuesday)]
		[InlineData(IsoDayOfWeek.Thursday, IsoDayOfWeek.Wednesday)]
		[InlineData(IsoDayOfWeek.Friday, IsoDayOfWeek.Thursday)]
		[InlineData(IsoDayOfWeek.Saturday, IsoDayOfWeek.Friday)]
		[InlineData(IsoDayOfWeek.Sunday, IsoDayOfWeek.Saturday)]
		[InlineData(IsoDayOfWeek.None, IsoDayOfWeek.None)]
		public void Prev(IsoDayOfWeek candidate, IsoDayOfWeek expected)
		{
			// Arrange
			// Act
			var result = candidate.Prev();

			// Assert
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(2010, 2010, 2019)]
		[InlineData(2011, 2010, 2019)]
		[InlineData(2012, 2010, 2019)]
		[InlineData(2013, 2010, 2019)]
		[InlineData(2014, 2010, 2019)]
		[InlineData(2015, 2010, 2019)]
		[InlineData(2016, 2010, 2019)]
		[InlineData(2017, 2010, 2019)]
		[InlineData(2018, 2010, 2019)]
		[InlineData(2019, 2010, 2019)]
		[InlineData(2020, 2020, 2029)]
		[InlineData(2021, 2020, 2029)]
		[InlineData(2022, 2020, 2029)]
		[InlineData(2023, 2020, 2029)]
		[InlineData(2024, 2020, 2029)]
		[InlineData(2025, 2020, 2029)]
		[InlineData(2026, 2020, 2029)]
		[InlineData(2027, 2020, 2029)]
		[InlineData(2028, 2020, 2029)]
		[InlineData(2029, 2020, 2029)]
		[InlineData(2030, 2030, 2039)]
		[InlineData(2031, 2030, 2039)]
		public void GetDecade(int year, int year0, int year9)
		{
			// Arrange
			// Act
			var (result0, result9) = year.GetDecade();

			// Assert
			Assert.Equal(year0, result0);
			Assert.Equal(year9, result9);
		}
	}
}
