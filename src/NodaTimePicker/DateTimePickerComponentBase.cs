using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTimePicker
{
	public class DateTimePickerComponentBase : PickerComponentBase
	{
		#region Parameters

		[Parameter] public bool Inline { get; set; }

		#endregion

		public string CssClass
		{
			get
			{
				if (!string.IsNullOrEmpty(Class))
				{
					return $"{CssClassGenerator.DateTimePickerMain(Inline)} {Class}";
				}
				else
				{
					return CssClassGenerator.DateTimePickerMain(Inline);
				}
			}
		}
	}
}
