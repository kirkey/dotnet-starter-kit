using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Create.v1;

/// <summary>
/// Command to create a new promise to pay.
/// </summary>
public sealed record CreatePromiseToPayCommand(
    Guid CollectionCaseId,
    Guid LoanId,
    Guid MemberId,
    DateOnly PromisedPaymentDate,
    decimal PromisedAmount,
    Guid RecordedById,
    Guid? CollectionActionId = null) : IRequest<CreatePromiseToPayResponse>;
