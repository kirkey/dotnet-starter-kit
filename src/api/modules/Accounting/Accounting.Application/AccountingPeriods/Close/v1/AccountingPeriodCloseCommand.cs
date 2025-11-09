using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Close.v1;

/// <summary>
/// Command to close an accounting period.
/// </summary>
public sealed record AccountingPeriodCloseCommand(DefaultIdType Id) : IRequest<AccountingPeriodTransitionResponse>;
