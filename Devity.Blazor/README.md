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

`lib.css` contains the shared generated utility styles used by `DevityDialog` and `Toasts`.

Optional primary color override:

```css
:root {
    --devity-primary-color: #7c3aed;
    --devity-primary-hover-color: #6d28d9;
}
```

`DevityDialog` uses this primary color for its default submit button.

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

### `DevityDialog`

`DevityDialog` renders a reusable modal dialog with overlay fade and panel slide animations. Reference the component and call `Show`, `Close`, `Cancel`, or `Submit` from your component code.

```razor
<DevityDialog @ref="_dialog"
              Title="Add label"
              OnSubmitAttempted="Save"
              OnCancel="Reset"
              Large>
    <EditForm Model="_model">
        <InputText @bind-Value="_model.Name" class="form-control" />
    </EditForm>
</DevityDialog>

<button type="button" @onclick="Open">Open</button>

@code {
    private DevityDialog? _dialog;
    private LabelModel _model = new();

    private async Task Open()
    {
        _model = new();

        if (_dialog is not null)
            await _dialog.Show();
    }

    private async Task Save()
    {
        if (_dialog is not null)
            await _dialog.Close();
    }

    private void Reset()
    {
        _model = new();
    }

    private sealed class LabelModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
```

For inherited dialog components, use `DialogBase`:

`EditNameDialog.razor`

```razor
@inherits DialogBase

<DevityDialog @ref="Dialog"
              Title="Edit name"
              SubmitButtonText="Save"
              OnSubmitAttempted="Save">
    <EditForm Model="_model">
        <Name For="() => _model.Name" class="form-label" />
        <InputText @bind-Value="_model.Name" class="form-control" @ref="FocusElement" />
    </EditForm>
</DevityDialog>

@code {
    [Parameter]
    public EventCallback<string> OnSubmit { get; set; }

    private EditNameModel _model = new();

    public void Open(string name)
    {
        _model = new()
        {
            Name = name
        };

        base.Open();
    }

    private async Task Save()
    {
        await OnSubmit.InvokeAsync(_model.Name);

        if (Dialog is not null)
            await Dialog.Close();
    }

    private sealed class EditNameModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
```

`ExamplePage.razor`

```razor
@page "/dialog-example"

<h1>Dialog example</h1>

<p>Current name: @_name</p>

<button type="button" @onclick="OpenDialog">Edit name</button>

<EditNameDialog @ref="_editNameDialog" OnSubmit="SaveName" />

@code {
    private EditNameDialog? _editNameDialog;
    private string _name = "Example";

    private void OpenDialog()
    {
        _editNameDialog?.Open(_name);
    }

    private void SaveName(string name)
    {
        _name = name;
    }
}
```

Supported parameters:

- `Title`: title text used by the default header
- `ChildContent`: required dialog body content
- `HeaderContent`, `FooterContent`, `CloseButtonContent`: optional custom regions
- `OnShown`, `OnCancel`, `OnClose`, `OnSubmitAttempted`: lifecycle callbacks
- `ShowHeader`, `ShowFooter`, `ShowCloseButton`: toggle default regions and close button
- `CloseOnOverlayClick`, `CloseOnEscape`, `SubmitOnEnter`: keyboard and overlay behavior
- `Large`, `FullWidth`, `DangerSubmit`: default layout and submit button variants
- `CancelButtonText`, `SubmitButtonText`, `CloseButtonTitle`: default control text
- `Class`: extra classes applied to the dialog panel
- `OverlayClass`: extra classes applied to the overlay
- `AnimationMilliseconds`: close animation delay before the dialog is removed

Styling hooks:

```css
.devity-dialog-overlay
.devity-dialog-panel
.devity-dialog-header
.devity-dialog-title
.devity-dialog-body
.devity-dialog-footer
.devity-dialog-close-button
.devity-dialog-cancel-button
.devity-dialog-submit-button
.devity-dialog-submit-button-danger
```

Override those classes in your app stylesheet for app-specific spacing, buttons, widths, colors, or stacking behavior such as `z-index`.

Keyboard behavior:

- `Tab` and `Shift+Tab` are trapped inside the open dialog.
- `Escape` cancels the dialog when `CloseOnEscape` is `true`.
- `Enter` invokes `OnSubmitAttempted` when `SubmitOnEnter` is `true`.

### `FocusTrap`

`FocusTrap` keeps keyboard focus inside its content while `Active` is `true`. It is used by `DevityDialog`, but can also wrap custom popovers, drawers, or other modal UI.

```razor
<FocusTrap Active="_open" Class="my-overlay">
    <div class="my-panel">
        <button type="button">First action</button>
        <button type="button">Second action</button>
    </div>
</FocusTrap>
```

Supported parameters:

- `ChildContent`: required trapped content
- `Active`: enables or disables the trap
- `FocusOnActivate`: focuses the first input, first focusable element, or trap container when activated
- `Class`: classes applied to the trap container
- `TabIndex`: trap container tabindex, defaults to `-1`

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
