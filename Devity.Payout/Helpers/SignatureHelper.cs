using Devity.Payout.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace Devity.Payout.Helpers;

public static class SignatureHelper
{
    public static PayoutCheckoutDTO PopulateSignature(this PayoutCheckoutDTO payoutCheckoutDTO, string clientSecret)
    {
        payoutCheckoutDTO.Nonce = Guid.NewGuid().ToString("N");

        var signatureBase = $"{payoutCheckoutDTO.AmountInCents}|{payoutCheckoutDTO.Currency}|{payoutCheckoutDTO.ExternalId}" +
            $"|{payoutCheckoutDTO.Nonce}|{clientSecret}";

        payoutCheckoutDTO.Signature = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(signatureBase))).ToLower();

        return payoutCheckoutDTO;
    }
    public static PayoutRefundDTO PopulateSignature(this PayoutRefundDTO payoutRefundDTO, string clientSecret)
    {
        payoutRefundDTO.Nonce = Guid.NewGuid().ToString("N");

        var signatureBase = $"{payoutRefundDTO.AmountInCents}|{payoutRefundDTO.Currency}|{payoutRefundDTO.ExternalId}|{string.Empty}" +
            $"|{payoutRefundDTO.Nonce}|{clientSecret}";

        payoutRefundDTO.Signature = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(signatureBase))).ToLower();

        return payoutRefundDTO;
    }

    public static bool IsSignatureValid(this PayoutWebhookDTO payoutWebhookDto, string clientSecret)
    {
        payoutWebhookDto.Nonce = Guid.NewGuid().ToString("N");

        var signatureBase = $"{payoutWebhookDto.ExternalId}|{payoutWebhookDto.Type}" +
            $"|{payoutWebhookDto.Nonce}|{clientSecret}";

        var signature = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(signatureBase))).ToLower();

        return signature.Equals(payoutWebhookDto.Signature);
    }
}