# Devity.Extensions

`Devity.Extensions` contains small helpers and extension methods for common .NET types, plus a simple string-template utility.

## Installation

```powershell
Install-Package Devity.Extensions
```

## Namespaces

Most APIs live under:

```csharp
using Devity.Extensions;
using Devity.Extensions.Helpers;
using Devity.Extensions.Templates;
```

## Included APIs

### Strings

- `Shorten(int maxLength)`: trims a string and appends `...` when needed.
- `ToFormattedIban()`: formats an IBAN into 4-character groups.
- `Has(string needle)`: search helper that also compares normalized text.
- `NormalizeForSearch()`: lowercases text and removes spaces, punctuation, dashes, and diacritics.

```csharp
var title = "Very long title".Shorten(8);           // Very lon...
var iban = "CZ6508000000192000145399".ToFormattedIban();
var matches = "Příliš žluťoučký kůň".Has("zlutoucky kun");
```

### Dates and times

- `DateTime.ToHtmlDateString()`
- `DateTime.ToHtmlString()`
- `DateTime.ToReadableString()`
- `DateTime.ToReadableStringWithTime()`
- `DateTime.GetUntilEndOfDay()`
- `DateTime.GetUntilEndOfMonth()`
- `DateTime.IsWithinRange(start, end)`
- `IEnumerable<TimeSpan>.Sum()`
- `DateOnly.ToReadableString()`

```csharp
var htmlDate = DateTime.UtcNow.ToHtmlDateString();
var display = DateTime.Now.ToReadableStringWithTime();
var expiresIn = DateTime.Now.GetUntilEndOfDay();
```

### Numbers

- `long.ToHumanReadableSize()`: converts raw bytes to `B`, `KiB`, `MiB`, and so on.

```csharp
var size = 15360L.ToHumanReadableSize(); // 15 KiB
```

### Enums and reflection helpers

- `Enum.GetDisplayName()`: reads `DisplayAttribute.Name` when present.
- `ClassFacade.GetPropertyHumanName(PropertyInfo)`: display name fallback for properties.
- `ClassFacade.GetCleanType(object)`: friendly type name, including generic arguments.

```csharp
public enum Status
{
    [Display(Name = "In progress")]
    InProgress
}

var label = Status.InProgress.GetDisplayName();
```

### JSON

- `object.ToJson(bool indented = true)`: serializes with `System.Text.Json`.
- `JsonHelper.INDENTED_OPTIONS`: shared indented serializer options.

```csharp
var json = new { Name = "Devity" }.ToJson();
```

### LINQ and EF Core

- `IQueryable<T>.DistinctByDb(...)`: database-friendly `DistinctBy` implemented via `GroupBy(...).First()`.

```csharp
var users = db.Users.DistinctByDb(x => x.Email);
```

## Templates

The template helper is a minimal file-based HTML/text templating system.

### Supported features

- Key replacement with `AddKey`
- Conditional block inclusion/exclusion with `AddCondition`
- Repeated block generation with `AddLoop`

### Basic example

Template file:

```html
<h1>-TITLE-</h1>
<p>Hello -NAME-</p>
```

Usage:

```csharp
var template = new DevityTemplate("Templates/welcome.html")
    .AddKey("-TITLE-", "Welcome")
    .AddKey("-NAME-", "Dave");

var html = template.PopulateTemplate();
```

### Conditional blocks

Use the same marker at the start and end of the section you want to conditionally keep.

```html
-SHOW-ADMIN-
<p>Admin content</p>
-SHOW-ADMIN-
```

```csharp
template.AddCondition("-SHOW-ADMIN-", currentUser.IsAdmin);
```

### Loops

Use the same loop marker to wrap the repeated block.

```html
-ITEMS-
<li>-NAME-: -PRICE-</li>
-ITEMS-
```

```csharp
var items = new List<CartItem>
{
    new() { Name = "Book", Price = 10 },
    new() { Name = "Pen", Price = 2 }
};

var loop = new DevityTemplateLoop<CartItem>(items)
    .AddKey("-NAME-", x => x.Name)
    .AddKey("-PRICE-", x => x.Price);

var template = new DevityTemplate("Templates/cart.html")
    .AddLoop("-ITEMS-", loop);
```
