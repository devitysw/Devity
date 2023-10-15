using System.ComponentModel.DataAnnotations;

public enum PayoutWebhookType
{
    [Display(Name = "withdrawal.paid")]
    WithdrawalPaid,
    [Display(Name = "withdrawal.failed")]
    WithdrawalFailed,
    [Display(Name = "checkout.succeeded")]
    CheckoutSucceeded,
    [Display(Name = "checkout.expired")]
    CheckoutExpired,
}