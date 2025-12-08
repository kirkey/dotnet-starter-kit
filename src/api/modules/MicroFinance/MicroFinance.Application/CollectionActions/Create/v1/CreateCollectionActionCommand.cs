using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;

/// <summary>
/// Command to create a new collection action.
/// </summary>
public sealed record CreateCollectionActionCommand(
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    string ActionType,
    DefaultIdType PerformedById,
    string Outcome,
    string? Description = null) : IRequest<CreateCollectionActionResponse>;
