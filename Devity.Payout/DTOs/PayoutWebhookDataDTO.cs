using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutWebhookDataDTO
{
    [JsonPropertyName("amount")]
    public double AmountInCents { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "EUR";

    [JsonPropertyName("customer")]
    public PayoutCustomerDTO? Customer { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("payment")]
    public PayoutPaymentDTO? Payment { get; set; }
}