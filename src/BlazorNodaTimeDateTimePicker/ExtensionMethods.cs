using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNodaTimeDateTimePicker
{
    public static class ExtensionMethods
    {
		public static int Decade(this int year)
		{
			return (year / 10) * 10;
		}
    }
}
