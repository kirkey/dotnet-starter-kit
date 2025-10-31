using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Get;

/// <summary>
/// Request to get a prepaid expense by ID.
/// </summary>
public record GetPrepaidExpenseRequest(DefaultIdType Id) : IRequest<PrepaidExpenseResponse>;
