namespace Devity.Payout.Exceptions;

public class PayoutBalanceUnavailableException : Exception
{
    public PayoutBalanceUnavailableException(string message) : base(message)
    {
    }
}
