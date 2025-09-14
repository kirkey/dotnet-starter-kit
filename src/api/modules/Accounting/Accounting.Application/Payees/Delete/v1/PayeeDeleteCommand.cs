namespace Accounting.Application.Payees.Delete.v1;
public sealed record PayeeDeleteCommand(
    DefaultIdType Id) : IRequest;
