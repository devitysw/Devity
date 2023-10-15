using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutWebhookDTO
{
    [JsonPropertyName("data")]
    public PayoutWebhookDataDTO? Data { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("signature")]
    public string? Signature { get; set; }

    [JsonPropertyName("nonce")]
    public string? Nonce { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("object")]
    public string? Object { get; set; }
}
