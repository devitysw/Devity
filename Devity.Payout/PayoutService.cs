namespace Devity.Payout;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Devity.Payout.DTOs;
using Microsoft.Extensions.Logging;
using Devity.Payout.Helpers;
using Devity.Payout.Exceptions;

public class PayoutService
{
    private readonly string _payoutUrl;
    private readonly string _authorizePath;
    private readonly string _balancePath;
    private readonly string _checkoutPath;
    private readonly string _refundPath;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private const int MAX_ATTEMPTS = 2;

    private readonly IConfiguration _configuration;
    private readonly ILogger<PayoutService> _logger;

    private PayoutAuthorizationResponseDTO? _lastAuthorization;
    private DateTime? _lastAuthorizationAt;

    public PayoutService(IConfiguration configuration, ILogger<PayoutService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var clientId = _configuration["Payout:ClientId"];
        var clientSecret = _configuration["Payout:ClientSecret"];
        var payoutUrl = _configuration["Payout:Url"];

        if (clientId is null || clientSecret is null || payoutUrl is null)
            throw new Exception(
                "Payout credentials not configured. Payout block in settings must contain ClientId, ClientSecret and Url"
            );

        _clientId = clientId;
        _clientSecret = clientSecret;
        _payoutUrl = payoutUrl;
        _authorizePath = _payoutUrl + "/authorize";
        _balancePath = _payoutUrl + "/balance";
        _checkoutPath = _payoutUrl + "/checkouts";
        _refundPath = _payoutUrl + "/refunds";
    }

    public async Task<string> GetTokenAsync()
    {
        if (_lastAuthorization is not null && _lastAuthorizationAt.HasValue)
        {
            var validUntil = _lastAuthorizationAt.Value.AddSeconds(_lastAuthorization.ValidFor);

            if (validUntil > DateTime.Now)
                return _lastAuthorization.Token;
        }

        using var httpClient = new HttpClient();

        var authorizationData = new PayoutAuthorizationDTO
        {
            ClientId = _clientId,
            ClientSecret = _clientSecret
        };

        var response = await httpClient.PostAsJsonAsync(_authorizePath, authorizationData);

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Failed to authorize for Payout: {response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}"
            );

        _lastAuthorizationAt = DateTime.Now;

        var responseDto =
            await response.Content.ReadFromJsonAsync<PayoutAuthorizationResponseDTO>();

        if (responseDto is null)
            throw new Exception($"Failed to get token: {response.Content}");

        _lastAuthorization = responseDto;

        return _lastAuthorization.Token;
    }

    public async Task<PayoutCheckoutResponseDTO> CreateCheckoutAsync(
        PayoutCheckoutDTO payoutCheckoutDTO
    )
    {
        var token = await GetTokenAsync()!;

        using var httpClient = new HttpClient();

        var data = payoutCheckoutDTO.PopulateSignature(_clientSecret);

        var jsonString = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            response = await httpClient.PostAsync(_checkoutPath, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed to create checkout on attempt {i + 1} out of {MAX_ATTEMPTS}: {response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}"
                );

                _lastAuthorization = null;
                token = await GetTokenAsync();
            }
            else
                break;
        }

        response!.EnsureSuccessStatusCode();

        _logger.LogInformation(
            $"Checkout of {payoutCheckoutDTO.AmountInCents / 100} EUR for user {payoutCheckoutDTO.Customer.Firstname} {payoutCheckoutDTO.Customer.Lastname} [{payoutCheckoutDTO.Customer.EmailAddress}] created successfully."
        );

        return (await response.Content.ReadFromJsonAsync<PayoutCheckoutResponseDTO>())!;
    }

    public async Task<PayoutCheckoutDTO> GetCheckoutAsync(
        int checkoutId
    )
    {
        var token = await GetTokenAsync()!;

        using var httpClient = new HttpClient();

        HttpResponseMessage? response = null;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            response = await httpClient.GetAsync($"{_checkoutPath}/{checkoutId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed to get checkout on attempt {i + 1} out of {MAX_ATTEMPTS}: {response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}"
                );

                _lastAuthorization = null;
                token = await GetTokenAsync();
            }
            else
                break;
        }

        response!.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<PayoutCheckoutDTO>())!;
    }

    public async Task<PayoutRefundResponseDTO> CreateRefundAsync(
        PayoutRefundDTO payoutRefundDTO
    )
    {
        var token = await GetTokenAsync()!;

        using var httpClient = new HttpClient();

        var data = payoutRefundDTO.PopulateSignature(_clientSecret);

        var jsonString = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            response = await httpClient.PostAsync(_refundPath, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed to create refund on attempt {i + 1} out of {MAX_ATTEMPTS}: {response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}"
                );

                _lastAuthorization = null;
                token = await GetTokenAsync();
            }
            else
                break;
        }

        try
        {
            response!.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            var message = await response!.Content.ReadAsStringAsync();

            if (response!.StatusCode == System.Net.HttpStatusCode.BadRequest && message.Contains("balance is not available yet"))
                throw new PayoutBalanceUnavailableException(message);
            else throw;
        }

        _logger.LogInformation(
            $"Refund of {payoutRefundDTO.AmountInCents / 100} EUR for checkout ID [{payoutRefundDTO.CheckoutId}] created successfully."
        );

        return (await response.Content.ReadFromJsonAsync<PayoutRefundResponseDTO>())!;
    }

    public async Task<PayoutBalanceResponseDTO[]> GetBalanceAsync()
    {
        var token = await GetTokenAsync();

        using var httpClient = new HttpClient();

        HttpResponseMessage? response = null;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            response = await httpClient.GetAsync(_balancePath);

            if (!response.IsSuccessStatusCode)
            {
                _lastAuthorization = null;
                token = await GetTokenAsync();
            }
            else
                break;
        }

        response!.EnsureSuccessStatusCode();

        var balanceData = await response.Content.ReadFromJsonAsync<PayoutBalanceResponseDTO[]>();

        return balanceData!;
    }
}
