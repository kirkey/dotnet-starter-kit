namespace Accounting.Application.PrepaidExpenses.Cancel.v1;

public sealed record CancelPrepaidExpenseCommand(DefaultIdType Id, string Reason) : IRequest<DefaultIdType>;

