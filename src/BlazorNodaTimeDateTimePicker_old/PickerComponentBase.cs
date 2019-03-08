using Microsoft.AspNetCore.Components;

namespace BlazorNodaTimeDateTimePicker
{
	/// <summary>Base class for any picker components.</summary>
	/// <typeparam name="TState"></typeparam>
	public abstract class PickerComponentBase<TState> : ComponentBase where TState : PickerStateBase, new()
	{ 
		/// <summary>The state machine for the current picker.</summary>
		internal TState State { get; set; } = new TState();
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
		/// <summary>Whether the picker should be visible or hidden.</summary>
		[Parameter]
		protected bool Visible
		{
			get => State.Visible;
			set => State.Visible = value;
		}
		/// <summary>Whether the <see cref="TimePicker"/> should display Inline or not.</summary>
		[Parameter]
		protected bool Inline
		{
			get => State.Inline;
			set => State.Inline = value;
		}
	}
}
