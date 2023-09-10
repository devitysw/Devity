using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutAuthorizationDTO
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;
}
