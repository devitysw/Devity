### Installing the package

You can install either of the provided elements from NuGet using the following command:

```
Install-Package Devity.Blazor
```

Or via the Visual Studio package manger.

### Name

Name functionality for forms from ASP.NET Core ported over to Blazor.

For easier use, add the following to the `_Imports.razor` file for global use.

```
@using Devity.Blazor
```

I would also suggest adding the following using statement to your main _Imports.razor to make referencing the component a bit easier.

### Select

Custom implementation of the HTML select element.

## Setup

In the `head` tag add the following css.

```
<link href="_content/Devity.Blazor/devity-select.css" rel="stylesheet" />
```

For easier use, add the following to the `_Imports.razor` file for global use.

```
@using Devity.Blazor
```

I would also suggest adding the following using statement to your main _Imports.razor to make referencing the component a bit easier.

## Example

```
<DevitySelect Items="_options" TValue="FetchedType?" @bind-Value="_selectedValue" @bind-Value:after="OnValueSelected" />

@code {
    private Dictionary<string, FetchedType?> _options = new();
    private FetchedType? _selectedValue;

    protected override void OnInitialized() 
    {
        _options.Add(string.Empty, null);
        foreach(var value in Enum.GetValues<FetchedType>())
        {
            _options.Add(value.GetDisplayName(), value);
        }
    }

    private async void OnValueSelected()
    {
        // TODO: Callback
    }
}
```

Enum class used in the example:
```
public enum FetchedType
{
    [Display(Name = "Example 1")]
    Example1,

    [Display(Name = "Example 2")]
    Example2
}
```

## Styling

To apply styling, you can target the following classes:
```
.devity-select
.devity-select-toggle
.devity-select-value
.devity-select-arrow
.devity-select-options
.devity-select-option
.devity-select-option input
.devity-select-option label
```
