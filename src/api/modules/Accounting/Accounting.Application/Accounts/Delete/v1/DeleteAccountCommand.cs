using MediatR;

namespace Accounting.Application.Accounts.Delete.v1;
public sealed record DeleteAccountCommand(
    DefaultIdType Id) : IRequest;
