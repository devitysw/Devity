using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutPaymentDTO
{
    [JsonPropertyName("failure_reason")]
    public required string FailureReason { get; set; }

    [JsonPropertyName("fee")]
    public required double FeeInCents { get; set; }

    [JsonPropertyName("net")]
    public required double NetInCents { get; set; }

    [JsonPropertyName("object")]
    public required double Object { get; set; }

    [JsonPropertyName("payment_method")]
    public required double PaymentMethod { get; set; }

    [JsonPropertyName("status")]
    public required double Status { get; set; }
}
