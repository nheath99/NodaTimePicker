# BlazorNodaTimeDateTimePicker
A Date/Time picker for [Blazor](https://github.com/aspnet/Blazor), using [NodaTime](https://github.com/nodatime/nodatime).

View the [Demo](https://nodatimepicker.azurewebsites.net/)

The aim of this project is to develop Date/Time picker components for Blazor applications, using NodaTime as the only dependency for Date and Time calculations, and CSS Grid for layout.

It is inspired by the very successful Javascript Date/Time picker developed by [TempusDominus](https://github.com/tempusdominus/bootstrap-4), though any dependencies on Bootstrap or jQuery have been removed - it is not simply a port of the code or interop, but a fresh start from a zero base.

## Installation

[![NuGet Pre Release](https://img.shields.io/badge/nuget-v0.0.5-orange.svg)](https://www.nuget.org/packages/BlazorNodaTimeDateTimePicker/)

## Getting Started

The easiest way to get started is to look at the Demo project, which has samples for most of the functionality.

The library can be downloaded from NuGet by searching for: BlazorNodaTimeDateTimePicker in NuGet Package Manager, or by executing the following command in the Package Manager Console:
````shell
PM> Install-Package BlazorNodaTimeDateTimePicker -Version 0.0.5
````
Until the issue https://github.com/aspnet/Blazor/issues/1315 is resolved, insert the following line into your *_ViewImports.cshtml* file:
````C#
@addTagHelper *, BlazorNodaTimeDateTimePicker
````
### Inline

To display a simple inline DatePicker, use the following code:
````C#
<DatePicker Inline=true />
````
![DatePicker1](/docs/images/DatePicker1.png)

## Bound to Input

To bind a DatePicker to an Input element, use Blazor event bindings:
````C#
<input type="text" onfocus=@focussed />
<DatePicker Visible=@visible Selected=@selected />

@functions {
  bool visible = false;
  void focussed(UIFocusEventArgs e)
  {
    visible = true;
  }
  void selected(LocalDate localDate)
  {
	visible = false;
	StateHasChanged();
  }
}
````
![DatePicker1](/docs/images/DatePicker_BoundToInput.png)

### Localization

Display day and month names in the specified culture:
````C#
<DatePicker Inline=true FormatProvider="@(new System.Globalization.CultureInfo("fr-FR"))" />
````
![DatePicker Localization](/docs/images/DatePicker_Localization.png)
![DatePicker Localization Months](/docs/images/DatePicker_Localization_Months.png)

### First Day of Week

Specify any weekday as the first day of the week (default is Monday):

````C#
<DatePicker Inline=true FirstDayOfWeek=IsoDayOfWeek.Thursday />
````
![DatePicker First Day of Week](/docs/images/DatePicker_FirstDayOfWeek.png)

### Disabled Days of Week

Disable specific days of the week:
````C#
<DatePicker Inline=true DaysOfWeekDisabled=@(new IsoDayOfWeek[] { IsoDayOfWeek.Monday, IsoDayOfWeek.Wednesday }) />
````
![DatePicker Disabled Days of Week](/docs/images/DatePicker_DaysOfWeekDisabled.png)

## Help Wanted!
Get in touch if you want to collaborate, there's plenty of features to add and plenty to improve!

## Future Development
- Time picker
- Date+Time picker
- Date range picker
- Date+Time range picker
- Month picker
- Year picker
- Support for different calendars
- Timezone awareness
- Locale support
