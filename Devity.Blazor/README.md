# Devity.Blazor

`Devity.Blazor` contains a few reusable Blazor components built for forms and lightweight UI feedback.

## Installation

```powershell
Install-Package Devity.Blazor
```

## Setup

Add the namespace to `_Imports.razor`:

```razor
@using Devity.Blazor
```

Add the package styles to your layout or app host:

```html
<link href="_content/Devity.Blazor/devity-select.css" rel="stylesheet" />
<link href="_content/Devity.Blazor/lib.css" rel="stylesheet" />
```

`lib.css` contains the shared generated utility styles used by `Toasts`.

## Components

### `Name<TValue>`

`Name` renders a `<label>` from a property expression. It uses `DisplayAttribute.Name` when present and otherwise falls back to the property name.

```razor
<EditForm Model="_model">
    <Name For="() => _model.FirstName" class="form-label" />
    <InputText @bind-Value="_model.FirstName" class="form-control" />
</EditForm>

@code {
    private PersonForm _model = new();

    public sealed class PersonForm
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;
    }
}
```

Notes:

- `For` is required.
- Extra HTML attributes are passed through to the rendered `<label>`.

### `DevitySelect<TValue>`

`DevitySelect` is a custom select component backed by a `Dictionary<string, TValue?>`, where the dictionary key is the text shown to the user.

```razor
<DevitySelect Items="_options"
              @bind-Value="_selectedValue"
              DefaultShowcasedText="Choose a value" />

@code {
    private Dictionary<string, Status?> _options = new()
    {
        { string.Empty, null },
        { "Success", Status.Success },
        { "Warning", Status.Warning },
        { "Error", Status.Error }
    };

    private Status? _selectedValue;

    public enum Status
    {
        Success,
        Warning,
        Error
    }
}
```

Supported parameters:

- `Items`: required options dictionary
- `Value`, `ValueChanged`, `ValueExpression`: standard bindable value parameters
- `DefaultShowcasedText`: placeholder text when nothing is selected
- `Style`: inline style applied to the root element
- `OnOpen`: callback invoked when the dropdown opens
- `OnClosed`: callback invoked after the close animation finishes
- `ConvertMethod`: optional conversion function before assigning `Value`

Styling hooks:

```css
.devity-select
.devity-select-toggle
.devity-select-value
.devity-select-arrow
.devity-select-options
.devity-select-option
.devity-select-option input
.devity-select-option label
```

### `Toasts`

`Toasts` renders temporary notifications with built-in success, info, warning, and error styles.

Add the component near the root of your layout and cascade it to descendants:

```razor
<Toasts @ref="_toasts" />

<CascadingValue Value="_toasts" Name="Toasts">
    @Body
</CascadingValue>

@code {
    private Toasts? _toasts;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            StateHasChanged();
        }
    }
}
```

Use it from a child component:

```razor
<button @onclick="ShowToast">Show toast</button>

@code {
    [CascadingParameter(Name = "Toasts")]
    public Toasts Toasts { get; set; } = null!;

    private void ShowToast()
    {
        Toasts.RunToast(new Toasts.Toast(
            "Saved successfully.",
            Toasts.ToastType.Success,
            3000));
    }
}
```

Available types:

- `Toasts.ToastType.Success`
- `Toasts.ToastType.Info`
- `Toasts.ToastType.Warning`
- `Toasts.ToastType.Error`

## Local stylesheet build

When developing this package locally, rebuild the generated stylesheet after changing `app.css`:

```bash
npm install
npm run build:css
```
