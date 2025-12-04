using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashIn.v1;

public sealed record RecordCashInCommand(Guid Id, decimal Amount) : IRequest<RecordCashInResponse>;
