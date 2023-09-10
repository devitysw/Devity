using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutCheckoutDTO
{
    [JsonPropertyName("amount")]
    public required double Amount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; } = "EUR";

    [JsonPropertyName("external_id")]
    public required string ExternalId { get; set; }

    [JsonPropertyName("customer")]
    public required PayoutCustomerDTO Customer { get; set; }

    [JsonPropertyName("billing_address")]
    public required PayoutAddressDTO BillingAddress { get; set; }

    [JsonPropertyName("shipping_address")]
    public required PayoutAddressDTO ShippingAddress { get; set; }

    [JsonPropertyName("signature")]
    public required string Signature { get; set; }

    [JsonPropertyName("nonce")]
    public required string Nonce { get; set; }

    [JsonPropertyName("redirect_url")]
    public required string RedirectUrl { get; set; }

    [JsonPropertyName("pisp_consent")]
    public bool? PispConsent { get; set; }

    [JsonPropertyName("idempotency_key")]
    public string? IdempotencyKey { get; set; }

    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }

    [JsonPropertyName("metadata")]
    public string? Mode { get; set; }

    [JsonPropertyName("recurrent_token")]
    public string? RecurrentToken { get; set; }

    [JsonPropertyName("products")]
    public List<PayoutProductDTO> Products { get; set; } = new();
}
