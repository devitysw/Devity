using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutAuthorizationResponseDTO
{
    [JsonPropertyName("valid_for")]
    public int ValidFor { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
