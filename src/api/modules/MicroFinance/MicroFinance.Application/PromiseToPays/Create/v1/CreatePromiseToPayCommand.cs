using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Create.v1;

/// <summary>
/// Command to create a new promise to pay.
/// </summary>
public sealed record CreatePromiseToPayCommand(
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    DateOnly PromisedPaymentDate,
    decimal PromisedAmount,
    DefaultIdType RecordedById,
    DefaultIdType? CollectionActionId = null) : IRequest<CreatePromiseToPayResponse>;
