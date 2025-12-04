using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;

public sealed class GetLegalActionHandler(
    [FromKeyedServices("microfinance:legalactions")] IReadRepository<LegalAction> repository)
    : IRequestHandler<GetLegalActionRequest, LegalActionResponse>
{
    public async Task<LegalActionResponse> Handle(
        GetLegalActionRequest request,
        CancellationToken cancellationToken)
    {
        var action = await repository.FirstOrDefaultAsync(
            new LegalActionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Legal action {request.Id} not found");

        return new LegalActionResponse(
            action.Id,
            action.CollectionCaseId,
            action.LoanId,
            action.MemberId,
            action.CaseReference,
            action.ActionType,
            action.Status,
            action.InitiatedDate,
            action.FiledDate,
            action.NextHearingDate,
            action.JudgmentDate,
            action.ClosedDate,
            action.CourtName,
            action.LawyerName,
            action.ClaimAmount,
            action.JudgmentAmount,
            action.AmountRecovered,
            action.LegalCosts,
            action.CourtFees,
            action.JudgmentSummary);
    }
}
