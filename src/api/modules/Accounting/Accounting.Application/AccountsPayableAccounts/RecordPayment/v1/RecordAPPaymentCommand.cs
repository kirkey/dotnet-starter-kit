namespace Accounting.Application.AccountsPayableAccounts.RecordPayment.v1;

public sealed record RecordAPPaymentCommand(
    DefaultIdType Id, 
    decimal Amount, 
    bool DiscountTaken, 
    decimal DiscountAmount
) : IRequest<DefaultIdType>;

