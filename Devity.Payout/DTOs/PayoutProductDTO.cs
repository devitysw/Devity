using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutProductDTO
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("unit_price")]
    public required int UnitPriceInCents { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }

    [JsonPropertyName("date")]
    public DateTime? DateTime { get; set; }
}
