using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Create.v1;

public sealed class CreateLegalActionHandler(
    [FromKeyedServices("microfinance:legalactions")] IRepository<LegalAction> repository,
    ILogger<CreateLegalActionHandler> logger)
    : IRequestHandler<CreateLegalActionCommand, CreateLegalActionResponse>
{
    public async Task<CreateLegalActionResponse> Handle(
        CreateLegalActionCommand request,
        CancellationToken cancellationToken)
    {
        var legalAction = LegalAction.Create(
            request.CollectionCaseId,
            request.LoanId,
            request.MemberId,
            request.ActionType,
            request.ClaimAmount);

        await repository.AddAsync(legalAction, cancellationToken);

        logger.LogInformation("Legal action created: {LegalActionId} for loan {LoanId}",
            legalAction.Id, request.LoanId);

        return new CreateLegalActionResponse(legalAction.Id);
    }
}
