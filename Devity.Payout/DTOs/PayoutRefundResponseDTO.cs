using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutRefundResponseDTO
{
    [JsonPropertyName("id")]
    public required int PayoutId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("customer")]
    public required PayoutCustomerDTO Customer { get; set; }

    [JsonPropertyName("external_id")]
    public required string ExternalId { get; set; }

    [JsonPropertyName("amount")]
    public required double AmountInCents { get; set; }

    [JsonPropertyName("iban")]
    public required string Iban { get; set; }

    [JsonPropertyName("signature")]
    public required string Signature { get; set; }

    [JsonPropertyName("nonce")]
    public required string Nonce { get; set; }

    [JsonPropertyName("object")]
    public required string ObjectType { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }
}
