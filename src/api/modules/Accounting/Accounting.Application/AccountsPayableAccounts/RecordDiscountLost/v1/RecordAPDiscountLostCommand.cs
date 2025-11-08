namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

/// <summary>
/// Command to record a discount lost for an accounts payable account.
/// </summary>
public sealed record RecordAPDiscountLostCommand(DefaultIdType Id, decimal DiscountAmount) : IRequest<DefaultIdType>;

