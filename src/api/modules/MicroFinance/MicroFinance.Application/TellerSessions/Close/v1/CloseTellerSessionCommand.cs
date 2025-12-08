using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Close.v1;

public sealed record CloseTellerSessionCommand(
    DefaultIdType Id,
    decimal ActualClosingBalance,
    string? ClosingDenominations = null) : IRequest<CloseTellerSessionResponse>;
