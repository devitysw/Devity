using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutCheckoutResponseDTO
{
    [JsonPropertyName("id")]
    public required int PayoutId { get; set; }

    [JsonPropertyName("amount")]
    public required double AmountInCents { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

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

    [JsonPropertyName("checkout_url")]
    public required string CheckoutUrl { get; set; }

    [JsonPropertyName("object")]
    public required string ObjectType { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("idempotency_key")]
    public string? IdempotencyKey { get; set; }

    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }

    [JsonPropertyName("products")]
    public List<PayoutProductDTO> Products { get; set; } = new();
}
