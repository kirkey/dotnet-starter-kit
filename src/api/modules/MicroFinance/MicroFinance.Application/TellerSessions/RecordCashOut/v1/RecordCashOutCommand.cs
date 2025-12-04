using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashOut.v1;

public sealed record RecordCashOutCommand(Guid Id, decimal Amount) : IRequest<RecordCashOutResponse>;
