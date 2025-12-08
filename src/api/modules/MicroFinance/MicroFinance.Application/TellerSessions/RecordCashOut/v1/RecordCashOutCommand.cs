using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashOut.v1;

public sealed record RecordCashOutCommand(DefaultIdType Id, decimal Amount) : IRequest<RecordCashOutResponse>;
