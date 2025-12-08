using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordContact.v1;

public sealed record RecordContactCommand(
    DefaultIdType Id,
    DateOnly ContactDate,
    DateOnly? NextFollowUp = null) : IRequest<RecordContactResponse>;
