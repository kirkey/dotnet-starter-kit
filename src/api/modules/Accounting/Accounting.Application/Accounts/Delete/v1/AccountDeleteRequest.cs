using MediatR;

namespace Accounting.Application.Accounts.Delete.v1;
public sealed record AccountDeleteRequest(
    DefaultIdType Id) : IRequest;
