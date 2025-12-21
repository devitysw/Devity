# Installing the package and general setup

You can install the library before using either of the provided elements from NuGet using the following command:

```
Install-Package Devity.Blazor
```

Or via the Visual Studio package manger.

For easier use, add the following to the `_Imports.razor` file for global use.

```
@using Devity.Blazor
```

I would also suggest adding the following using statement to your main _Imports.razor to make referencing the component a bit easier.

# Name Component

Name functionality for forms from ASP.NET Core ported over to Blazor.

For easier use, add the following to the `_Imports.razor` file for global use.

```
@using Devity.Blazor
```

I would also suggest adding the following using statement to your main _Imports.razor to make referencing the component a bit easier.

# DevitySelect Component

Custom implementation of the HTML select element.

## Setup

In the `head` tag add the following css.

```
<link href="_content/Devity.Blazor/devity-select.css" rel="stylesheet" />
```

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

# Toasts Component

Toasts implementation. Here's a usage example:

```
<button @onclick="() => RunToast("This is just a testing notification to showcase how it can look.", Toasts.ToastType.Success)">Show Info Toast</button>

@code {
	[CascadingParameter(Name = "Toasts")]
	public Toasts Toasts { get; set; } = null!;

	private void RunToast(Toasts.ToastType? toastType) => Toasts.RunToast(new("This is just a testing notification to showcase how it can look.", toastType ?? Toasts.ToastType.Info));
}
```

And to actually include the component itself in the MainLayout:
```
<Toasts @ref="_toasts"/>

<CascadingValue Value="_toasts" Name="Toasts">
    <div class="page">
        <div class="sidebar">
            <NavMenu />
        </div>

        <main>
            <article class="content px-4">
                @Body
            </article>
        </main>
    </div>
</CascadingValue>

@code {
    private Toasts? _toasts;

    protected override void OnAfterRender(bool firstRender)
    {
        if(firstRender)
        {
            StateHasChanged();
        }
    }
}
```

In the `head` tag add the following css.

```
<link href="_content/Devity.Blazor/lib.css" rel="stylesheet" />
```