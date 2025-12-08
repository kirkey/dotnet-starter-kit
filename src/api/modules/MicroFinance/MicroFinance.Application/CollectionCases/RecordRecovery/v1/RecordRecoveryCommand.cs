using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;

public sealed record RecordCollectionCaseRecoveryCommand(DefaultIdType Id, decimal Amount) : IRequest<RecordCollectionCaseRecoveryResponse>;
