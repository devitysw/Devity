# Devity.Mailing

`Devity.Mailing` provides a base service for sending template-based emails and a DI registration helper built on top of `Devity.NETCore.MailKit`.

## Installation

```powershell
Install-Package Devity.Mailing
```

## Dependencies

This package expects:

- `Devity.Extensions` templates for email body generation
- `Devity.NETCore.MailKit` for the underlying mail transport

## Main Types

- `CommonMailService`: base class for your app-specific mail service
- `DevityEmail`: email payload with recipient, subject fragment, template, and attachments
- `AddDevityMailing<T>()`: DI registration helper

## Create a mail service

Derive your own service from `CommonMailService`.

```csharp
using Devity.Extensions.Templates;
using Devity.Mailing;
using Devity.NETCore.MailKit.Core;

public sealed class AppMailService : CommonMailService
{
    public AppMailService(IEmailService emailService)
        : base(emailService, "My App | -TITLE-")
    {
    }

    public Task SendWelcomeEmailAsync(string emailAddress, string firstName)
    {
        var template = new DevityTemplate("Templates/welcome.html")
            .AddKey("-FIRSTNAME-", firstName);

        var email = new DevityEmail(emailAddress, "Welcome", template);

        return SendEmailAsync(email);
    }
}
```

`subjectFormat` must contain `-TITLE-`, because `CommonMailService` replaces that token with `DevityEmail.SubjectMessage`.

## Register in DI

### With `appsettings.json`

```csharp
builder.Services.AddDevityMailing<AppMailService>(builder.Configuration);
```

The package expects an `Email` configuration section in `MailKitOptions` format.

```json
{
  "Email": {
    "Server": "smtp.example.com",
    "Port": 587,
    "SenderName": "My App",
    "SenderEmail": "noreply@example.com",
    "Account": "smtp-user",
    "Password": "smtp-password",
    "Security": true
  }
}
```

### With explicit options

```csharp
using Devity.NETCore.MailKit.Infrastructure.Internal;

builder.Services.AddDevityMailing<AppMailService>(new MailKitOptions
{
    Server = "smtp.example.com",
    Port = 587,
    SenderName = "My App",
    SenderEmail = "noreply@example.com",
    Account = "smtp-user",
    Password = "smtp-password",
    Security = true
});
```

## Attachments

`DevityEmail` supports chained attachment registration.

```csharp
var email = new DevityEmail("user@example.com", "Invoice", template)
    .AddAttachment("/tmp/invoice.pdf");
```

## Template usage

Email bodies are rendered with `DevityTemplate.PopulateTemplate()`. See [`../Devity.Extensions/README.md`](../Devity.Extensions/README.md) for the full template API.
