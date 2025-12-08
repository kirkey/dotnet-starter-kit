using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;

public sealed record AssignCollectionCaseCommand(
    DefaultIdType Id,
    DefaultIdType CollectorId,
    DateOnly? FollowUpDate = null) : IRequest<AssignCollectionCaseResponse>;
