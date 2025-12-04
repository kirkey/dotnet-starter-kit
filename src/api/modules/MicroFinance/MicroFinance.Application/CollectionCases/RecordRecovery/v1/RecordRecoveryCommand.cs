using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;

public sealed record RecordRecoveryCommand(Guid Id, decimal Amount) : IRequest<RecordRecoveryResponse>;
