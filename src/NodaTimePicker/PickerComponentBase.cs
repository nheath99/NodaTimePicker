using Microsoft.AspNetCore.Components;

namespace NodaTimePicker
{
	/// <summary>Base class for any picker components.</summary>
	/// <typeparam name="TState"></typeparam>
	public abstract class PickerComponentBase : ComponentBase
	{ 
		/// <summary>Any CSS classes to be applied to the wrapper element.</summary>
		[Parameter] protected string Class { get; set; }
		/// <summary>Any CSS styles to be applied to the wrapper element.</summary>
		[Parameter] protected string Style { get; set; }
		/// <summary>The maximum width of the wrapper element. Must be a valid CSS width value.</summary>
		[Parameter] protected string MaxWidth { get; set; }
		/// <summary>The width of the wrapper element. Must be a valid CSS width value.</summary>
		[Parameter] protected string Width { get; set; } = "250px";
		/// <summary>If true and <see cref="Inline"/> is true, the Clear button will be displayed. If false, or <see cref="Inline"/> is false, it will be hidden.</summary>
		[Parameter] protected bool ShowClose { get; set; } = false;
		// Parameters not yet in use
		[Parameter] protected bool CloseOnSelect { get; set; } = true;			
	}
}
