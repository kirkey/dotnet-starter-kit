using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Create.v1;

public sealed record CreateLegalActionCommand(
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    string ActionType,
    decimal ClaimAmount) : IRequest<CreateLegalActionResponse>;
