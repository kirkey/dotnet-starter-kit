namespace Accounting.Application.RetainedEarnings.Reopen.v1;

public sealed record ReopenRetainedEarningsCommand(DefaultIdType Id, string Reason) : IRequest<DefaultIdType>;

