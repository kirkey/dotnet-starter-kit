using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Reopen.v1;

/// <summary>
/// Command to reopen an accounting period.
/// </summary>
public sealed record AccountingPeriodReopenCommand(DefaultIdType Id) : IRequest<AccountingPeriodTransitionResponse>;
