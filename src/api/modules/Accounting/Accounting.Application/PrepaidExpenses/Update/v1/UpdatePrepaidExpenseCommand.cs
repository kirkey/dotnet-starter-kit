namespace Accounting.Application.PrepaidExpenses.Update.v1;

public sealed record UpdatePrepaidExpenseCommand(
    DefaultIdType Id,
    string? Description = null,
    DateTime? EndDate = null,
    DefaultIdType? CostCenterId = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
