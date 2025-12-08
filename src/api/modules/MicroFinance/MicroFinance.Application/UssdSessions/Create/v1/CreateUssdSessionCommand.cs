using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Create.v1;

public sealed record CreateUssdSessionCommand(
    string SessionId,
    string PhoneNumber,
    string ServiceCode,
    DefaultIdType? MemberId = null,
    DefaultIdType? WalletId = null,
    string? Language = null) : IRequest<CreateUssdSessionResponse>;
