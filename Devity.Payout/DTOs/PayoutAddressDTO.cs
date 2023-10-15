using System.Text.Json.Serialization;

namespace Devity.Payout.DTOs;

public class PayoutAddressDTO
{
    [JsonPropertyName("name")]
    public required string FullName { get; set; }

    [JsonPropertyName("address_line_1")]
    public required string AddressLineOne { get; set; }

    [JsonPropertyName("address_line_2")]
    public required string AddressLineTwo { get; set; }

    [JsonPropertyName("postal_code")]
    public required string PostalCode { get; set; }

    [JsonPropertyName("country_code")]
    public required string CountryCode { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }
}
