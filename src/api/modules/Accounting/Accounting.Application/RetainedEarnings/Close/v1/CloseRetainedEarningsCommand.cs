namespace Accounting.Application.RetainedEarnings.Close.v1;

public sealed record CloseRetainedEarningsCommand(DefaultIdType Id, string ClosedBy) : IRequest<DefaultIdType>;

