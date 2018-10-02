namespace BlazorNodaTimeDateTimePicker
{
	internal static class ExtensionMethods
    {
		internal static int Decade(this int year)
		{
			return (year / 10) * 10;
		}
    }
}
