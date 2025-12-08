using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashIn.v1;

public sealed record RecordCashInCommand(DefaultIdType Id, decimal Amount) : IRequest<RecordCashInResponse>;
