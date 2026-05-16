# Devity.Payout

`Devity.Payout` is a small wrapper around the Payout API. It handles authorization, checkout creation, refunds, balance retrieval, and webhook signature validation.

## Installation

```powershell
Install-Package Devity.Payout
```

## Configuration

`PayoutService` reads its settings from configuration:

```json
{
  "Payout": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "Url": "https://api.example.com"
  }
}
```

Required keys:

- `Payout:ClientId`
- `Payout:ClientSecret`
- `Payout:Url`

## Register the service

The package does not ship its own DI extension method, so register `PayoutService` in your application.

```csharp
builder.Services.AddScoped<PayoutService>();
```

## Main operations

- `GetTokenAsync()`: gets and caches an authorization token.
- `CreateCheckoutAsync(PayoutCheckoutDTO)`: creates a checkout.
- `GetCheckoutAsync(int checkoutId)`: fetches a checkout by ID.
- `CreateRefundAsync(PayoutRefundDTO)`: creates a refund.
- `GetBalanceAsync()`: gets balance data.

## Create a checkout

```csharp
using Devity.Payout;
using Devity.Payout.DTOs;

var response = await payoutService.CreateCheckoutAsync(new PayoutCheckoutDTO
{
    AmountInCents = 1999,
    ExternalId = "order-1001",
    RedirectUrl = "https://example.com/payment/complete",
    Customer = new PayoutCustomerDTO
    {
        Firstname = "Dave",
        Lastname = "Smith",
        EmailAddress = "dave@example.com"
    },
    BillingAddress = new PayoutAddressDTO
    {
        FullName = "Dave Smith",
        AddressLineOne = "Main street 1",
        AddressLineTwo = "Apartment 2",
        City = "Prague",
        PostalCode = "11000",
        CountryCode = "CZ"
    },
    ShippingAddress = new PayoutAddressDTO
    {
        FullName = "Dave Smith",
        AddressLineOne = "Main street 1",
        AddressLineTwo = "Apartment 2",
        City = "Prague",
        PostalCode = "11000",
        CountryCode = "CZ"
    },
    Products =
    [
        new PayoutProductDTO
        {
            Name = "Starter plan",
            UnitPriceInCents = 1999,
            Quantity = 1
        }
    ]
});

var checkoutUrl = response.CheckoutUrl;
```

The service generates the checkout signature and nonce for you before sending the request.

## Refunds

```csharp
var refund = await payoutService.CreateRefundAsync(new PayoutRefundDTO
{
    AmountInCents = 1999,
    Currency = "EUR",
    CheckoutId = "12345",
    ExternalId = "refund-1001",
    StatementDescriptor = "Order refund"
});
```

If the API returns a bad request containing `balance is not available yet`, the service throws `PayoutBalanceUnavailableException`.

## Balance

```csharp
var balances = await payoutService.GetBalanceAsync();

foreach (var balance in balances)
{
    Console.WriteLine($"{balance.Currency}: available={balance.Available}, pending={balance.Pending}");
}
```

## Webhook signature validation

Use `IsSignatureValid` to verify incoming webhook payloads.

```csharp
using Devity.Payout.DTOs;
using Devity.Payout.Helpers;

if (!webhook.IsSignatureValid(builder.Configuration["Payout:ClientSecret"]!))
{
    return Results.Unauthorized();
}
```

Related types:

- `PayoutWebhookDTO`
- `PayoutWebhookDataDTO`
- `PayoutWebhookType`

`PayoutWebhookType` contains the currently modeled webhook event names as `Display`-annotated enum values.
