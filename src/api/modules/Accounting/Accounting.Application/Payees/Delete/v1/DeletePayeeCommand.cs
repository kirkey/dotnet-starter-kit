namespace Accounting.Application.Payees.Delete.v1;
public sealed record DeletePayeeCommand(
    DefaultIdType Id) : IRequest;
