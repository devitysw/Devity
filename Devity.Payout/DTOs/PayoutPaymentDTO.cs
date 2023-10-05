using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutPaymentDTO
{
    [JsonPropertyName("failure_reason")]
    public string? FailureReason { get; set; }

    [JsonPropertyName("fee")]
    public double? Fee { get; set; }
}