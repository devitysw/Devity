# Devity API Index (LLM-ready)

Quick-reference surface of the shared Devity libraries — names and signatures only, so an LLM can
check "does this already exist?" without grepping the source. Read the actual file under each
project (path given) only if you need real implementation details or plan to use/modify it.

Update this file whenever a public member is added, renamed, or removed in any project below.

## Devity.Blazor — shared UI components

- **`DevityDialog`** (`Devity.Blazor/DevityDialog.razor`) — modal dialog.
  Parameters: `Id`, `Title`, `ChildContent`, `HeaderContent`, `FooterContent`,
  `CloseButtonContent`, `OnCancel`, `OnSubmitAttempted`, `OnClose`, `OnShown`, `ShowHeader`,
  `ShowFooter`, `ShowCloseButton`, `CloseOnOverlayClick`, `CloseOnEscape`, `SubmitOnEnter`,
  `Large`, `FullWidth`, `DangerSubmit`, `CancelButtonText`, `SubmitButtonText`,
  `CloseButtonTitle`, `OverlayClass`, `Class`, `AnimationMilliseconds`.
  Methods: `Show()`, `Close()`, `Cancel()`, `Submit()` (all `Task`).
- **`DialogBase`** (`Devity.Blazor/DialogBase.razor`) — base class to inherit for a component that
  wraps/drives a `DevityDialog`. Members: `Open()`, `OpenAsync()` (auto-focuses `FocusElement`:
  `InputText`/`InputTextArea`/`ElementReference`), overridable `OnOpen()`.
- **`DevitySelect<TValue>`** (`Devity.Blazor/DevitySelect.razor`) — generic searchable select bound
  like a normal Blazor input. Parameters: `Items` (`Dictionary<string, TValue?>`), `Value`,
  `ValueChanged`, `ValueExpression`, `ConvertMethod`, `DefaultShowcasedText`, `Style`, `OnOpen`,
  `OnClosed`.
- **`FocusTrap`** (`Devity.Blazor/FocusTrap.razor`) — traps keyboard focus within `ChildContent`.
  Parameters: `ChildContent`, `Active`, `FocusOnActivate`, `Class`, `TabIndex`.
  `@implements IAsyncDisposable`.
- **`Name<TValue>`** (`Devity.Blazor/Name.razor`) — renders a display name for a bound field via
  `For` (`Expression<Func<TValue>>`), e.g. for labels driven by data annotations.
- **`Toasts`** (`Devity.Blazor/Toasts.razor`) — toast notification host component. Nested
  `record Toast(string Text, ToastType Type = ToastType.Info, int DurationMilliseconds = 5000)`.
  Instance methods: `RunToast(Toast)`, `KillToast(Toast)`, `Hide(Toast)`.

## Devity.Extensions — shared C# helpers/extension methods

`Devity.Extensions/DateOnlyExtensions.cs`
- `DateOnly.ToReadableString()`

`Devity.Extensions/DateTimeExtensions.cs`
- `DateTime.ToHtmlDateString()`, `.ToHtmlString()`, `.ToReadableString()`,
  `.ToReadableStringWithTime()`
- `DateTime.GetUntilEndOfDay()`, `.GetUntilEndOfMonth()`
- `DateTime.IsWithinRange(...)`
- `IEnumerable<TimeSpan>.Sum()`

`Devity.Extensions/EnumExtensions.cs`
- `Enum.GetDisplayName()` — reads `[Display(Name = ...)]`.

`Devity.Extensions/JsonExtensions.cs`
- `object.ToJson(bool indented = true)`

`Devity.Extensions/NumberExtensions.cs`
- `long.ToHumanReadableSize()`

`Devity.Extensions/QueryableExtensions.cs`
- `IQueryable<TSource>.DistinctByDb<TSource, TKey>(keySelector)` — `DistinctBy` that works as pure
  `IQueryable` (EF Core-safe).

`Devity.Extensions/SearchExtensions.cs`
- `string.Has(string needle)`, `string.NormalizeForSearch()`

`Devity.Extensions/StringExtensions.cs`
- `string.Shorten(int maxLength)`, `string.ToFormattedIban()`

`Devity.Extensions/Helpers/ClassFacade.cs`
- `ClassFacade.GetPropertyHumanName(PropertyInfo)`, `ClassFacade.GetCleanType(object)`

`Devity.Extensions/Helpers/DateHelper.cs`
- `DateHelper.GetFirstDayOfMonth([DateTime])`, `.GetLastDayOfMonth([DateTime])`,
  `.DateAndTimeToDateTime(date, time)`, `.DoDateRangesIntersect(...)`

`Devity.Extensions/Helpers/JsonHelper.cs`
- `JsonHelper.INDENTED_OPTIONS` — a `JsonSerializerOptions` with `WriteIndented = true`.

`Devity.Extensions/Templates/` — simple string-template engine:
- `DevityTemplate(string bodyPath)`, `.AddKey(key, value)`, `.AddLoop(key, loop)`,
  `.AddCondition(key, bool)`
- `DevityTemplateLoop<T>(List<T>)`, `.AddKey(key, Expression<Func<T, dynamic>>)`
- `DevityTemplate.PopulateTemplate()` (extension method, in `TemplateExtensions.cs`)

## Devity.Mailing — transactional email

`Devity.Mailing/ServiceCollectionExtensions.cs`
- `IServiceCollection.AddDevityMailing<T>(ConfigurationManager)` where `T : CommonMailService` —
  reads a `"Email"` config section shaped like `MailKitOptions`.
- `IServiceCollection.AddDevityMailing<T>(MailKitOptions)` — same, options passed directly.

`Devity.Mailing/CommonMailService.cs`
- `abstract class CommonMailService(IEmailService mailService, string subjectFormat)` — inherit
  this for an app's own mail service.

`Devity.Mailing/DevityEmail.cs`
- `DevityEmail(string emailAddress, string subjectMessage, DevityTemplate template)` —
  `.AddAttachment(path)`. Properties: `EmailAddress`, `SubjectMessage`, `Template`, `Attachments`.

## Devity.Payout — payment/payout integration

`Devity.Payout/PayoutService.cs`
- `PayoutService(IConfiguration, ILogger<PayoutService>)`
- `GetTokenAsync()`
- `CreateCheckoutAsync(...)` → `PayoutCheckoutResponseDTO`
- `GetCheckoutAsync(...)` → `PayoutCheckoutDTO`
- `CreateRefundAsync(...)` → `PayoutRefundResponseDTO`
- `GetBalanceAsync()` → `PayoutBalanceResponseDTO[]`

`Devity.Payout/PayoutWebhookType.cs` — enum: `WithdrawalPaid`, `WithdrawalFailed`,
`CheckoutSucceeded`, `CheckoutExpired`, `CheckoutRefundRequest`, `CheckoutRefunded`,
`CheckoutRefundFailed`.

DTOs live under `Devity.Payout/DTOs/` (`PayoutCheckoutDTO`, `PayoutCustomerDTO`,
`PayoutProductDTO`, `PayoutRefundDTO`, `PayoutWebhookDTO`, etc.) and exceptions under
`Devity.Payout/Exceptions/` (`PayoutBalanceUnavailableException`) — check those directly for
field-level shape when actually integrating.
