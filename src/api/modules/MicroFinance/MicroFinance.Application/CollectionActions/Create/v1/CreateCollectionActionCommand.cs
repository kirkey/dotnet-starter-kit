using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;

/// <summary>
/// Command to create a new collection action.
/// </summary>
public sealed record CreateCollectionActionCommand(
    Guid CollectionCaseId,
    Guid LoanId,
    string ActionType,
    Guid PerformedById,
    string Outcome,
    string? Description = null) : IRequest<CreateCollectionActionResponse>;
