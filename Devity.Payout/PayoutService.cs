﻿namespace Devity.Payout;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;
using Devity.Payout.DTOs;

public class PayoutService
{
    private readonly string _payoutUrl;
    private readonly string _authorizePath;
    private readonly string _withdrawPath;
    private readonly string _balancePath;
    private readonly string _checkoutPath;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private const int MAX_ATTEMPTS = 2;

    private readonly IConfiguration _configuration;

    private PayoutAuthorizationResponseDTO? _lastAuthorization;
    private DateTime? _lastAuthorizationAt;

    public PayoutService(IConfiguration configuration)
    {
        _configuration = configuration;

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
        _withdrawPath = _payoutUrl + "/withdrawals";
        _balancePath = _payoutUrl + "/balance";
        _checkoutPath = _payoutUrl + "/checkouts";
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

    public async Task WithdrawAsync(int transactionId)
    {
        var token = await GetTokenAsync()!;

        using var scope = _serviceProvider.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>()!;
        using var db = dbContextFactory.CreateDbContext();

        var transaction = await db.Transactions
            .Include(x => x.BankAccount)
            .Include(x => x.Employee)
            .ThenInclude(x => x!.Account)
            .AsNoTracking()
            .SingleAsync(x => x.Id.Equals(transactionId));

        using var httpClient = new HttpClient();

        var data = new PayoutWithdrawDTO
        {
            Amount = Convert.ToInt64(transaction.Amount * 100),
            Customer = new PayoutCustomerDTO
            {
                EmailAddress = transaction.Employee!.Account!.Email,
                Firstname = transaction.Employee.Account.Firstname,
                Lastname = transaction.Employee.Account.Surname
            },
            Descriptor = "EWA withdrawal",
            ExternalId = transaction.Id.ToString(),
            Iban = transaction.BankAccount!.Iban
        };

        data = SignatureHelper.PopulateSignature(data, _configuration["Payout:ClientSecret"]);

        var jsonString = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            response = await httpClient.PostAsync(_withdrawPath, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed to submit withdraw request on attempt {i + 1} out of {MAX_ATTEMPTS}: {response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}"
                );

                _lastAuthorization = null;
                token = await GetTokenAsync();
            }
            else
                break;
        }

        response!.EnsureSuccessStatusCode();

        _logger.LogInformation(
            $"Withdrawal of {transaction.Amount} EUR for user {transaction.Employee.Account.Email} sent successfully."
        );
    }

    public async Task CreateCheckoutAsync()
    {
        var token = await GetTokenAsync();
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
