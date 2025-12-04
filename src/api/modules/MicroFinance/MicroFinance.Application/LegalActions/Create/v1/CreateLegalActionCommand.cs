using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Create.v1;

public sealed record CreateLegalActionCommand(
    Guid CollectionCaseId,
    Guid LoanId,
    Guid MemberId,
    string ActionType,
    decimal ClaimAmount) : IRequest<CreateLegalActionResponse>;
