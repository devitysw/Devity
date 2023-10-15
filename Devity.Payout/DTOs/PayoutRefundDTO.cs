using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutRefundDTO
{
    [JsonPropertyName("amount")]
    public required double AmountInCents { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("checkout_id")]
    public required string CheckoutId { get; set; }

    [JsonPropertyName("external_id")]
    public required string ExternalId { get; set; }

    [JsonPropertyName("signature")]
    public string? Signature { get; set; }

    [JsonPropertyName("nonce")]
    public string? Nonce { get; set; }

    [JsonPropertyName("iban")]
    private string Iban { get; set; } = string.Empty;

    [JsonPropertyName("statement_descriptor")]
    public required string StatementDescriptor { get; set; }
}
