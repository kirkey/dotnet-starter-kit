using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Close.v1;

/// <summary>
/// Command to close an accounting period.
/// </summary>
public sealed record CloseAccountingPeriodCommand(DefaultIdType Id) : IRequest<AccountingPeriodTransitionResponse>;
