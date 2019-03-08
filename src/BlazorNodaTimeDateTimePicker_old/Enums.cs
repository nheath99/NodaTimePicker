namespace BlazorNodaTimeDateTimePicker
{
	/// <summary>Represents the type of view to display.</summary>
	public enum ViewMode
	{
		/// <summary>Displays days of the month</summary>
		Days,
		/// <summary>Displays weeks of the month</summary>
		Weeks,
		/// <summary>Displays the months in a single year</summary>
		Months,
		/// <summary>Displays the years of a single decade</summary>		
		Years,
		/// <summary>Displays the decades of a single century</summary>
		Decades
	}

	internal enum TimeViewMode
	{
		Clock = 0,
		Hours = 1,
		Minutes = 2
	}
}
