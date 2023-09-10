using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutCustomerDTO
{
    [JsonPropertyName("first_name")]
    public required string Firstname { get; set; }

    [JsonPropertyName("last_name")]
    public required string Lastname { get; set; }

    [JsonPropertyName("email")]
    public required string EmailAddress { get; set; }

    [JsonPropertyName("phone")]
    public required string PhoneNumber { get; set; }
}
