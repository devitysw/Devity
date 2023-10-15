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
    [Display(Name = "checkout.refund_request")]
    CheckoutRefundRequest,
    [Display(Name = "checkout.refunded")]
    CheckoutRefunded,
    [Display(Name = "checkout.refund_failed")]
    CheckoutRefundFailed,
}