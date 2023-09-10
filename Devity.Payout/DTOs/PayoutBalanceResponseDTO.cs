using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutBalanceResponseDTO
{
    [JsonPropertyName("available")]
    public double Available { get; set; }

    [JsonPropertyName("pending")]
    public double Pending { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}
