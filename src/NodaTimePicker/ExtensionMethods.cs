namespace NodaTimePicker
{
	internal static class ExtensionMethods
	{
		/// <summary>
		/// Given a year, returns the first year of the current decade.
		/// </summary>
		/// <example>
		/// <code>
		/// int year = 2015;
		/// int decade = year.Decade();
		/// // decade = 2010
		/// </code>
		/// </example>
		/// <param name="year">The year within the decade to return.</param>
		/// <returns></returns>
		internal static int Decade(this int year)
		{
			return (year / 10) * 10;
		}
	}
}
