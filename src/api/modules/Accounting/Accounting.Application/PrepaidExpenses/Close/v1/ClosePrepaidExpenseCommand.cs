namespace Accounting.Application.PrepaidExpenses.Close.v1;

public sealed record ClosePrepaidExpenseCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

